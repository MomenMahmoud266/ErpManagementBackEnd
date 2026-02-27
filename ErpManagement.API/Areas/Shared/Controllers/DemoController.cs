using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.DTOs.Request.Core;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductEntity = ErpManagement.Domain.Models.Products.Product;
using CategoryEntity = ErpManagement.Domain.Models.Products.Category;

namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(SDStatic.Modules.Shared)]
[ApiExplorerSettings(GroupName = SDStatic.Modules.Shared)]
[Authorize(Roles = "Admin,Superadmin")]
[ApiController]
[Route("api/[controller]")]
public class DemoController(ErpManagementDbContext db, ICurrentTenant currentTenant) : ControllerBase
{
    private readonly ErpManagementDbContext _db = db;
    private readonly ICurrentTenant _currentTenant = currentTenant;
    private readonly Random _rng = new(42); // deterministic for repeatability

    /// <summary>
    /// Generates realistic demo data for the current tenant.
    /// ADMIN ONLY. Idempotent-safe — runs inside a single transaction.
    /// </summary>
    [HttpPost("seed")]
    [Produces(typeof(Response<object>))]
    public async Task<IActionResult> Seed([FromBody] DemoSeedRequest request)
    {
        var tenantId = _currentTenant.TenantId;
        if (tenantId <= 0)
            return Unauthorized(Fail("No valid tenant context."));

        request ??= new DemoSeedRequest();

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            // ── 0. Ensure a Country exists ───────────────────────────────────
            var country = await _db.Countries.AsNoTracking().FirstOrDefaultAsync();
            if (country is null)
            {
                var newCountry = new Country { NameEn = "Default Country", Code = "XX", IsActive = true };
                _db.Countries.Add(newCountry);
                await _db.SaveChangesAsync();
                country = newCountry;
            }

            // ── 1. Branches ──────────────────────────────────────────────────
            var branches = new List<Branch>();
            for (var i = 0; i < request.BranchCount; i++)
            {
                branches.Add(new Branch
                {
                    TenantId = tenantId,
                    NameEn = $"Demo Branch {i + 1}",
                    CountryId = country.Id,
                    City = "Demo City",
                    IsActive = true,
                    InsertDate = DateTime.UtcNow
                });
            }
            _db.Branches.AddRange(branches);
            await _db.SaveChangesAsync();

            // ── 2. Warehouses ────────────────────────────────────────────────
            var warehouses = new List<Warehouse>();
            foreach (var branch in branches)
            {
                for (var j = 0; j < request.WarehousesPerBranch; j++)
                {
                    warehouses.Add(new Warehouse
                    {
                        BranchId = branch.Id,
                        Name = $"Warehouse {j + 1} @ {branch.NameEn}",
                        IsActive = true,
                        InsertDate = DateTime.UtcNow
                    });
                }
            }
            _db.Warehouses.AddRange(warehouses);
            await _db.SaveChangesAsync();

            // ── 3. Categories ────────────────────────────────────────────────
            var categoryTitles = new[] { "Electronics", "Clothing", "Food & Beverages", "Household", "Sports & Outdoors" };
            var categories = categoryTitles.Select(t => new CategoryEntity
            {
                TenantId = tenantId,
                Title = t,
                Type = "Product",
                IsActive = true,
                InsertDate = DateTime.UtcNow
            }).ToList();
            _db.Categories.AddRange(categories);
            await _db.SaveChangesAsync();

            // ── 4. Products ──────────────────────────────────────────────────
            var products = new List<ProductEntity>(request.ProductCount);
            for (var i = 0; i < request.ProductCount; i++)
            {
                var cat = categories[i % categories.Count];
                var cost = _rng.Next(5, 200);
                products.Add(new ProductEntity
                {
                    TenantId = tenantId,
                    CategoryId = cat.Id,
                    ProductCode = $"DEMO-{tenantId:D2}-{i + 1:D4}",
                    Title = $"Demo Product {i + 1}",
                    Price = cost * 1.5m + _rng.Next(1, 50),
                    Cost = cost,
                    Quantity = _rng.Next(50, 500),
                    AlertQuantity = 10,
                    IsActive = true,
                    InsertDate = DateTime.UtcNow
                });
            }
            _db.Products.AddRange(products);
            await _db.SaveChangesAsync();

            // ── 5. Customers ─────────────────────────────────────────────────
            var customers = new List<Customer>(request.CustomerCount);
            for (var i = 0; i < request.CustomerCount; i++)
            {
                customers.Add(new Customer
                {
                    TenantId = tenantId,
                    CountryId = country.Id,
                    CustomerCode = $"CST-{tenantId:D2}-{i + 1:D4}",
                    FirstName = $"Customer",
                    LastName = $"{i + 1}",
                    Phone = $"+1555{i:D6}",
                    IsActive = true,
                    InsertDate = DateTime.UtcNow
                });
            }
            _db.Customers.AddRange(customers);
            await _db.SaveChangesAsync();

            // ── 6. Initial stock per warehouse ───────────────────────────────
            const int batchSize = 500;
            var wpBatch = new List<WarehouseProduct>(batchSize);
            foreach (var wh in warehouses)
            {
                foreach (var prod in products)
                {
                    wpBatch.Add(new WarehouseProduct
                    {
                        TenantId = tenantId,
                        WarehouseId = wh.Id,
                        ProductId = prod.Id,
                        Quantity = _rng.Next(20, 200),
                        InsertDate = DateTime.UtcNow
                    });

                    if (wpBatch.Count >= batchSize)
                    {
                        _db.WarehouseProducts.AddRange(wpBatch);
                        await _db.SaveChangesAsync();
                        wpBatch.Clear();
                    }
                }
            }
            if (wpBatch.Count > 0)
            {
                _db.WarehouseProducts.AddRange(wpBatch);
                await _db.SaveChangesAsync();
                wpBatch.Clear();
            }

            // ── 7. Sales + SaleItems + Payments + (occasional) SaleReturns ──
            var salesCreated = 0;
            var paymentsCreated = 0;
            var returnsCreated = 0;

            // Build lookup: branchId → list of warehouses in that branch
            var whByBranch = warehouses.GroupBy(w => w.BranchId)
                                       .ToDictionary(g => g.Key, g => g.ToList());

            var saleBatch = new List<Sale>(batchSize);

            for (var day = 0; day < request.DaysOfSales; day++)
            {
                var saleDate = DateTime.UtcNow.AddDays(-day).Date;

                for (var s = 0; s < request.SalesPerDay; s++)
                {
                    var branch = branches[(day * request.SalesPerDay + s) % branches.Count];
                    var whList = whByBranch[branch.Id];
                    var warehouse = whList[s % whList.Count];
                    var customer = customers[(day * request.SalesPerDay + s) % customers.Count];

                    var itemCount = _rng.Next(1, 6);
                    var items = new List<SaleItem>(itemCount);
                    decimal totalAmount = 0;

                    for (var p = 0; p < itemCount; p++)
                    {
                        var prod = products[_rng.Next(products.Count)];
                        var qty = _rng.Next(1, 5);
                        var unitPrice = prod.Price;
                        var lineTotal = qty * unitPrice;
                        items.Add(new SaleItem
                        {
                            ProductId = prod.Id,
                            Quantity = qty,
                            UnitPrice = unitPrice,
                            Amount = lineTotal,
                            TaxAmount = 0,
                            Discount = 0,
                            TotalAmount = lineTotal,
                            InsertDate = saleDate
                        });
                        totalAmount += lineTotal;
                    }

                    // Payment status: 60% Paid, 20% Partial, 20% Unpaid
                    var payRoll = _rng.Next(100);
                    string payStatus;
                    decimal paidAmt;
                    if (payRoll < 60) { payStatus = "Paid"; paidAmt = totalAmount; }
                    else if (payRoll < 80) { payStatus = "Partial"; paidAmt = Math.Round(totalAmount * (decimal)_rng.NextDouble(), 2); }
                    else { payStatus = "Unpaid"; paidAmt = 0; }

                    var sale = new Sale
                    {
                        TenantId = tenantId,
                        BranchId = branch.Id,
                        WarehouseId = warehouse.Id,
                        CustomerId = customer.Id,
                        SaleCode = $"SL-{saleDate:yyyyMMdd}-{s + 1:D4}",
                        SaleDate = saleDate,
                        Amount = totalAmount,
                        TaxAmount = 0,
                        Discount = 0,
                        ShippingAmount = 0,
                        TotalAmount = totalAmount,
                        SaleStatus = "Completed",
                        PaymentStatus = payStatus,
                        IsActive = true,
                        InsertDate = saleDate,
                        Items = items
                    };

                    saleBatch.Add(sale);
                    salesCreated++;

                    if (saleBatch.Count >= batchSize)
                    {
                        _db.Sales.AddRange(saleBatch);
                        await _db.SaveChangesAsync();

                        // Payments for saved sales
                        var pmts = BuildPayments(saleBatch, tenantId);
                        if (pmts.Count > 0)
                        {
                            _db.Payments.AddRange(pmts);
                            await _db.SaveChangesAsync();
                        }
                        paymentsCreated += pmts.Count;

                        // Occasional sale returns (≈5% of sales)
                        var rets = BuildSaleReturns(saleBatch, tenantId);
                        if (rets.Count > 0)
                        {
                            _db.SaleReturns.AddRange(rets);
                            await _db.SaveChangesAsync();
                        }
                        returnsCreated += rets.Count;
                        saleBatch.Clear();
                    }
                }
            }

            // Flush remainder
            if (saleBatch.Count > 0)
            {
                _db.Sales.AddRange(saleBatch);
                await _db.SaveChangesAsync();

                var pmts = BuildPayments(saleBatch, tenantId);
                if (pmts.Count > 0) { _db.Payments.AddRange(pmts); await _db.SaveChangesAsync(); }
                paymentsCreated += pmts.Count;

                var rets = BuildSaleReturns(saleBatch, tenantId);
                if (rets.Count > 0) { _db.SaleReturns.AddRange(rets); await _db.SaveChangesAsync(); }
                returnsCreated += rets.Count;
                saleBatch.Clear();
            }

            await tx.CommitAsync();

            return Ok(new Response<object>
            {
                IsSuccess = true,
                Message = "Demo data seeded successfully.",
                Data = new
                {
                    TenantId = tenantId,
                    Branches = branches.Count,
                    Warehouses = warehouses.Count,
                    Categories = categories.Count,
                    Products = products.Count,
                    Customers = customers.Count,
                    Sales = salesCreated,
                    Payments = paymentsCreated,
                    SaleReturns = returnsCreated
                }
            });
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return StatusCode(500, Fail($"Demo seeding failed: {ex.Message}"));
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static Response<object> Fail(string msg) => new()
    {
        IsSuccess = false,
        Message = msg,
        Error = msg
    };

    private List<Payment> BuildPayments(IEnumerable<Sale> sales, int tenantId)
    {
        var list = new List<Payment>();
        foreach (var sale in sales)
        {
            if (sale.PaymentStatus == "Unpaid") continue;

            var paid = sale.PaymentStatus == "Paid"
                ? sale.TotalAmount
                : Math.Round(sale.TotalAmount * (decimal)_rng.NextDouble(), 2);

            list.Add(new Payment
            {
                TenantId = tenantId,
                SaleId = sale.Id,
                PaymentCode = $"PMT-{sale.SaleCode}",
                PayableAmount = sale.TotalAmount,
                PaidAmount = paid,
                PaymentDate = sale.SaleDate,
                PaymentType = _rng.Next(2) == 0 ? "Cash" : "Card",
                IsActive = true,
                InsertDate = sale.SaleDate
            });
        }
        return list;
    }

    private List<SaleReturn> BuildSaleReturns(IEnumerable<Sale> sales, int tenantId)
    {
        var list = new List<SaleReturn>();
        var idx = 0;
        foreach (var sale in sales)
        {
            // ~5% return rate
            if (idx % 20 != 0) { idx++; continue; }

            var returnItems = sale.Items
                .Take(1)
                .Select(si => new SaleReturnItem
                {
                    ProductId = si.ProductId,
                    Quantity = 1,
                    UnitPrice = si.UnitPrice,
                    Amount = si.UnitPrice,
                    TaxAmount = 0,
                    Discount = 0,
                    TotalAmount = si.UnitPrice,
                    InsertDate = sale.SaleDate.AddDays(1)
                }).ToList();

            if (returnItems.Count == 0) { idx++; continue; }

            list.Add(new SaleReturn
            {
                TenantId = tenantId,
                CustomerId = sale.CustomerId,
                SaleId = sale.Id,
                WarehouseId = sale.WarehouseId,
                ReturnCode = $"SR-{sale.SaleCode}",
                Amount = returnItems.Sum(r => r.Amount),
                TaxAmount = 0,
                Discount = 0,
                ShippingAmount = 0,
                TotalAmount = returnItems.Sum(r => r.TotalAmount),
                ReturnDate = sale.SaleDate.AddDays(1),
                ReturnStatus = "Completed",
                IsActive = true,
                InsertDate = sale.SaleDate.AddDays(1),
                Items = returnItems
            });
            idx++;
        }
        return list;
    }
}
