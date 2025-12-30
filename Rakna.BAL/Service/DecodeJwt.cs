using Rakna.BAL.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.Service
{
    public class DecodeJwt : IDecodeJwt
    {
        public string? GetUserIdFromToken(string? token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();


            if (tokenHandler.CanReadToken(token))
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["uid"]?.ToString();

                return userId;
            }
            else
            {
                return null;
            }
        }
    }
}
