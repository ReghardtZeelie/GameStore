using Microsoft.AspNetCore.Mvc;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
