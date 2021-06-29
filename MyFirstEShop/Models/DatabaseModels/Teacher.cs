using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class Teacher
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public int ProductId { get; set; }


        #region Navigation Property

        public ICollection<Product> Products { get; set; }
        public User Info { get; set; }

        #endregion
    }
}
