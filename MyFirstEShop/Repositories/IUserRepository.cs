using System;
using System.Linq;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Data;


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
        bool CheckUserPasssword(string password, int userid);
        void ChangeInfo(UserViewModel user);


    }

    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext DbContext;
        private readonly IHasher hasher;
        public UserRepository(MyDbContext context, IHasher _hasher)
        {
            DbContext = context;
            hasher = _hasher;
        }

        public User GetUser(int userId)
        {
            return DbContext.User.SingleOrDefault(i => i.Id == userId);
        }

        public User GetUser(string Email)
        {
            return DbContext.User.SingleOrDefault(i => i.Email == Email.ToLower());
        }
        public bool CheckUserPasssword(string password, int userid)
        {
            return hasher.VerifyPassword(password, GetUser(userid).Password);
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
                VerifyEmail = false,
                Password = hasher.HashGenerate(registerAccount.Password),
                IsAdmin = false,
                IsTeacher = false,
                RegisterTime = DateTime.Now,
                SecurityStamp = Guid.NewGuid()
            });
           
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
                RegisterTime = selectUser.RegisterTime
            };
        }

    }

}

