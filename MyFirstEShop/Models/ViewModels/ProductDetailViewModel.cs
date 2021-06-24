using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.ViewModels
{
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string CoverAddress { get; set; }

        public int Price { get; set; }

        public int NumberOfPurchases { get; set; }

        public int NumberOfVideos { get; set; }

        public CourcesLevel CourceLevel { get; set; }
        public ProductStatus Status { get; set; }

        public DateTime StartDate { get; set; }

        public string TeacherName { get; set; }
    }
}
