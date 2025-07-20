using Microsoft.AspNetCore.Mvc;

namespace ChatNET.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
