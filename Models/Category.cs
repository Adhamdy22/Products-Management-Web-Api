
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }


        public string? CreatedBy { get; set; } // Store UserId here

        
    }
}
