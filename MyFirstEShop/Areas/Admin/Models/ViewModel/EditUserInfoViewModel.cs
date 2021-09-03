using System.Collections.Generic;
using MyFirstEShop.Models.DatabaseModels;

namespace MyFirstEShop.Areas.Admin.Models.ViewModel
{
    public class EditUserInfoViewModel
    {

        public IEnumerable<Access> UserAccess { get; set; }
        public IEnumerable<Access> AdminAccess { get; set; }
        public User User { get; set; }

    }
}
