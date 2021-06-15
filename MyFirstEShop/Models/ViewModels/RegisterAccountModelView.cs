using System.ComponentModel.DataAnnotations;


namespace MyFirstEShop.Models
{
    public class RegisterAccountModelView
    {

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام")]
        [MaxLength(32, ErrorMessage = "کاربر محترم شما نمیتوانید بیشتر از 20 کارکتر نام داشته باشید")]
        [MinLength(4, ErrorMessage = "کاربر محترم شما نمیتوانید کمتر از 4 کارکتر نام داشته باشید")]
        public string FirstName { get; set; }

         

        [Display(Name = "نام خانوادگی")]
        [MaxLength(32, ErrorMessage = "کاربر محترم شما نمیتوانید بیشتر از 20 کارکتر نام داشته باشید")]
        [MinLength(4, ErrorMessage = "کاربر محترم شما نمیتوانید کمتر از 4 کارکتر نام داشته باشید")]
        public string LastName { get; set; }



        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کاربری")]
        [MaxLength(32, ErrorMessage = "کاربر محترم شما نمیتوانید بیشتر از 32 کارکتر نام کاربری داشته باشید")]
        [MinLength(5, ErrorMessage = "کاربر محترم شما نمیتوانید کمتر از 5 کارکتر نام کاربری داشته باشید")]
        public string UserName { get; set; }



        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل شما درست نوشته نشده است لطفا مجدد بررسی نماید")]
        public string EmailAddress { get; set; }



        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز")]
        [MaxLength(32, ErrorMessage = "رمز شما بسیار طولانی است")]
        [MinLength(4, ErrorMessage = "رمز شما بسیار کوتاه است")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور را دوباره وارد کنید")]
        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Compare("Password" , ErrorMessage = ("رمز عبور را درست وارد کنید"))]
        public string RePassword { get; set; }


    }
}
