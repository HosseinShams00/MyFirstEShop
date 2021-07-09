using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد نماید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        public string Token { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد نماید")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد نماید")]
        [Display(Name = "تکرار رمز عبور")]
        [Compare("NewPassword",ErrorMessage = "لطفا پسورد را درست وارد نمایید")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

        public string Error { get; set; }


    }
}
