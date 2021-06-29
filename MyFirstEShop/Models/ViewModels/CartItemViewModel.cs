using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string TeacherName { get; set; }
        public string ProductCoverAddress { get; set; }
        public int Price { get; set; }
        public int DiscountPrice { get; set; }
        public double FinalPrice
        {
            get
            {
                double Discount = DiscountPrice == 0 ? 0.0 : DiscountPrice / 100.0;

                return Price * (1 - Discount);
            }
        }

    }
}
