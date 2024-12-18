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
    public class ItemViewModel  
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        //public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? CategoryId { get; set; }

        public IFormFile? ImagePath { get; set; }
        //public Category? Category { get; set; }
    }
}
