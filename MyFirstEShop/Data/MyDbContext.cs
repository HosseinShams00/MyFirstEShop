
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


        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<TeacherInfo> TeacherInfos { get; set; }


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

            modelBuilder.Entity<UserInfo>()
                .HasOne(i => i.UserSetting)
                .WithOne(i => i.UserInfo)
                .HasForeignKey<UserSetting>(key => key.UserId);

            modelBuilder.Entity<UserInfo>()
                .HasOne(i => i.Teacher)
                .WithOne(i => i.Info)
                .HasForeignKey<TeacherInfo>(key => key.UserId);

            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }
}
