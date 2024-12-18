using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CustomValidation
{
    public class UniquePhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            var entity = _context.Users.SingleOrDefault(e => e.PhoneNumber == value.ToString());

            if (entity != null)
            {
                return new ValidationResult("Phone number already in use.");
            }
            return ValidationResult.Success;
        }
    }
}
