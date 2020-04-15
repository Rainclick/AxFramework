using System;
using System.IdentityModel.Tokens.Jwt;

namespace Services
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }

        public AccessToken(JwtSecurityToken securityToken)
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token_type = "Bearer";
            var datetime = DateTime.Parse("1970-01-01T00:00:00");
            expires_in = (long)(securityToken.ValidTo - datetime.ToUniversalTime()).TotalSeconds;
        }
    }
}