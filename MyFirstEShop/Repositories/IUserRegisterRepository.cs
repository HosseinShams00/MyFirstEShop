using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Models;
using MyFirstEShop.Data;

namespace MyFirstEShop.Repository
{
    public interface IUserRegisterRepository
    {
        User GetUserByEmail(string email);
        bool ExistUser(string email);
        void AddUser(User user);
        void RemoveUser(User user);
        void ChangeInfo(User user);
    }

    public class UserRegister : IUserRegisterRepository
    {
        private MyDbContext DbContext { get; set; }
        public UserRegister(MyDbContext context)
        {
            DbContext = context;

        }
        public bool ExistUser(string Email)
        {
            return DbContext.UserInfos.Any(user => user.Email == Email);
        }

        public User GetUserByEmail(string Email)
        {
            return DbContext.UserInfos.SingleOrDefault(i => i.Email == Email.ToLower());
        }

        public void AddUser(User user)
        {
            DbContext.Add(user);
            DbContext.SaveChanges();
        }

        public void RemoveUser(User user)
        {
            DbContext.Remove(user);
            DbContext.SaveChanges();
        }


        public void ChangeInfo(User user)
        {
            var us = DbContext.UserInfos.Single(i => i.Id == user.Id) ;
            us.FirstName = user.FirstName;
            us.LastName = user.LastName;
            us.About = user.About;
            us.Address = user.Address;

            DbContext.SaveChanges();
        }
    }
}
