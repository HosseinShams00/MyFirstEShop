using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Models.DatabaseModels;

namespace MyFirstEShop.Models.ViewModels
{
    public class ProductViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "لطفا یک نام برای این دوره انتخاب نماید")]
        [MaxLength(150)]
        [Display(Name = "نام دوره")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا توضیحاتی را برای این دوره وارد نماید")]
        [Display(Name = "توضیحات دوره")]
        public string Description { get; set; }


        [Required(ErrorMessage = "لطفا عکس دوره را انتخاب نماید")]
        [Display(Name = "عکس دوره")]
        [DataType(DataType.ImageUrl,ErrorMessage = "لطفا فرمت عکس انتخاب کنید")]
        public IFormFile ProductImage { get; set; }

        public string ProductCoverAddress { get; set; }


        [Required(ErrorMessage = "داشتن قیمت اجباری است قیمت 0 به معنی رایگان بودن است")]
        [Display(Name = "قیمت")]
        public int Price { get; set; }
        // public Stream ProductCover { get; set; }

        [Display(Name = "تاریخ تولید")]
        public DateTime Create { get; set; }
        [Display(Name = "تاریخ اتمام دوره")]
        public DateTime? EndSupport { get; set; }

        [Display(Name = "آیا به روز رسانی میشود")]
        public bool HaveUpdate { get; set; }

        [Display(Name = "درصد تخفیف")]
        public int DiscountPercent { get; set; }

        [Display(Name = "سطح دوره")]
        public CourcesLevel CourceLevel { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "گروه ها")]
        public IEnumerable<Category> Categories { get; set; }

    }
}
