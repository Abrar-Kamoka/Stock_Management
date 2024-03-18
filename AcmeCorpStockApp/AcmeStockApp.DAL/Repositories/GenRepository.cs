using AcmeStockApp.DAL.Interfaces;
using AcmeStockApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.DAL.Repositories
{
    public class GenRepository<T> : IGenRepository<T> where T : class
    {
        private readonly StockDBContext _context;

        internal DbSet<T> Stocks;
        public GenRepository(StockDBContext context)
        {
            _context = context;
            Stocks = _context.Set<T>();
        }

        public void Adding(T entity)
        {
            Stocks.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = Stocks;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = Stocks;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            Stocks.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            Stocks.RemoveRange(entity);
        }
    }
}
