using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTLoginAuthenticationAuthorization
{
    public class GenerateJWTTokenClass
    {
        private readonly IConfiguration _config;
        public GenerateJWTTokenClass(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(UsersModel user,ref string Log)
        {
            JwtSecurityToken token;
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.DateOfBirth,user.DOB.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                new Claim(ClaimTypes.SerialNumber,user.CartID.ToString())
            };
                 token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["TokenSettings:Timeout"])),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Log = "An exception has occurred while generating the authentication token. Error: " + ex.Message.ToString()+"";
                    return null;
            }

           

        }
    }
}
