using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(32)]
        public string FirstName { get; set; }

        [MaxLength(32)]
        public string LastName { get; set; }

        public string Email { get; set; }
        public bool VerifyEmail { get; set; }

        [MaxLength(110)]
        public string Password { get; set; }

        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [MaxLength(300)]
        public string About { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsTeacher { get; set; }

        public System.DateTime? RegisterTime { get; set; }

        public System.Guid SecurityStamp { get; set; }


        #region Navigation Property

        public Teacher Teacher { get; set; }
        public UserSetting UserSetting { get; set; }
        public UserSecurity UserSecurity { get; set; }  
        public ICollection<Cart> Carts { get; set; }

        #endregion
    }
}
