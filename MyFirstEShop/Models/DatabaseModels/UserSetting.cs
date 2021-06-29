using System.IO;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class UserSetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool ConfirmEmail { get; set; }
        public bool ConfirmPhoneNumber { get; set; }
        
        public bool TowFactorAthurizeEnable { get; set; }

        #region Navigation Property

        public User UserInfo { get; set; }

        #endregion
    }
}
