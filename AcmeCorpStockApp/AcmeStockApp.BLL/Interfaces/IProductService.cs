using AcmeStockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.BLL.Interfaces
{
    public interface IProductService
    {
        public void Add(Product product);

        public Product GetFindProductById(int id);

        public void UpdateProduct(Product existingProduct, Product updatedProduct);

        public void DeleteProduct(Product product);

        public void Save();

    }
}
