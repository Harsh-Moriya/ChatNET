using ChatNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatNet.Controllers
{
    public class AccountController : Controller
    {
        UserDAL dal = null;
        private IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            dal = new UserDAL();
            _config = config;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            TempData["IsLogin"] = true;
            TempData["LoggedIn"] = null;
            TempData["IsRegister"] = null;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User u)
        {
            var user = AuthenticateUser(u);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                Response.Cookies.Append("JwtToken", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                TempData["LoggedIn"] = true;

                return RedirectToAction("Index", "Home");
            }

            TempData["LoginError"] = "Incorrect User ID or Password";

            return View();
        }

        private User AuthenticateUser(User u)
        {
            User user = dal.GetUser(u);

            return user;
        }

        private string GenerateJSONWebToken(User u)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, u.UserId),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IActionResult Signup()
        {
            TempData["IsLogin"] = null;
            TempData["LoggedIn"] = null;
            TempData["IsRegister"] = true;

            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user)
        {

            bool success = dal.AddUser(user);

            if (success)
            {
                TempData["loggedin"] = true;
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");

            return RedirectToAction("Login");
        }
    }
}
