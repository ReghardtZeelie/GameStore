using Microsoft.AspNetCore.Mvc;
using Models;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpPost]
        [Route("AddItemsToCart")]
        public ActionResult AddItemsToCart(CartModel cart, string token)
        {
            return Ok();
        }

        [HttpGet]
        [Route("ViewCart")]
        public ActionResult ViewCart(int UserID, string token)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("AddItemsToCart")]
        public ActionResult AddItemsToCart(int UserID,string itemdesc, string token)
        {
            return Ok();
        }
    }
}
