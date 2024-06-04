using ChatNet.Filters;
using ChatNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatNet.Controllers
{
    [TypeFilter(typeof(JwtAuthorizeFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserDAL userDAL = null;

        public UsersController() 
        {
            userDAL = new UserDAL();
        }

        [HttpGet("getusers/{userid}/{username}/{currentpage}")]
        public IActionResult GetUsers(string userid, string username, int currentpage)
        {
            List<User> users = userDAL.GetUsers(userid, username);
            int increase = 8;
            int startIndex = (currentpage - 1) * increase;
            int endIndex = currentpage * increase;
            if(endIndex > users.Count)
            {
                endIndex = users.Count;
            }

            List<User> sendUsers = new List<User>();
            for(int i = startIndex; i < endIndex; i++)
            {
                sendUsers.Add(users[i]);
            }

            return Ok(sendUsers);
        }
    }
}
