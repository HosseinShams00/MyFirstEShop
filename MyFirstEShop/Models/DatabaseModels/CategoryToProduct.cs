using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models
{
    public class CategoryToProduct
    {
        public int CategoryID { get; set; }
        public int ProductID { get; set; }


        #region Navigation Property

        public Category Category { get; set; }
        public Product Product { get; set; }

        #endregion

    }
}
