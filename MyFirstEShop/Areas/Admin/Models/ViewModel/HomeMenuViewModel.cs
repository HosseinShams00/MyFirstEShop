
using System.Collections.Generic;
using MyFirstEShop.Models.DatabaseModels;

namespace MyFirstEShop.Areas.Admin.Models.ViewModel
{
    public class HomeMenuViewModel
    {
        public int TicketsCount { get; set; }
        
        public int SuspendProductsCount { get; set; }

        public IEnumerable<Access> AdminAccess {get;set;}
            
    }
}
