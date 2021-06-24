
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


        public DbSet<User> UserInfos { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<Teacher> TeacherInfos { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Relations

            modelBuilder.Entity<Category>()
                .HasMany(i => i.Products)
               .WithMany(q => q.Categories);

            //////////////////////////////////////
            ///
            modelBuilder.Entity<Product>()
                .HasOne(i => i.ProductOtherInfo)
                .WithOne(i => i.Product)
                .HasForeignKey<ProductOtherInfo>(key => key.ProdutId);

            modelBuilder.Entity<Product>()
                .HasOne(i => i.Teacher)
                .WithMany(i => i.Products)
                .HasForeignKey(key => key.TeacherId);

            //////////////////////////////////////

            modelBuilder.Entity<User>()
                .HasOne(i => i.UserSetting)
                .WithOne(i => i.UserInfo)
                .HasForeignKey<UserSetting>(key => key.UserId);

            modelBuilder.Entity<User>()
                .HasOne(i => i.Teacher)
                .WithOne(i => i.Info)
                .HasForeignKey<Teacher>(key => key.UserId);

            #endregion

            #region SeedData

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 5,
                FirstName = "Hossein",
                LastName = "Shams Pouya",
                Email = "admin@gmail.com",
                Password = "1234",
                IsTeacher = true,
                IsAdmin = true,
                RegisterTime = System.DateTime.Now,

                
            }) ;

            modelBuilder.Entity<Teacher>().HasData(new Teacher
            {
                Id = 1,
                UserId = 5,
            });

            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }
}
