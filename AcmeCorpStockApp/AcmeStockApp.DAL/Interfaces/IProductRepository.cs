using AcmeStockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.DAL.Interfaces
{
    public interface IProductRepository : IGenRepository<Product>
    {
        void Update(Product obj);
        void Save();
    }
}
