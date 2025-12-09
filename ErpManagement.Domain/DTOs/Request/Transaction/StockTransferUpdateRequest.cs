using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockTransferUpdateRequest
{
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int Id { get; set; }

    [Display(Name = Annotations.Warehouse)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int FromWarehouseId { get; set; }

    [Display(Name = Annotations.Warehouse)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int ToWarehouseId { get; set; }

    public DateTime TransferDate { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string? TransferCode { get; set; }

    [MaxLength(500)]
    public string? Remark { get; set; }

    [MaxLength(1000)]
    public string? TransferNote { get; set; }

    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public List<StockTransferItemCreateRequest> Items { get; set; } = new();
}