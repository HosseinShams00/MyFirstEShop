
using Microsoft.EntityFrameworkCore;
using MyFirstEShop.Models.DatabaseModels;

namespace MyFirstEShop.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        #region DbSets

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOtherInfo> ProductOtherInfos { get; set; }


        public DbSet<User> User { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<Teacher> Teacher { get; set; }

        public DbSet<Cart> Cart { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Relations

            modelBuilder.Entity<Category>()
                .HasMany(Q => Q.Products)
                .WithMany(Q => Q.Categories);

            modelBuilder.Entity<Product>()
                .HasOne(Q => Q.ProductOtherInfo)
                .WithOne(Q => Q.Product)
                .HasForeignKey<ProductOtherInfo>(Q => Q.ProdutId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .HasOne(Q => Q.Teacher)
                .WithMany(Q => Q.Products)
                .HasForeignKey(Q => Q.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductOtherInfo>()
                .HasOne(Q => Q.Product)
                .WithOne(Q => Q.ProductOtherInfo)
                .HasForeignKey<ProductOtherInfo>(Q => Q.ProdutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(Q => Q.Teacher)
                .WithOne(Q => Q.Info)
                .HasForeignKey<Teacher>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                .HasOne(Q => Q.UserSetting)
                .WithOne(Q => Q.UserInfo)
                .HasForeignKey<UserSetting>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(Q => Q.Carts)
                .WithOne(Q => Q.User)
                .HasForeignKey(Q => Q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserSetting>()
                .HasOne(Q => Q.UserInfo)
                .WithOne(Q => Q.UserSetting)
                .HasForeignKey<UserSetting>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Teacher>()
                .HasOne(Q => Q.Info)
                .WithOne(Q => Q.Teacher)
                .HasForeignKey<Teacher>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Teacher>()
                .HasMany(Q => Q.Products)
                .WithOne(Q => Q.Teacher)
                .HasForeignKey(Q => Q.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Cart>()
                .HasOne(Q => Q.User)
                .WithMany(Q => Q.Carts)
                .HasForeignKey(Q => Q.UserId)
                .OnDelete(DeleteBehavior.Cascade);



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


            });

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
