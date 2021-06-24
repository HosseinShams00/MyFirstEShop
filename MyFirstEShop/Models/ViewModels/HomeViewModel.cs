using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.ViewModels
{
    public class HomeViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CoverAddress { get; set; }
        public int Price { get; set; }
        public string TeacherName { get; set; }

    }
}
