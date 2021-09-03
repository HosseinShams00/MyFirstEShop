using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstEShop.Data;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Areas.Admin.Models.ViewModel;

namespace MyFirstEShop.Repositories
{
    public interface IUserAccessRepository
    {
        IEnumerable<Access> GetUserAccess(int userId);
        void SetUserAccess(EditUserInfoViewModel editUserInfoViewModel);

        UserAccess GetAccess(Access access);

    }

    public class UserAccessRepository : IUserAccessRepository
    {
        private readonly IUserRepository userRepository;
        private readonly MyDbContext dbContext;
        private readonly IHasher hasher;
        public UserAccessRepository(MyDbContext _myDbContext, IUserRepository _userRepository, IHasher _hasher)
        {
            dbContext = _myDbContext;
            userRepository = _userRepository;
            hasher = _hasher;
        }

        public UserAccess GetAccess(Access access)
        {
            return dbContext.UserAccesses
            .First(i => i.AdminAccess == access);
        }

        public IEnumerable<Access> GetUserAccess(int userId)
        {
            var userAccess = dbContext.UserUserAccesses
                .Include(q => q.UserAccess)
                .Where(q => q.UserId == userId)
                .Select(i => i.UserAccess.AdminAccess)
                .ToList();


            if (userAccess == null || userAccess.Count == 0)
                return new List<Access>();

            return userAccess.ToList();

        }

        public void SetUserAccess(EditUserInfoViewModel editUserInfoViewModel)
        {
            var user = userRepository.GetUser(editUserInfoViewModel.User.Id);

            if (user == null)
            {
                return;
            }

            if (editUserInfoViewModel.AdminAccess.Contains(Access.CanSetAdmin))
            {
                user.IsAdmin = editUserInfoViewModel.User.IsAdmin;
                user.SecurityStamp = Guid.NewGuid();
                dbContext.SaveChanges();
            }

            if (editUserInfoViewModel.AdminAccess.Contains(Access.CanSetTeacher))
            {
                user.IsTeacher = editUserInfoViewModel.User.IsTeacher;
                user.SecurityStamp = Guid.NewGuid();
                dbContext.SaveChanges();
            }

            if (editUserInfoViewModel.AdminAccess.Contains(Access.CanEditUserInfo))
            {
                if (editUserInfoViewModel.User != null)
                {
                    user.FirstName = editUserInfoViewModel.User.FirstName;
                    user.LastName = editUserInfoViewModel.User.LastName;
                    user.Email = editUserInfoViewModel.User.Email;
                    user.About = editUserInfoViewModel.User.About;
                    user.PhoneNumber = editUserInfoViewModel.User.PhoneNumber;
                    user.VerifyEmail = editUserInfoViewModel.User.VerifyEmail;

                    if (editUserInfoViewModel.User.Password != null)
                    {
                        user.Password = hasher.HashGenerate(editUserInfoViewModel.User.Password);
                    }

                    user.SecurityStamp = Guid.NewGuid();
                    dbContext.SaveChanges();
                }
            }

            if (editUserInfoViewModel.AdminAccess.Contains(Access.CanSetAccessForUser))
            {
                var userAccess = GetUserAccess(user.Id);
                var newAccesss = editUserInfoViewModel.UserAccess.Where(i => userAccess.Any(q => q == i) == false).ToList();
                var removeAccess = userAccess.Where(i => editUserInfoViewModel.UserAccess.Any(q => q == i) == false).ToList();


                foreach(var access in newAccesss)
                {
                    var ac = GetAccess(access);
                    dbContext.UserUserAccesses.Add(new UserUserAccess()
                    {
                        UserId = user.Id,
                        UserAccessId = ac.Id,
                    });
                    
                }

                foreach (var access in removeAccess)
                {
                    var ac = GetAccess(access);
                    var uuAccess = dbContext.UserUserAccesses
                    .FirstOrDefault(i => i.UserId == user.Id && i.UserAccessId == ac.Id);

                    dbContext.UserUserAccesses.Remove(uuAccess);
                }

                dbContext.SaveChanges();

            }
            

        }
    }
}
