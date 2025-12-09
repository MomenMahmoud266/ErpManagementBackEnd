namespace ErpManagement.Domain.Customization.Attributes;

public class MaxFileLettersCountAttribute(int maxFileSize) : ValidationAttribute
{
    private readonly int _maxFileSize = maxFileSize;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file is not null)
            if (file.FileName.Length > _maxFileSize)
                return new ValidationResult($"Maximum allowed size is {_maxFileSize} char");

        return ValidationResult.Success;
    }
}