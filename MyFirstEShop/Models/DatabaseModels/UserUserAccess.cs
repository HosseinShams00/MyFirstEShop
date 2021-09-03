
namespace MyFirstEShop.Models.DatabaseModels
{
    public class UserUserAccess
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int UserAccessId { get; set; }


        #region Navigation

        public User User { get; set; }

        public UserAccess UserAccess { get; set; }


        #endregion


    }
}
