using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.CustomAttributes
{
    public class IsDateTimeAfterNow : ValidationAttribute
    {
        private readonly int? _minuteMargin;
        public IsDateTimeAfterNow()
        {

        }

        public IsDateTimeAfterNow(int minuteMargin)
        {
            _minuteMargin = minuteMargin;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }
            if (value is DateTime)
            {
                var parseDateTime = DateTime.Parse(value.ToString());
                var dateTimeToCompare = DateTime.Now;
                if (_minuteMargin is not null)
                    dateTimeToCompare = dateTimeToCompare.AddMinutes(_minuteMargin.Value);
                if (DateTime.Compare(parseDateTime, dateTimeToCompare) <= 0)
                {
                    return new ValidationResult("date time provided is before now");
                }

                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("require type to validate is DateTime");
            }
        }
    }
}
