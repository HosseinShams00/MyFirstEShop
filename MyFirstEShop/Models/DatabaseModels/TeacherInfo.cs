using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models
{
    public class TeacherInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }


        #region Navigation Property

        public ICollection<Product> Products { get; set; }
        public UserInfo Info { get; set; }

        #endregion
    }
}
