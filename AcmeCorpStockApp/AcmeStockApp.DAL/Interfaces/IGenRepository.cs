using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.DAL.Interfaces
{
    public interface IGenRepository<T> where T : class
    {
        //T - is any Generic class
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> predicate);
        void Adding(T entity);
        void Remove (T entity);
        void RemoveRange(IEnumerable<T> entity);
       
    }
}
