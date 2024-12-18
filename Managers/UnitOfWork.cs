using Managers.Categories;
using Managers.Items;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class UnitOfWork : IDisposable
    {
        private readonly AppDbContext appDb;
        public ItemsManager Items;
        public CategoryManager Categories;

        public UnitOfWork(AppDbContext context)
        {
            appDb = context;
            Categories = new CategoryManager(appDb);
            Items = new ItemsManager(appDb);
        }
        public int CommitChanges()
        {
            return appDb.SaveChanges();
        }

        public void Dispose()
        {
            appDb.Dispose();
        }
    }
}
