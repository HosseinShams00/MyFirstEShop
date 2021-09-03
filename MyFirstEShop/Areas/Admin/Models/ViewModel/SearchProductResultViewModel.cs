using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Areas.Admin.Models.ViewModel
{
    public class SearchProductResultViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string TeacherName { get; set; }
        public int Price { get; set; }

    }
}
