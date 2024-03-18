using AcmeStockApp.DAL.Interfaces;
using AcmeStockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.DAL.Repositories
{
    public class ProductRepository : GenRepository<Product>, IProductRepository
    {
        private readonly StockDBContext _context;

        public ProductRepository(StockDBContext context) : base(context) 
        {
            this._context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Product obj)
        {
            _context.Products.Update(obj);
        }
    }
}
