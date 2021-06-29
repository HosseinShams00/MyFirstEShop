using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace MyFirstEShop.Repositories
{
    public interface IUserRepository
    {
        User GetUser(int userId);
        User GetUser(string email);
        UserViewModel GetUserView(int userId);
        bool ExistUser(string email);
        void AddUser(User user);
        void AddUser(RegisterAccountModelView user);
        void RemoveUser(User user);
        void ChangeInfo(UserViewModel user);

        
    }

    public class UserRepository : IUserRepository
    {
        private MyDbContext DbContext { get; set; }
        public UserRepository(MyDbContext context)
        {
            DbContext = context;

        }

        public User GetUser(int userId)
        {
            return DbContext.User.SingleOrDefault(i => i.Id == userId);
        }
        public User GetUser(string Email)
        {
            return DbContext.User.SingleOrDefault(i => i.Email == Email.ToLower());
        }

        public bool ExistUser(string Email)
        {
            return DbContext.User.Any(user => user.Email == Email);
        }  

        public void AddUser(User user)
        {
            DbContext.Add(user);
            DbContext.SaveChanges();
        }

        public void AddUser(RegisterAccountModelView registerAccount)
        {
            AddUser(new User()
            {
                FirstName = registerAccount.FirstName,
                LastName = registerAccount.LastName,
                Email = registerAccount.EmailAddress,
                Password = registerAccount.Password,
                IsAdmin = false,
                RegisterTime = DateTime.Now
            });
            DbContext.SaveChanges();
        }

        public void RemoveUser(User user)
        {
            DbContext.Remove(user);
            DbContext.SaveChanges();
        }


        public void ChangeInfo(UserViewModel user)
        {
            var us = DbContext.User.Single(i => i.Id == user.Id);
            us.FirstName = user.FirstName;
            us.LastName = user.LastName;
            us.About = user.About;
            us.Address = user.Address;

            DbContext.SaveChanges();
        }

        public UserViewModel GetUserView(int userId)
        {

            var selectUser = GetUser(userId);

            return new UserViewModel()
            {
                FirstName = selectUser.FirstName,
                LastName = selectUser.LastName,
                EmailAddress = selectUser.Email,
                PhoneNumber = selectUser.PhoneNumber,
                About = selectUser.About,
                Address = selectUser.Address,
                RegisterTime = selectUser.RegisterTime
            };
        }
    }
}
