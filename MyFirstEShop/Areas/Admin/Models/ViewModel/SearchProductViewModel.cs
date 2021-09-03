using System.Collections.Generic;
using MyFirstEShop.Models.DatabaseModels;

namespace MyFirstEShop.Areas.Admin.Models.ViewModel
{
    public class SearchProductViewModel
    {
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public List<Category> Categories { get; set; }
        public int SelectedCategoryId { get; set; }   


    }
}
