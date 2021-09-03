using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyFirstEShop.Models.ViewModels
{
    public class TicketViewModel
    {

        public int MessageId { get; set; }
        public int UserId { get; set; }       
        
        [Required(ErrorMessage = "لطفا موضوع را وارد نماید")]
        [Display(Name = "موضوع")]
        [MaxLength(50)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "لطفا متن درخواست خود را وارد نماید")]
        [Display(Name = "متن درخواست")]
        public string UserQuestion { get; set; }

        [Required(ErrorMessage = "لطفا پاسخ درخواست خود را وارد نماید")]
        [Display(Name = "پاسخ درخواست")]
        public string AdminAnswer { get; set; }
        
        [Display(Name = "تاریخ ثبت تیکت")]
        public DateTime UserSendQuestionDateTime { get; set; }

        [Display(Name = "تاریخ پاسخ به تیکت")]
        public DateTime AdminAnswerDateTime { get; set; }

    }
}
