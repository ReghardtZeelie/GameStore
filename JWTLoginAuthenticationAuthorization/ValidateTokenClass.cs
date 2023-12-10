using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        public JwtSecurityToken ValidateTWTToken(string token,string UserName,ref string Log,ref UsersModel user)
        {
            JwtSecurityToken jwtToken;
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
                    jwtToken = (JwtSecurityToken)validatedToken;
                   
                  var TokenUserName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                    var TokenUserID = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    var TokenCartID = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber).Value;
                    if (TokenUserID == null)
                    {
                        
                            Log = "Token Authentication failed.";
                        return null;
                    }
                    else
                    {
                        user.ID = Convert.ToInt32(TokenUserID);
                    }
                    if (TokenUserName != null)
                    {
                        if (UserName != TokenUserName)
                        {
                            Log = "Token Authentication failed.";
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                  user.CartID = Convert.ToInt32(TokenCartID);

                    return jwtToken;
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
