using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class UserSecurity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(150)]
        public string? EmailToken { get; set; }
        [MaxLength(150)]
        public string? PasswordToken { get; set; }
        public DateTime? Deadline { get; set; }


        #region Navigation

        public User User { get; set; }  

        #endregion

    }
}
