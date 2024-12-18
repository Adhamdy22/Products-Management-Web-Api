using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Account
{
    public class UserRegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        
        //[Length(11)]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        //[Required]
        //public string ConfirmPassword { get; set; }


    }
}
