﻿using Microsoft.AspNetCore.Mvc;
using Models;
using GameStore;
using Serilog;
using System.Collections.Generic;
using System.Net;
using Azure.Core;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using DAL;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    [Route("GameStore/[controller]")]
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
        UsersModel usersModel = new UsersModel();
       
        public ItemController(IConfiguration config, ILogger<ItemController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddNewitem")]
        public ActionResult AddNewitem([FromForm] NewItemModel newItem, string UserName, string token)
        {
            ModelState.Clear();
            int newItemCode = 0;
            itemsDAL = new ItemsDAL(_config);
            modelMapperClass = new ModelMapperClass();
            ValidateToken = new ValidateTokenClass(_config);
            helperClass = new HelperClass(_config);


            var ValidatedToken = ValidateToken.ValidateTWTToken(token,UserName, ref log, ref usersModel);

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

            var ImageArray = helperClass.imagetoByteArray(newItem.file, ref log);

            if (ImageArray == null && !string.IsNullOrEmpty(log))
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
                _logger.LogInformation("New item: " + newItem.ItemName + " with item Code: " + newItemCode.ToString() + " has been created.");
                return Ok("New item: " + newItem.ItemName + " with item Code: " + newItemCode.ToString() + " has been created.");
            }

        }
        [HttpDelete]
        [Route("DeleteItem")]
        public ActionResult DeleteItem(string token, string UserName,int ItemCode)
        {
            itemsDAL = new ItemsDAL(_config);
            ValidateToken = new ValidateTokenClass(_config);
            var ValidatedToken = ValidateToken.ValidateTWTToken(token,UserName, ref log, ref usersModel);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }



            if (!itemsDAL.DItem(ItemCode, ref log) && !string.IsNullOrEmpty(log))
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
        [HttpGet]
        [ResponseType(typeof(ImageModel))]
        [Route("ViewItemImage")]
        public ActionResult ViewItemImage(string token, string UserName, int ItemCode)
        {
            ModelState.Clear();
            string log = string.Empty;
            itemsDAL = new ItemsDAL(_config);
            ImageModel Image = new ImageModel();

            itemsDAL = new ItemsDAL(_config);
            ValidateToken = new ValidateTokenClass(_config);
            var ValidatedToken = ValidateToken.ValidateTWTToken(token, UserName,ref log, ref usersModel);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }

            Image = itemsDAL.QImage_Item(ItemCode, ref log);

            if (Image.ImageFile == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return BadRequest(ModelState);
            }

                return File(Image.ImageFile, @"image/"+Image.fileType);
               
           
        }

        [HttpGet]
  
        [Route("SearchItems")]
        public ActionResult SearchItems(string token,string Username, string Itemname)
        {
            ModelState.Clear();
            string log = string.Empty;
            List<ItemsModel> ItemList = new List<ItemsModel>();

            itemsDAL = new ItemsDAL(_config);
            ValidateToken = new ValidateTokenClass(_config);
            var ValidatedToken = ValidateToken.ValidateTWTToken(token, Username, ref log, ref usersModel);

            if (ValidatedToken == null && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return Unauthorized(ModelState);
            }

            ItemList = itemsDAL.QAllItems(Itemname, ref log);

            if ((ItemList == null || ItemList.Count() == 0) && !string.IsNullOrEmpty(log))
            {
                _logger.LogInformation(log);
                ModelState.AddModelError("ErrorMessage", log);

                return BadRequest(ModelState);
            }

            return Ok(ItemList);


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

            if (Path.GetExtension(newItem.file.FileName).ToLower() != ".png")
            {
                Log = "Only .png image are accepted.";
                return false;
            }
            return true;
        }

       
    }
}
