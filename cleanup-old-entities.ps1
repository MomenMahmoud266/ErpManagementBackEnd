# PowerShell script to delete old entity files
$oldFiles = @(
    "ErpManagement.Domain\Models\SharProduct.cs",
    "ErpManagement.Domain\Models\SharStock.cs",
    "ErpManagement.Domain\Models\SharState.cs",
    "ErpManagement.Domain\Models\SharCountry.cs",
    "ErpManagement.Domain\Models\HrBranch.cs",
    "ErpManagement.Domain\Models\HrDepartment.cs",
    "ErpManagement.Domain\Models\ComSupplier.cs"
    # Add any other old files here
)

foreach ($file in $oldFiles) {
    if (Test-Path $file) {
        Write-Host "Deleting: $file" -ForegroundColor Yellow
        Remove-Item $file -Force
        Write-Host "Deleted: $file" -ForegroundColor Green
    } else {
        Write-Host "Not found: $file" -ForegroundColor Gray
    }
}

Write-Host "`nCleanup complete!" -ForegroundColor Cyan