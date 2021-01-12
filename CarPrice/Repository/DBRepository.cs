using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPrice.Repository
{
    internal sealed class DBRepository<T> : IRepository<T> where T : class
    {
        private readonly CarContext context;
        private readonly DbSet<T> dbSet; 

        public DBRepository()
        {
            context = new();
            dbSet = context.Set<T>();
        }

        public void Add(T element) => dbSet.Add(element);

        public IEnumerable<T> GetAll() => dbSet;

        #region Dispose

        public void Dispose()
        {
            context?.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
