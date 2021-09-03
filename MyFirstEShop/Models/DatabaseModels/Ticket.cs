using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.DatabaseModels
{
    public class Ticket
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Subject { get; set; }

        public string UserQuestion { get; set; }

        public string AdminAnswer { get; set; }

        public DateTime UserSendQuestionDateTime { get; set; }

        public DateTime AdminAnswerDateTime { get; set; }

        #region Navigation Property

        public User User { get; set; }

        #endregion
    }
}
