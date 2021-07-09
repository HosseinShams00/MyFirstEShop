using System;
using MyFirstEShop.Data;
using MyFirstEShop.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MyFirstEShop.Repositories
{
    public interface ITeacherRepository
    {
        bool ExistTeacher(int userId);
        Teacher GetTeacher(int userId);
        Teacher GetTeacherWithProducts(int userID);
        bool TeacherHaveThisProduct(int userId, int productId);
    }

    public class TeacherRepository : ITeacherRepository
    {
        private readonly MyDbContext DbContext;
        public TeacherRepository(MyDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public bool ExistTeacher(int userId)
        {
            return DbContext.User.Any(i => i.Id == userId && i.IsTeacher == true);
        }

        public Teacher GetTeacher(int userId)
        {
            return DbContext.Teacher
                .Include(i => i.Info)
                .SingleOrDefault(u => u.UserId == userId);
        }

        public bool TeacherHaveThisProduct(int userId ,int productId)
        {
            return DbContext.Teacher.Include(i => i.Products).Any(i => i.UserId == userId && i.Products.Any(i => i.Id == productId));
        }

        public Teacher GetTeacherWithProducts(int userID)
        {
            return DbContext.Teacher
            .Include(i => i.Info)
            .Include(i => i.Products)
            .ThenInclude(i => i.ProductOtherInfo)
            .FirstOrDefault(u => u.Info.Id == userID);
        }
    }
}
