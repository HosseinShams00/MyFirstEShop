
namespace MyFirstEShop.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string About { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsTeacher { get; set; }

        public System.DateTime? RegisterTime { get; set; }

        #region Navigation Property

        public TeacherInfo Teacher { get; set; }
        public UserSetting UserSetting { get; set; }

        #endregion
    }
}
