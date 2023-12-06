using Microsoft.AspNetCore.Mvc;
using DAL;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Models;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ItemController> _logger;
        ItemsDAL ItemsDAL;
        ValidateTokenClass ValidateToken;
        public ItemController(IConfiguration config, ILogger<ItemController> logger)
        {
            _config = config;
            _logger = logger;
        }
      
        [HttpPost] 
        public ActionResult AddNewitem(ItemsModel newItem, string token)
        {
            string log = string.Empty;
             ValidateToken = new ValidateTokenClass(_config);

            var ValidatedToken = ValidateToken.ValidateTWTToken(token, ref log);
            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }
            return Ok();



        }
    }
}
