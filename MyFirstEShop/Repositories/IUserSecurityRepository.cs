using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Data;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.CustomException;

namespace MyFirstEShop.Repositories
{
    public interface IUserSecurityRepository
    {
        bool VerifyUserStamp(int userId, Guid stamp);
        string SetPasswordRecovery(string email, string Token);
        bool CheckPasswordRecoveryToken(string email, string Token);
        void ChangePassword(string email, string password);
        string SetEmailVerifyToken(string email);
        bool VerifyEmail(string email, string token);
    }

    public class UserSecurityRepository : IUserSecurityRepository
    {
        private readonly IUserRepository userRepository;
        private readonly MyDbContext DbContext;
        private readonly IHasher hasher;
        public UserSecurityRepository(MyDbContext _dbContext ,IUserRepository _userRepository,IHasher _hasher)
        {
            userRepository = _userRepository;
            DbContext = _dbContext;
            hasher = _hasher;
        }
        public bool VerifyUserStamp(int userId, Guid stamp)
        {
            var user = userRepository.GetUser(userId);
            return user.SecurityStamp == stamp ? true : false;
        }

        public string SetPasswordRecovery(string email, string Token)
        {
            var user = userRepository.GetUser(email);
            var userSecurity = DbContext.UserSecurities.SingleOrDefault(i => i.UserId == user.Id);

            if (user != null)
            {
                if (userSecurity == null)
                {
                    var security = new UserSecurity()
                    {
                        UserId = user.Id,
                        PasswordToken = Token,
                        Deadline = DateTime.Now.AddHours(1),
                    };

                    DbContext.UserSecurities.Add(security);
                    DbContext.SaveChanges();
                    userSecurity = security;
                }
                else
                {
                    if (userSecurity.PasswordToken == null)
                    {
                        userSecurity.PasswordToken = Token;
                        userSecurity.Deadline = DateTime.Now.AddHours(1);
                        DbContext.SaveChanges();
                    }
                    else
                    {
                        int checkTime = DateTime.Compare(userSecurity.Deadline.Value, DateTime.Now);
                        if (checkTime < 0)
                        {
                            userSecurity.PasswordToken = Token;
                            userSecurity.Deadline = DateTime.Now.AddHours(1);
                            DbContext.SaveChanges();
                        }
                    }
                }

                return userSecurity.PasswordToken;
            }
            else
                return null;

        }

        public bool CheckPasswordRecoveryToken(string email, string Token)
        {
            var user = userRepository.GetUser(email);
            if (user != null)
            {
                var userSecurity = DbContext.UserSecurities.SingleOrDefault(i => i.UserId == user.Id);
                if (userSecurity != null)
                {
                    if (userSecurity.PasswordToken != null)
                    {
                        int checkTime = DateTime.Compare(userSecurity.Deadline.Value, DateTime.Now);
                        if (checkTime < 0)
                        {
                            throw new Models.CustomException.DeadlineEnded();
                        }
                        else if (userSecurity.PasswordToken == Token)
                        {
                            return true;
                        }
                        else
                        {
                            throw new Models.CustomException.TokenIsNotValid();
                        }
                    }
                    else
                    {
                        throw new Models.CustomException.TokenIsNotValid();
                    }

                }
                else
                {
                    throw new Models.CustomException.TokenIsNotValid();
                }
            }
            else
            {
                throw new Models.CustomException.UserDosNotExist();
            }
        }

        public void ChangePassword(string email, string password)
        {
            var user = userRepository.GetUser(email);
            if (user != null)
            {
                var userSecurity = DbContext.UserSecurities.SingleOrDefault(i => i.UserId == user.Id);

                user.Password = hasher.HashGenerate(password);
                user.SecurityStamp = Guid.NewGuid();

                if (userSecurity.EmailToken == null)
                    DbContext.UserSecurities.Remove(userSecurity);
                else
                {
                    userSecurity.PasswordToken = null;
                    userSecurity.Deadline = null;
                }

                DbContext.SaveChanges();
            }
        }

        public string SetEmailVerifyToken(string email)
        {
            var user = userRepository.GetUser(email);

            if(user != null)
            {
                var userSecurity = DbContext.UserSecurities.SingleOrDefault(i => i.UserId == user.Id);
                string token = hasher.GenerateToken();

                if (userSecurity == null)
                {
                    DbContext.UserSecurities.Add(new UserSecurity()
                    {
                        UserId = user.Id,
                        EmailToken = token,
                    });
                    DbContext.SaveChanges();
                }
                else
                {
                    token = userSecurity.EmailToken;
                }

                return token;
            }
            else
            {
                throw new UserDosNotExist();
            }

            
        }

        public bool VerifyEmail(string email, string token)
        {
            var user = userRepository.GetUser(email);
            if (user != null)
            {
                if (user.VerifyEmail == false)
                {
                    var userSecurity = DbContext.UserSecurities.SingleOrDefault(i => i.UserId == user.Id);
                    if (userSecurity != null && userSecurity.EmailToken == token)
                    {
                        user.VerifyEmail = true;

                        if (userSecurity.PasswordToken == null)
                        {
                            DbContext.UserSecurities.Remove(userSecurity);
                        }
                        else
                        {
                            userSecurity.EmailToken = null;
                        }

                        DbContext.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;

            }
            else
                return false;

        }
    }
}
