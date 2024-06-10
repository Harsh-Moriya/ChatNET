using ChatNet.Filters;
using ChatNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatNet.Controllers
{
    [TypeFilter(typeof(JwtAuthorizeFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        ConversationDAL convDal = null;
        UserDAL userDAL = null;

        public ConversationsController()
        {
            convDal = new ConversationDAL();
            userDAL = new UserDAL();
        }

        [HttpGet("getconversations/{userid}")]
        public IActionResult GetConversations(string userid)
        {
            List<Conversation> conversations = convDal.GetConversations(userid);

            return Ok(conversations);
        }

        [HttpPost("postconversation")]
        public IActionResult PostConversation(Users users)
        {
            string uone = users.uone;
            string utwo = users.utwo;

            string conversationid = convDal.CheckConversation(uone, utwo);

            if(string.IsNullOrEmpty(conversationid))
            {
                User userone = userDAL.CheckUserID(uone);
                User usertwo = userDAL.CheckUserID(utwo);
                conversationid = Guid.NewGuid().ToString();
                DateTime dateTime = DateTime.Now;

                bool success = convDal.CreateConversation(conversationid, userone, usertwo, dateTime);

                return Ok(new { success, existed = false, conversationid });
            } else
            {
                return Ok(new { success = true, existed = true, conversationid });
            }
        }
    }

    public class Users
    {
        public string uone { get; set; }
        public string utwo { get; set; }
    }
}
