using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ViewModel.Categories
{
    public class AddCategoryViewModel
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    }
}
