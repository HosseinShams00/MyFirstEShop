
using System.ComponentModel.DataAnnotations;

namespace MyFirstEShop.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(32)]
        public string FirstName { get; set; }

        [MaxLength(32)]
        public string LastName { get; set; }

        public string Email { get; set; }

        [MaxLength(32)]
        public string Password { get; set; }

        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [MaxLength(100)]
        public string About { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsTeacher { get; set; }

        public System.DateTime? RegisterTime { get; set; }

        #region Navigation Property

        public Teacher Teacher { get; set; }
        public UserSetting UserSetting { get; set; }

        #endregion
    }
}
