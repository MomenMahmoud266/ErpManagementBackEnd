namespace ErpManagement.Domain.Customization.Attributes;

public class AllowedLettersAttribute(string allowedExtensions) : ValidationAttribute
{
    private readonly string _allowedExtensions = allowedExtensions;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file is not null)
        {
            string specialChar = _allowedExtensions;
            foreach (var item in specialChar)
                if (file.FileName.Contains(item))
                    return new ValidationResult($"File name is '{file.FileName}' contains special characters like [{_allowedExtensions}]");
        }
        return ValidationResult.Success;
    }

}