using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Helper
{
    public class AuthApiResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }
        public string Token { get; set; } 

    }
}
