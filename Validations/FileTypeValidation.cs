using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cinema.Validations
{
    public class FileTypeValidation : ValidationAttribute
    {
        private readonly string[] validTypes;

        public FileTypeValidation(string[] types)
        {
            this.validTypes = types;
        }

        public FileTypeValidation(FileType fileType)
        {
            if (fileType == FileType.Image)
            {
                validTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
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

            if (!validTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult($"The file format should by : {string.Join(", ", validTypes)}");
            }

            return ValidationResult.Success;
        }
    }
}
