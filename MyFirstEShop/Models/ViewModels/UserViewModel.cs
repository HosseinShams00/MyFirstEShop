using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

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
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل شما درست نوشته نشده است لطفا مجدد بررسی نماید")]
        public string EmailAddress { get; set; }

        [Display(Name = "شماره تلفن")]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [Display(Name = "درباره ی")]
        [MaxLength(100)]
        public string About { get; set; }

        [Display(Name = "زمان ثبت نام")]
        public DateTime? RegisterTime { get; set; }


    }
}
