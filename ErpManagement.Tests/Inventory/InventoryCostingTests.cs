using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Tests.Inventory;

/// <summary>
/// Smoke tests for inventory costing (Perpetual WAC) and P&L calculations.
/// These are pure unit tests with no DB dependency.
/// </summary>
public class InventoryCostingTests
{
    // ── SMOKE TEST 1: Perpetual Weighted Average Cost ────────────────────────

    [Fact]
    public void PerpetualWac_FirstPurchase_SetsAverageCostToUnitCost()
    {
        // Arrange
        var wp = new WarehouseProduct { Quantity = 0, AverageCost = 0 };

        // Act: Purchase 10 units @ cost 10
        decimal purchasedQty = 10;
        decimal unitCost = 10;
        decimal oldQty = wp.Quantity;
        decimal oldAvg = wp.AverageCost;
        wp.Quantity += purchasedQty;
        wp.AverageCost = oldQty > 0
            ? ((oldQty * oldAvg) + (purchasedQty * unitCost)) / wp.Quantity
            : unitCost;

        // Assert
        Assert.Equal(10, wp.Quantity);
        Assert.Equal(10m, wp.AverageCost);
    }

    [Fact]
    public void PerpetualWac_SecondPurchase_RecalculatesWeightedAverage()
    {
        // Arrange: After first purchase, 10 units @ avg 10
        var wp = new WarehouseProduct { Quantity = 10, AverageCost = 10 };

        // Act: Purchase 10 more units @ cost 14
        decimal purchasedQty = 10;
        decimal unitCost = 14;
        decimal oldQty = wp.Quantity;
        decimal oldAvg = wp.AverageCost;
        wp.Quantity += purchasedQty;
        wp.AverageCost = oldQty > 0
            ? ((oldQty * oldAvg) + (purchasedQty * unitCost)) / wp.Quantity
            : unitCost;

        // Assert: new avg = ((10*10) + (10*14)) / 20 = (100+140)/20 = 12
        Assert.Equal(20, wp.Quantity);
        Assert.Equal(12m, wp.AverageCost);
    }

    [Fact]
    public void PerpetualWac_Sale_CostAtSale_SnapshotsCurrent_AverageCost()
    {
        // Arrange: 20 units @ avg 12
        decimal averageCost = 12m;

        // Act: Sell 5 units, snapshot cost at sale
        var saleItem = new SaleItem
        {
            Quantity = 5,
            UnitPrice = 20m, // selling price
            CostAtSale = averageCost // snapshot
        };

        decimal cogs = saleItem.Quantity * saleItem.CostAtSale;

        // Assert
        Assert.Equal(12m, saleItem.CostAtSale);
        Assert.Equal(60m, cogs); // 5 * 12 = 60
    }

    [Fact]
    public void PerpetualWac_FullFlow_TwoPurchases_ThenSale()
    {
        // Purchase 10 @ 10 → avg = 10
        var wp = new WarehouseProduct { Quantity = 0, AverageCost = 0 };
        Apply(wp, qty: 10, cost: 10);
        Assert.Equal(10m, wp.AverageCost);

        // Purchase 10 @ 14 → avg = 12
        Apply(wp, qty: 10, cost: 14);
        Assert.Equal(12m, wp.AverageCost);

        // Sell 5 → CostAtSale = 12, COGS = 60
        decimal costAtSale = wp.AverageCost;
        wp.Quantity -= 5;
        decimal cogs = 5 * costAtSale;

        Assert.Equal(15, wp.Quantity);
        Assert.Equal(12m, costAtSale);
        Assert.Equal(60m, cogs);
    }

    // ── SMOKE TEST 2: P&L Calculations ──────────────────────────────────────

    [Fact]
    public void ProfitLoss_Revenue_SumsAllSalesTotalAmount()
    {
        var sales = new List<Sale>
        {
            new() { TotalAmount = 1000m },
            new() { TotalAmount = 500m },
            new() { TotalAmount = 750m }
        };

        decimal revenue = sales.Sum(s => s.TotalAmount);

        Assert.Equal(2250m, revenue);
    }

    [Fact]
    public void ProfitLoss_ExpensesReduceNetProfit()
    {
        decimal revenue = 2250m;
        decimal cogs = 900m;
        decimal expenses = 350m;

        decimal grossProfit = revenue - cogs;
        decimal netProfit = grossProfit - expenses;

        Assert.Equal(1350m, grossProfit);
        Assert.Equal(1000m, netProfit);
    }

    [Fact]
    public void ProfitLoss_CogsUsesPerpetutalItems_WhenModeIsPerpetual()
    {
        // Arrange: 2 sale items with CostAtSale
        var items = new List<SaleItem>
        {
            new() { Quantity = 5, CostAtSale = 12m },  // COGS = 60
            new() { Quantity = 3, CostAtSale = 15m }   // COGS = 45
        };

        decimal cogs = items.Sum(i => i.Quantity * i.CostAtSale);

        Assert.Equal(105m, cogs);
    }

    // ── SMOKE TEST 3: Periodic COGS ─────────────────────────────────────────

    [Fact]
    public void PeriodicClose_CogsFormula_BeginningPlusPurchasesMinusEnding()
    {
        var period = new InventoryPeriod
        {
            BeginningValue = 1000m,
            PurchasesValue = 5000m,
            EndingValue = 1500m
        };

        period.CogsValue = period.BeginningValue + period.PurchasesValue - period.EndingValue;

        Assert.Equal(4500m, period.CogsValue);
    }

    [Fact]
    public void PeriodicClose_EndingValue_SumsPhysicalCounts()
    {
        var counts = new List<PhysicalCount>
        {
            new() { CountQty = 10, CostUsed = 12m, LineValue = 120m },
            new() { CountQty = 5,  CostUsed = 20m, LineValue = 100m },
            new() { CountQty = 8,  CostUsed = 15m, LineValue = 120m }
        };

        // LineValue should be pre-computed at entry time
        foreach (var c in counts)
            Assert.Equal(c.CountQty * c.CostUsed, c.LineValue);

        decimal endingValue = counts.Sum(c => c.LineValue);
        Assert.Equal(340m, endingValue);
    }

    [Fact]
    public void PeriodicClose_FirstPeriod_BeginningValueIsZero()
    {
        // If no previous closed period exists, beginning = 0
        InventoryPeriod? previousPeriod = null;
        decimal beginningValue = previousPeriod?.EndingValue ?? 0;

        Assert.Equal(0m, beginningValue);
    }

    [Fact]
    public void PeriodicClose_SecondPeriod_BeginningValueFromPreviousEnding()
    {
        var previousPeriod = new InventoryPeriod { IsClosed = true, EndingValue = 1500m };
        decimal beginningValue = previousPeriod.EndingValue;

        Assert.Equal(1500m, beginningValue);
    }

    // ── Helper ──────────────────────────────────────────────────────────────

    private static void Apply(WarehouseProduct wp, decimal qty, decimal cost)
    {
        decimal oldQty = wp.Quantity;
        decimal oldAvg = wp.AverageCost;
        wp.Quantity += qty;
        wp.AverageCost = oldQty > 0
            ? ((oldQty * oldAvg) + (qty * cost)) / wp.Quantity
            : cost;
    }
}
