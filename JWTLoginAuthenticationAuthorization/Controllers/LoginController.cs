using Microsoft.AspNetCore.Mvc;
using Models;

using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Serilog;
using GameStore;
using DAL;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    [Route("GameStore/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        
        private readonly IConfiguration _config;
        private readonly ILogger<LoginController> _logger;
        GenerateJWTTokenClass generateJWTToken;
        AuthenticateClass authenticateUserLogin;
        HelperClass helperClass;
        ValidateTokenClass ValidateToken;
        UsersModel usersModel = new UsersModel();
        public LoginController(IConfiguration config, ILogger<LoginController> logger)
        {
            _config = config;
            _logger = logger;
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginModel User)
        {
            ModelState.Clear();
            string log = string.Empty;
            if (!validateUserLogin(User,ref log))
            {
                _logger.LogInformation(log);
                return BadRequest(log);

            }
            authenticateUserLogin = new AuthenticateClass(_config);
            generateJWTToken = new GenerateJWTTokenClass(_config);
            var usersModel = authenticateUserLogin.Authenticate(User, ref log);
            if (usersModel == null && !string.IsNullOrEmpty(log)) 
            {
                ModelState.AddModelError("ErrorMessage", log);

                return BadRequest(ModelState);
               
            }
            if (usersModel != null)
            {
                var token = generateJWTToken.GenerateToken(usersModel, ref log);

                if (token == null && !string.IsNullOrEmpty(log))
                {
                    ModelState.AddModelError("ErrorMessage", log);

                    return BadRequest(ModelState);

                }

                _logger.LogInformation("New token generated for user: " + usersModel.Name);

                return Ok(token);
            }

            _logger.LogInformation("Authentication failed for: "+ User.Name + "");
            return Unauthorized("Authentication failed for: " + User.Name + "");
        }
        [HttpPost]
        [Route("Logout")]
        public ActionResult Logout(string UserName, string token)
        {
            UsersDAL usersDAL = new UsersDAL(_config);
            string log = string.Empty;
            ValidateToken = new ValidateTokenClass(_config);
            helperClass = new HelperClass(_config);
            var ValidatedToken = ValidateToken.ValidateTWTToken(token, UserName, ref log, ref usersModel);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }
            var status = usersDAL.LogoutUser(usersModel, ref log);
            if (status == true)
            {
                _logger.LogInformation(log);

                return Ok(log);
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }

        }

        private bool validateUserLogin(LoginModel User, ref string Log)
        {
            if (string.IsNullOrEmpty(User.Name))
            {
                Log = "Please enter an valid user name.";
                return false;
            }
            else
            {
                if (User.Name.Length > 30)
                {
                    Log = "User name can not be more than 30 alpha numeric characters";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(User.Password))
            {
                Log = "Please enter an valid password.";
                return false;
            }
            else
            {
                if (User.Password.Length > 100)
                {
                    Log = "Password can not be more than 100 alpha numeric characters";
                    return false;
                }
            }

            return true;
        }
       
    }
}
