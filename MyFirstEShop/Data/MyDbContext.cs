
using Microsoft.EntityFrameworkCore;
using MyFirstEShop.Models;

namespace MyFirstEShop.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        #region DbSets

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOtherInfo> ProductOtherInfos { get; set; }
        public DbSet<CategoryToProduct> CategoryToProducts { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<TeacherInfo> TeacherInfos { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }


    }
}
