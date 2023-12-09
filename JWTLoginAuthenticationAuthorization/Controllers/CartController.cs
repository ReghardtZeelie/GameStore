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
        int _UserID;
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

            var ValidatedToken = ValidateToken.ValidateTWTToken(token, UserName, ref log,ref _UserID);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }
            cartDAL.AddItemToCart(cart,  _UserID, ref log);

            //if (!validateNewItem(newItem, ref log))
            //{
            //    _logger.LogInformation(log);
            //    return BadRequest(log);
            //}

            _logger.LogInformation(log);
            return Ok(log);
        }

        [HttpGet]
        [Route("ViewCart")]
        public ActionResult ViewCart(int UserID, string token)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("RemoveItemsfromCart")]
        public ActionResult RemoveItemsfromCart(int UserID,string itemdesc, string token)
        {
            return Ok();
        }

        private bool validateAddItem(NewItemModel newItem, ref string Log)
        {
            if (string.IsNullOrEmpty(newItem.ItemName))
            {
                Log = "Please enter an valid item name.";
                return false;
            }
            else
            {
                if (newItem.ItemName.Length > 30)
                {
                    Log = "Item name can not be more than 30 alpha numeric characters";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(newItem.ItemDescription))
            {
                Log = "Please enter an valid item description.";
                return false;
            }
            else
            {
                if (newItem.ItemDescription.Length > 500)
                {
                    Log = "Item description can not be more than 500 alpha numeric characters";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(newItem.itemCost.ToString()))
            {
                Log = "Please enter item cost.";
                return false;
            }
            else
            {
                if (newItem.itemCost <= 0m)
                {
                    Log = "cost can not be less than zero or zero";
                    return false;
                }
                try
                {
                    decimal test = decimal.Parse(newItem.itemCost.ToString());
                }
                catch
                {
                    Log = "Please enter a valid cost.";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(newItem.ItemWholeSale.ToString()))
            {
                Log = "Please enter item wholesale.";
                return false;
            }
            else
            {
                if (newItem.ItemWholeSale <= 0m)
                {
                    Log = "wholesale can less than zero or zero";
                    return false;
                }

                try
                {
                    decimal test = decimal.Parse(newItem.ItemWholeSale.ToString());
                }
                catch
                {
                    Log = "Please enter a valid wholesale.";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(newItem.ItemRetail.ToString()))
            {
                Log = "Please enter item retail.";
                return false;
            }
            else
            {
                if (newItem.ItemRetail <= 0m)
                {
                    Log = "retail can less than zero or zero";
                    return false;
                }
                try
                {
                    decimal test = decimal.Parse(newItem.ItemRetail.ToString());
                }
                catch
                {
                    Log = "Please enter a valid retail.";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(newItem.Make))
            {
                Log = "Please enter an valid item make.";
                return false;
            }
            else
            {
                if (newItem.Make.Length > 30)
                {
                    Log = "Item make can not be more than 30 alpha numeric characters";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(newItem.Model))
            {
                Log = "Please enter an valid item model.";
                return false;
            }
            else
            {
                if (newItem.Model.Length > 30)
                {
                    Log = "Item model can not be more than 30 alpha numeric characters";
                    return false;
                }
            }
            return true;
        }
    }
}
