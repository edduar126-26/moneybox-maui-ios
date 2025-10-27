using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box
{
    public class ResetPasswordLinkMessage
    {

        public string Token { get; set; }
        public int Uid { get; set; }

        public ResetPasswordLinkMessage(string token, int uid)
        {
            Token = token;
            Uid = uid;
        }
    }

    public sealed class ResetPasswordInvalidLinkMessage
    {
        public string Reason { get; }
        public ResetPasswordInvalidLinkMessage(string reason) => Reason = reason;
    }
}
