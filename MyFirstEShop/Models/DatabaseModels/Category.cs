using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string CategoryImageAddress { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
