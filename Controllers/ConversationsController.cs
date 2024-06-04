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

        [HttpGet("postconversation/{uone}/{utwo}")]
        public IActionResult PostConversation(string uone, string utwo)
        {
            User userone = userDAL.CheckUserID(uone);
            User usertwo = userDAL.CheckUserID(utwo);
            string conversationid = Guid.NewGuid().ToString();
            DateTime dateTime = DateTime.Now;

            bool success = convDal.CreateConversation(conversationid, userone, usertwo, dateTime);

            return Ok(new {success});
        }
    }
}
