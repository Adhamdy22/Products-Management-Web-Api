using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Categories
{
    public class CategoryManager:BaseManager<Category>
    {
        private AppDbContext Context;
        public CategoryManager(AppDbContext context) : base(context)
        {
            Context = context;
        }
    }
}
