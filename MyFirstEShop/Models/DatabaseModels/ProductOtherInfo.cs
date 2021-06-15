using System;
using System.Collections.Generic;
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
        #region Navigation Property
        public Product Product { get; set; }

        #endregion

    }

    public enum CourcesLevel
    {
        Elementary,
        Normal,
        Advanced
    }
}
