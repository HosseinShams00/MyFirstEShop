using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
       // public Stream ProductCover { get; set; }
        public DateTime Create { get; set; }
        public DateTime? EndSupport { get; set; }
        public DateTime TotalTime { get; set; }
        public bool HaveUpdate { get; set; }
        public int OffPercent { get; set; }


        #region Navigation
        public ICollection<Category> Categories { get; set; }
        public TeacherInfo Teacher { get; set; }
        public ProductOtherInfo ProductOtherInfo { get; set; }


        #endregion
    }
}
