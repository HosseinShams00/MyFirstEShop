using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Models.CustomException
{
    public class UserDosNotExist : Exception
    {
        public UserDosNotExist() : base("شخصی با مشخصات فوق پیدا نشد")
        {

        }
    }
    public class DeadlineEnded : Exception
    {
        public DeadlineEnded() : base("زمان شما به پایان رسیده لطفا دوباره اقدام نماید")
        { }
    }
    public class TokenIsNotValid : Exception
    {
        public TokenIsNotValid() : base("توکن نا معتبر است")
        {

        }
    }
}
