namespace GameZone.Attributes
{
    public class AllowedExtentionsAttribute : ValidationAttribute
    {
        private readonly string _allowedExtensions;
        public AllowedExtentionsAttribute(string allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                var isAllowed = _allowedExtensions.Split(',').Contains(extension, StringComparer.OrdinalIgnoreCase);
                if(!isAllowed)
                {
                    return new ValidationResult($"This file extension is not allowed. Allowed extensions: {_allowedExtensions}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
