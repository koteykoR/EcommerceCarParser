using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPrice.Repository
{
    internal interface IRepository<T> : IDisposable where T : class
    {
        void Add(T element);

        IEnumerable<T> GetAll();
    }
}
