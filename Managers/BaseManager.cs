
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace Managers
{
    public class BaseManager<T> where T : class
    {
        

        protected AppDbContext context;
        private DbSet<T> DbSet;

        public BaseManager(AppDbContext context)
        {
            this.context = context;
            DbSet= context.Set<T>();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }
        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsQueryable();
        }

        public IEnumerable<T> GetAll(params string[] eager)
        {
            IQueryable<T> query= context.Set<T>().AsQueryable();

            if(eager.Length > 0)
            {
                foreach(var eger in eager)
                {
                    query=query.Include(eger);
                }
            }
            return query.ToList();

        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] eager)   
        {
            IQueryable<T> query = context.Set<T>().AsQueryable();

            if (eager.Length > 0)
            {
                foreach (var eger in eager)
                {
                    query = query.Include(eger);
                }
            }
            return await query.ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }


        //public T Search(Expression<Func<T, bool>> search) {
        //    return this.DbSet.SingleOrDefault(search);
        //}
        public IQueryable<T> Search(Expression<Func<T, bool>> search)
        {
            IQueryable<T> query = context.Set<T>().AsQueryable();
            if (search != null)
            {
                query = query.Where(search);
            }
            return query;
        }

        public IQueryable<T> Sort(IQueryable<T> query, string columnName = "Id", bool isAscending = true)
        {
            if (!string.IsNullOrEmpty(columnName))
            {
                query = query.OrderBy(columnName, isAscending);  // Apply dynamic sorting
            }
            return query;
        }


        public bool Add(T Data)
        {
            try
            {
                context.Set<T>().Add(Data);
                this.context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            
        }

        public bool Update(T Data)
        {
            try
            {
                context.Set<T>().Update(Data);
                this.context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool Delete(T Data)
        {
            try
            {
                context.Set<T>().Remove(Data);
                this.context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        public void AddList(IEnumerable<T> myList)
        {
            context.Set<T>().AddRange(myList);
            context.SaveChanges();
        }

        public void UpdateList(IEnumerable<T> myList)
        {
            context.Set<T>().UpdateRange(myList);
            context.SaveChanges();
        }

        public void DeleteList(IEnumerable<T> myList)
        {
            context.Set<T>().RemoveRange(myList);
            context.SaveChanges();
        }


        


    }
}
