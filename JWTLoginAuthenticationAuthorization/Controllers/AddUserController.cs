using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AddUserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AddUserController> _logger;
        UsersDAL usersDAL;
        public AddUserController(IConfiguration config, ILogger<AddUserController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddUser([FromBody] AddUserModel NewUser)
        {
            ModelState.Clear();
            string log = string.Empty;
            if (validateNewUser(NewUser, ref log))
            {
                 usersDAL = new UsersDAL(_config);

                var user = usersDAL.IUser(NewUser, ref log);
                if (!string.IsNullOrEmpty(log))
                {
                    _logger.LogInformation(log);
                    user = null;
                    var model = new
                    {
                        message = new
                        {
                            code = 500,
                            message = log
                        }
                    };
                    ModelState.AddModelError("ErrorMessage", model.message.message);

                    return BadRequest(ModelState);


                }
                if (user != null)
                {
                    if (user.Name == "User name already exist")
                    {
                        _logger.LogInformation("User with name: " + NewUser.Name + " already exist in the database.");
                        var model = new
                        {
                            message = new
                            {
                                code = 412,
                                message = "User with name: " + NewUser.Name + " already exist in the database."
                            }
                        };
                        ModelState.AddModelError("ErrorMessage", model.message.message);

                        return BadRequest(ModelState);
                    }
                    else
                    {

                        _logger.LogInformation("User: " + user.Name + " successfully Created. Please login.");
                        var model = new
                        {
                            message = new
                            {
                                code = 200,
                                message = "User: " + user.Name + " successfully Created. Please login."
                            }
                        };


                        return Ok("User: " + user.Name + " successfully Created. Please login.");
                    }
                }
            }
            ModelState.AddModelError("Validation Message", log);

            return BadRequest(ModelState);

        }
        private bool validateNewUser(AddUserModel NewUser, ref string Log)
        {
            if (string.IsNullOrEmpty(NewUser.Name))
            {
                Log = "Please enter an valid new user name.";
                return false;
            }
            else
            {
                if (NewUser.Name.Length > 30)
                {
                    Log = "User name can not be more than 30 alpha numeric characters";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(NewUser.Password))
            {
                Log = "Please enter an valid new password.";
                return false;
            }
            else
            {
                if (NewUser.Password.Length > 100)
                {
                    Log = "New password can not be more than 100 alpha numeric characters";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(NewUser.ConfirmPassword))
            {
                Log = "Please confirm the new password.";
                return false;
            }
            else
            {
                if (NewUser.ConfirmPassword.Length > 100)
                {
                    Log = "Confirm new password can not be more than 100 alpha numeric characters";
                    return false;
                }
            }
            if (NewUser.ConfirmPassword.Trim() != NewUser.Password)
            {
                Log = "Passwords do not match.";
                return false;
            }
            if (string.IsNullOrEmpty(NewUser.Age.ToString()))
            {
                Log = "Please enter a valid age.";
                return false;
            }
            else 
            {
                try
                {
                    DateTime test = DateTime.Parse(NewUser.Age.ToString());
                }
                catch
                {
                    Log = "Please enter a valid age.";
                    return false;
                }
            }

            if (DateTime.Now.Year - NewUser.Age.Year < 18) 
            {
                Log = "Users must be 18 yars or older.";
                return false;
            }
            return true;
        }
    }
}
