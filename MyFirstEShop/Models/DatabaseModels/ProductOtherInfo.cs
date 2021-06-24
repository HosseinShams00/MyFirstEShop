using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models
{
    public class ProductOtherInfo
    {
        public int Id { get; set; }

        public int ProdutId { get; set; }
        public int NumberOfPurchases { get; set; }
        public int NumberOfVideos { get; set; }
        public CourcesLevel CourceLevel { get; set; }
        public DateTime TotalTime { get; set; }

        #region Navigation Property
        public Product Product { get; set; }

        #endregion

    }

    public enum CourcesLevel
    {
        [Display(Name = "مبتدی")]
        Elementary,
        [Display(Name = "متوسط")]
        Normal,
        [Display(Name = "پیشرفته")]
        Advanced
    }
}
