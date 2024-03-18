using AcmeStockApp.DAL.Interfaces;
using AcmeStockApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.DAL.Repositories
{
    public class StockAppUserRepository : GenRepository<StockAppUser>, IStockAppUserRepository
    {
        private readonly StockDBContext _context;

        public StockAppUserRepository(StockDBContext context) : base(context)
        {
            _context = context;
        }
        public void Update(StockAppUser user)
        {
            _context.StockAppUsers.Update(user);
        }

        public StockAppUser CheckUserByEmail(string email)
        {
            return _context.StockAppUsers.FirstOrDefault(u => u.Email == email);
        }

        public StockAppUser CheckUserByPassword(string password)
        {
            return _context.StockAppUsers.FirstOrDefault(u => u.Password == password);
        }

        public StockAppUser CheckByEmailandPassword(string email, string password)
        {
            return _context.StockAppUsers.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        //public StockAppUser CheckByUserRememberMe(string email, string password, string rememberme)
        //{
        //    return _context.StockAppUsers.FirstOrDefault(u => u.Email == email && u.Password == password && u.Remember);
        //}

        public IEnumerable<StockAppUser> GetAllUsers()
        {
            return GetAllUsers();
        }

        public StockAppUser AddUser(StockAppUser regUser)
        {
            Adding(regUser); 
            return regUser;
        }

        public void Save()
        {
            _context.SaveChanges();
        }



    }
}
