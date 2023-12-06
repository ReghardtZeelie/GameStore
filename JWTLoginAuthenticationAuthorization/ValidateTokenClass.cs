using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWTLoginAuthenticationAuthorization
{
    public class ValidateTokenClass
    {
        private readonly IConfiguration _config;
        public ValidateTokenClass(IConfiguration config) 
        {
            _config = config;
        }
        public JwtSecurityToken ValidateTWTToken(string token,ref string Log)
        {
            JwtSecurityToken jwt1;
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero,
            };

            try
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                    jwt1 = (JwtSecurityToken)validatedToken;

                    return jwt1;
                }
                catch (Exception ex)
                {
                    Log = ex.Message;
                    return null;
                }
            }
            catch (SecurityTokenValidationException ex)
            {
                Log = ex.Message;
                return null;
            }
        }
    }
}
