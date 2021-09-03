using System.Collections.Generic;

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

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<UserUserAccess> UserUserAccesses { get; set; }

        public DbSet<UserSecurity> UserSecurities { get; set; }

        public DbSet<UserAccess> UserAccesses { get; set; }

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
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(Q => Q.Tickets)
                .WithOne(Q => Q.User)
                .HasForeignKey(Q => Q.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                .HasOne(Q => Q.UserSetting)
                .WithOne(Q => Q.UserInfo)
                .HasForeignKey<UserSetting>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(Q => Q.UserSecurity)
                .WithOne(Q => Q.User)
                .HasForeignKey<UserSecurity>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(Q => Q.Carts)
                .WithOne(Q => Q.User)
                .HasForeignKey(Q => Q.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<UserSetting>()
                .HasOne(Q => Q.UserInfo)
                .WithOne(Q => Q.UserSetting)
                .HasForeignKey<UserSetting>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSecurity>()
                .HasOne(Q => Q.User)
                .WithOne(Q => Q.UserSecurity)
                .HasForeignKey<UserSecurity>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Teacher>()
                .HasOne(Q => Q.Info)
                .WithOne(Q => Q.Teacher)
                .HasForeignKey<Teacher>(Q => Q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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


            modelBuilder.Entity<UserAccess>()
                .HasData(new List<UserAccess>()
                {
                    new UserAccess(){Id = 1 , AdminAccess = Access.CanSetAdmin},
                    new UserAccess(){Id = 2 , AdminAccess = Access.CanSetTeacher},
                    new UserAccess(){Id = 3 , AdminAccess = Access.CanEditUserInfo},
                    new UserAccess(){Id = 4 , AdminAccess = Access.CanEditProductDetail},
                    new UserAccess(){Id = 5 , AdminAccess = Access.CanSeeTicket},
                    new UserAccess(){Id = 6 , AdminAccess = Access.CanSetAccessForUser},
                    new UserAccess(){Id = 7 , AdminAccess = Access.CanEditProductStatus}

                });

            modelBuilder.Entity<User>()
                .HasData(new User()
                {
                    Id = 1,
                    FirstName = "Hossein",
                    LastName = "Shams Pouya",
                    Email = "admin@gmail.com",
                    /// Password is 1234
                    Password = "$s2$16384$8$1$WMvLICQbkAUri7Lf0nMcZGW5ScrSK8TkNUUpz4TARuQ=$hRqwnMGt8qtp6nTnvih2wyfmCa5CBCNCJDDrM2daUpE=",
                    IsTeacher = true,
                    IsAdmin = true,
                    VerifyEmail = true,
                    RegisterTime = System.DateTime.Now,
                });

            modelBuilder.Entity<UserUserAccess>()
                .HasData(new List<UserUserAccess>()
                {
                    new UserUserAccess(){ Id = 1 , UserId = 1 , UserAccessId = 1},
                    new UserUserAccess(){ Id = 2 , UserId = 1 , UserAccessId = 2},
                    new UserUserAccess(){ Id = 3 , UserId = 1 , UserAccessId = 3},
                    new UserUserAccess(){ Id = 4 , UserId = 1 , UserAccessId = 4},
                    new UserUserAccess(){ Id = 5 , UserId = 1 , UserAccessId = 5},
                    new UserUserAccess(){ Id = 6 , UserId = 1 , UserAccessId = 6},
                    new UserUserAccess(){ Id = 7 , UserId = 1 , UserAccessId = 7},
                });

            modelBuilder.Entity<Teacher>()
                .HasData(new Teacher()
                {
                    Id = 1,
                    UserId = 1,
                });



            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }
}
