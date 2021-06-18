using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Validations
{
    public class FileSizeValidation : ValidationAttribute
    {
        private readonly int maxFileSizeMB;

        public FileSizeValidation(int MaxFileSizeMB)
        {
            maxFileSizeMB = MaxFileSizeMB;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > maxFileSizeMB * 1024 * 1024)
            {
                return new ValidationResult($"File size can't be bigger than: {maxFileSizeMB}mb");
            }

            return ValidationResult.Success;
        }
    }
}
