using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Security.JwtConfig
{
    public class AccessTokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public AccessTokenResponse(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }

}
