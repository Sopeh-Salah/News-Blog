using System.ComponentModel.DataAnnotations;

namespace NewsBlogAPI.Helpers
{
    public class ValidatePublicationDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime publicationDate = (DateTime)value;
                DateTime today = DateTime.Today;
                DateTime maxDate = today.AddDays(7);

                if (publicationDate < today || publicationDate > maxDate)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
