using ChatNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatNet.Controllers
{
    public class AccountController : Controller
    {
        UserDAL userDAL = null;

        public AccountController()
        {
            userDAL = new UserDAL();
        }

        public IActionResult Login()
        {
            TempData["islogin"] = true;

            return View();
        }

        [HttpPost]
        public IActionResult Login(User u)
        {
            
            User user = userDAL.GetUser(u);

            if (user != null)
            {
                HttpContext.Session.SetString("UserID", user.UserId);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Signup()
        {
            TempData["islogin"] = null;

            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user)
        {

            bool success = userDAL.AddUser(user);

            if (success)
            {
                TempData["loggedin"] = true;
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}
