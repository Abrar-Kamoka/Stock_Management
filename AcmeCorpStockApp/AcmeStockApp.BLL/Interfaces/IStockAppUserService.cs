using AcmeStockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.BLL.Interfaces
{
    public interface IStockAppUserService
    {
        public StockAppUser CheckByEmail(string email);

        public StockAppUser CheckByPassword(string password);

        public StockAppUser CheckUserByEmailandPassword(string email, string password);

        public IEnumerable<StockAppUser> GetAll();

        public StockAppUser Add(StockAppUser regUser);

        public void Save();

        public StockAppUser Update(StockAppUser User);

        public void SendEmail(string email, string subject, string messageBody);

        //public StockAppUser Save();
    }
}
