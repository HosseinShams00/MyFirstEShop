using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Areas.Admin.Models.ViewModel
{
    public class SearchUserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public Options[] SelectedOptions = {Options.Nothing , Options.Admin , Options.Teacher};
        public int[] SelectedOptionsId { get; set; }

    }

    public enum Options
    {
        Nothing,
        Admin,
        Teacher
    }

}
