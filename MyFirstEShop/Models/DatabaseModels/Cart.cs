using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }

        #region Navigation

        public ICollection<Product> Products { get; set; } = new Collection<Product>(); 
        public User User { get; set; }

        #endregion

    }
}
