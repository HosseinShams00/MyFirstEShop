using System.ComponentModel.DataAnnotations;

namespace MyFirstEShop.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل شما درست نوشته نشده است لطفا مجدد بررسی نماید")]
        public string Email { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }

        public string RedirectUrl { get; set; }
    }
}
