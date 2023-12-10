using DAL;
using GameStore;
using Microsoft.AspNetCore.Mvc;
using Models;
using Serilog;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    [Route("GameStore/[controller]")]
    [ApiController]

   
    public class CartController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ItemController> _logger;
        CartDAL cartDAL;
        HelperClass helperClass;
        ValidateTokenClass ValidateToken;
        ModelMapperClass modelMapperClass;
        UsersModel usersModel = new UsersModel();
        
        public CartController(IConfiguration config, ILogger<ItemController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddItemsToCart")]
        public ActionResult AddItemsToCart(CartModel cart, string UserName, string token)
        {
            ModelState.Clear();
            
            cartDAL = new CartDAL(_config);
            ValidateToken = new ValidateTokenClass(_config);

            string log = string.Empty;

            var ValidatedToken = ValidateToken.ValidateTWTToken(token, UserName, ref log,ref usersModel);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }
            if (!validateItemAdd(cart, ref log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }
            cartDAL.AddItemToCart(cart, usersModel.ID, ref log);

            _logger.LogInformation(log);
            return Ok(log);
        }

        [HttpGet]
        [Route("ViewCart")]
        public ActionResult ViewCart(string token, string Username)
        {
            ModelState.Clear();
            string log = string.Empty;
            cartDAL = new CartDAL(_config);
            List<ViewCartModel> ItemList = new List<ViewCartModel>();


            ValidateToken = new ValidateTokenClass(_config);
            var ValidatedToken = ValidateToken.ValidateTWTToken(token, Username, ref log, ref usersModel);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }

            ItemList = cartDAL.QCart(usersModel, ref log);

            if ((ItemList == null || ItemList.Count() == 0) && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return BadRequest(ModelState);
            }

            return Ok(ItemList);
            return Ok();
        }

        [HttpDelete]
        [Route("RemoveItemsfromCart")]
        public ActionResult RemoveItemsfromCart(CartModel cart, string UserName, string token)
        {
            ModelState.Clear();

            cartDAL = new CartDAL(_config);
            ValidateToken = new ValidateTokenClass(_config);

            string log = string.Empty;

            var ValidatedToken = ValidateToken.ValidateTWTToken(token, UserName, ref log, ref usersModel);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }
            if (!validateItemRemove(cart, ref log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }

            cartDAL.RemoveItemFromCart(cart, usersModel, ref log);


            _logger.LogInformation(log);
            return Ok(log);
            
        }

        private bool validateItemRemove(CartModel cart, ref string Log)
        {
            foreach (var item in cart.cartItems)
            {
                if (item.Qty <= 0)
                {
                    Log = "Please enter an valid positive quantity to remove.";
                }
            }
            return true;
        }
        private bool validateItemAdd(CartModel cart, ref string Log)
        {
            foreach (var item in cart.cartItems)
            {
                if (item.Qty <= 0)
                {
                    Log = "Please enter an valid positive quantity to add.";
                }
            }
            return true;
        }
    }
}
