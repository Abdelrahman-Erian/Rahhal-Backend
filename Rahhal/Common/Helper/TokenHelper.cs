using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rahhal.Common.Views;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rahhal.Common.Helper
{
    public class TokenHelper
    {

        readonly JWTSetting _jwtSetting;
        public TokenHelper(IOptions<JWTSetting> jwtSettings)
        {
            _jwtSetting = jwtSettings.Value;
        }


        public async Task<string> GenerateToken(string userId, string name)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.SecretKey);
            var issuer = _jwtSetting.Issuer;
            var audience = _jwtSetting.Audience;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),

                    new Claim(ClaimTypes.Name, name)


                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
