using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class Product
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string ProductCoverAddress { get; set; }
        public int Price { get; set; }
       
        public DateTime Create { get; set; }
        public DateTime? EndSupport { get; set; }
        public DateTime TotalTime { get; set; }
        public bool HaveUpdate { get; set; }
        public int DiscountPercent { get; set; }
        public ProductStatus Status { get; set; }

        #region Navigation
        public ICollection<Category> Categories { get; set; }
        public Teacher Teacher { get; set; }
        public ProductOtherInfo ProductOtherInfo { get; set; }

        #endregion
    }

    public enum ProductStatus
    {
        Active,
        Suspension,
        Inactive
    }

}
