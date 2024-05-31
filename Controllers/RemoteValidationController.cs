using ChatNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatNet.Controllers
{
    public class RemoteValidationController : Controller
    {
        UserDAL dal = null;

        public RemoteValidationController()
        {
            dal = new UserDAL();
        }

        public IActionResult IsUserIDValid(string userid)
        {
            if (TempData["islogin"]  != null)
            {
                return Json(true);
            }

            User user = dal.CheckUserID(userid);

            if(user != null)
            {
                return Json("User ID is already taken!");
            }

            return Json(true);
        }

        public IActionResult IsUserEmailValid(string email)
        {
            if (TempData["islogin"] != null)
            {
                return Json(true);
            }

            User user = dal.CheckUserEmail(email);

            if (user != null)
            {
                return Json("User Email is already taken!");
            }

            return Json(true);
        }
    }
}
