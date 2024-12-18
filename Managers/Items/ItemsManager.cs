using LinqKit;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Items
{
    public class ItemsManager : BaseManager<Item>
    {
        private AppDbContext Context;
        public ItemsManager(AppDbContext context) : base(context)
        {
            Context = context;
        }

        public IQueryable<Item> FilteredItems(string searchText, decimal price, int CategoryId = 0)
        {
            var builder = PredicateBuilder.New<Item>(true);

            if (!string.IsNullOrEmpty(searchText))
            {
                builder = builder.And(i => i.Name.Contains(searchText));
            }

            if (price > 0)
            {
                builder = builder.And(i => i.Price <= price);
            }

            if (CategoryId > 0)
            {
                builder = builder.And(i => i.CategoryId == CategoryId);
            }

            // Ensure eager loading of Category
            var query = base.Search(builder).Include(i => i.Category);

            return base.Search(builder);
        }

    }
}
