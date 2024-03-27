using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Shop_MVC.Attributes
{
    public class NoSwedishAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string viewModelNoSwedish = Convert.ToString(value)?.ToLower();

            if (viewModelNoSwedish != null && (viewModelNoSwedish.Contains("å") || viewModelNoSwedish.Contains("ä") ||
                                               viewModelNoSwedish.Contains("ö")))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;

        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (!context.Attributes.ContainsKey("data-val"))
                context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-validNoSwedish", ErrorMessage);
        }
    }
}
