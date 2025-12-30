using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Interface
{
    public interface IDecodeJwt
    {
        public string? GetUserIdFromToken(string? token);

    }
}
