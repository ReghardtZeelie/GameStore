using Microsoft.AspNetCore.Mvc;
using DAL;
using Models;
using GameStore;
using Serilog;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ItemController> _logger;
        ItemsDAL itemsDAL;
        
        HelperClass helperClass;
        ValidateTokenClass ValidateToken;
        ModelMapperClass modelMapperClass;
        string log = string.Empty;
        public ItemController(IConfiguration config, ILogger<ItemController> logger)
        {
            _config = config;
            _logger = logger;
        }
      
        [HttpPost] 
        [Route("AddNewitem")]
        public ActionResult AddNewitem([FromForm] NewItemModel newItem, string token)
        {
            ModelState.Clear();
            int newItemCode = 0;
            itemsDAL = new ItemsDAL(_config);
            modelMapperClass = new ModelMapperClass();
            ValidateToken = new ValidateTokenClass(_config);
            helperClass = new HelperClass(_config);
                      
           
            var ValidatedToken = ValidateToken.ValidateTWTToken(token, ref log);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }

           
            if (!validateNewItem(newItem, ref log))
            {
                _logger.LogInformation(log);
                return BadRequest(log);
            }

            var ImageArray =   helperClass.imagetoByteArray(newItem.file, ref log);

            if ( ImageArray == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return BadRequest(ModelState);
            }

            newItemCode = itemsDAL.IItem(modelMapperClass.MapNewItemModelToItemsModel(newItem, ImageArray), ref log);

            if (newItemCode == 0 && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return BadRequest(ModelState);
            }
            else
            {
                _logger.LogInformation("New item: "+ newItem .ItemName+ " with item Code: "+ newItemCode.ToString()+ " has been created.");
                return Ok("New item: " + newItem.ItemName + " with item Code: " + newItemCode.ToString() + " has been created.");
            }

        }
        [HttpDelete]
        [Route("DeleteItem")]
        public ActionResult DeleteItem(string token, int ItemCode)
        {
            itemsDAL = new ItemsDAL(_config);
            ValidateToken = new ValidateTokenClass(_config);
            var ValidatedToken = ValidateToken.ValidateTWTToken(token, ref log);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }

            

            if (!itemsDAL.DItem(ItemCode,ref log) && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return BadRequest(ModelState);
            }
            else
            {
               _logger.LogInformation("Item: " + ItemCode.ToString() + " has been deleted.");
            return Ok("Item: " + ItemCode.ToString() + " has been deleted.");
            }

           
        }

            private bool validateNewItem(NewItemModel newItem, ref string Log)
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
