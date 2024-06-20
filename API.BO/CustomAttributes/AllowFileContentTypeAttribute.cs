using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.CustomAttributes
{
    public class AllowFileContentTypeAttribute : ValidationAttribute
    {
        private readonly string[] _allowedType;

        public AllowFileContentTypeAttribute(string[] allowedType)
        {
            _allowedType = allowedType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }
            if (value is IFormFile)
            {
                var formFile = (IFormFile)value;
                //var fileName = formFile.FileName;
                var contentType = formFile.ContentType;
                ////var fileExtension = FileHelper.ExtractFileExtention(fileName);
                //if (contentType != "image/jpeg" || contentType != "image/png")
                //{
                //	return new ValidationResult("there is not extention from this file, please send appropriate file");
                //}
                var isExist = _allowedType.Contains(contentType);
                if (isExist is true)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult($"{contentType} type is not allowed");
            }
            else
            {
                return new ValidationResult("require type to validate is IFormFile");
            }
        }
    }
}
