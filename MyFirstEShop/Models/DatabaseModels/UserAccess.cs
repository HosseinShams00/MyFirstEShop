using System.Collections.Generic;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class UserAccess
    {
        public int Id { get; set; }

        public Access AdminAccess { get; set; }

        #region Navigation Property

        public ICollection<UserUserAccess> UserUserAccesses { get; set; }

        #endregion

    }


    public enum Access
    {
        CanEditUserInfo,

        CanEditProductDetail,

        CanEditProductStatus,

        CanSeeTicket,

        CanSetAccessForUser,

        CanSetAdmin,

        CanSetTeacher,
    }
}
