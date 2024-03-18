using AcmeStockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.DAL.Interfaces
{
    public interface IStockAppUserRepository : IGenRepository<StockAppUser>
    {
        void Update(StockAppUser user);

        //void Save();

        StockAppUser CheckUserByEmail(string email);

        StockAppUser CheckUserByPassword(string password);

        StockAppUser CheckByEmailandPassword(string email, string password);

        public IEnumerable<StockAppUser> GetAllUsers();

        StockAppUser AddUser(StockAppUser regUser);

        //public StockAppUser SaveToDb();

        public void Save();

    }
}
