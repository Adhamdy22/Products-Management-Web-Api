using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;



namespace ViewModel.Items
{
    public class AddItemViewModel
    {
        
        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("The Price")]
        //[Range(10, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public decimal Price { get; set; }
        //public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public string? ImagePath { get; set; }

        [DisplayName("Item Image")]
        [Required]
        public IFormFile ItemImage { get; set; }

        //public Category? Category { get; set; }
    }
}
