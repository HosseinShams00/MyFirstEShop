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
        UserInfo GetUserByUserName(string UserName);
        UserInfo GetUserByEmail(string Email);
        bool ExistUser(string Email);
        void AddUser(UserInfo user);
        void RemoveUser(UserInfo user);
    }

    public class UserRegister : IUserRegisterRepository
    {
        private MyDbContext DbContext { get; set; }
        public UserRegister(MyDbContext context)
        {
            DbContext = context;

        }

        public void AddUser(UserInfo user)
        {
            DbContext.Add(user);
            DbContext.SaveChanges();
        }

        public void RemoveUser(UserInfo user)
        {
            DbContext.Remove(user);
            DbContext.SaveChanges();
        }

        public bool ExistUser(string Email)
        {
            return DbContext.UserInfos.Any(user => user.Email == Email);
        }

        public UserInfo GetUserByEmail(string Email)
        {
            return DbContext.UserInfos.SingleOrDefault(i => i.Email == Email.ToLower());
        }

        public UserInfo GetUserByUserName(string UserName)
        {
            return DbContext.UserInfos.SingleOrDefault(i => i.UserName == UserName.ToLower());
        }
    }
}
