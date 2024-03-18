using AcmeStockApp.BLL.Interfaces;
using AcmeStockApp.DAL.Interfaces;
using AcmeStockApp.DAL.Repositories;
using AcmeStockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _proRepo;

        public ProductService(IProductRepository productRepository) 
        {
            _proRepo = productRepository;
        }

        public void Add(Product product)
        {
            try
            {
                _proRepo.Adding(product);
                _proRepo.Save();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public Product GetFindProductById(int id)
        {
            return _proRepo.Get(p => p.Id == id);
        }

        public void UpdateProduct(Product existingProduct, Product updatedProduct)
        {
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Quantity = updatedProduct.Quantity;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Manufactured = updatedProduct.Manufactured;
            _proRepo.Update(existingProduct);
        }

        public void DeleteProduct(Product product)
        {
            _proRepo.Remove(product);
        }
        
        //public void SaveChanges()
        //{
        //    _proRepo.Save();
        //}

        public void Save()
        {
            _proRepo.Save();
        }
    }
}
