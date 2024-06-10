using ChatNet.Filters;
using ChatNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace ChatNet.Controllers
{
    [TypeFilter(typeof(JwtAuthorizeFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        MessageDAL msgDAL = null;

        public MessagesController()
        {
            msgDAL = new MessageDAL();
        }

        [HttpGet("getmessages/{conversationid}/{mtimestamp}")]
        public IActionResult GetMessages(string conversationid, string? mtimestamp)
        {
            DateTime Mtimestamp = DateTime.Now;

            if (!mtimestamp.Equals("_novalue_"))
            {
                Mtimestamp = Convert.ToDateTime(mtimestamp);
            }

            List<Message> messages = msgDAL.GetMessages(conversationid, Mtimestamp);

            return Ok(messages);
        }

        [HttpGet("getnewmessages/{conversationid}/{mtimestamp}")]
        public IActionResult GetNewMessages(string conversationid, string? mtimestamp)
        {
            DateTime MTimestamp = DateTime.Parse("01/01/1754");

            if (!mtimestamp.Equals("_novalue_"))
            {
                MTimestamp = Convert.ToDateTime(mtimestamp);
            }

            List<Message> newMessages = msgDAL.GetNewMessages(conversationid, MTimestamp);

            return Ok(newMessages);
        }

        [HttpPost("postmessage")]
        public IActionResult PostMessage(CapturedMessage message)
        {
            Message newMessage = new Message
            {
                MessageID = Guid.NewGuid().ToString(),
                ConversationID = message.ConversationID,
                Sender = message.Sender,
                Receiver = message.Receiver,
                Msg = message.Msg,
                mTimestamp = DateTime.Now
            };

            bool success = msgDAL.AddMessage(newMessage);

            return Ok(new { success });
        }

        [HttpGet("checkmessages/{conversationid}/{mtimestamp}")]
        public IActionResult CheckMessages(string conversationid, string mtimestamp)
        {
            DateTime LMTimestamp = DateTime.Parse("01/01/1754");

            if (!mtimestamp.Equals("_novalue_"))
            {
                LMTimestamp = Convert.ToDateTime(mtimestamp);
            }

            int newMessages = msgDAL.CheckMessages(conversationid, LMTimestamp);

            return Ok(newMessages);
        }
    }

    public class CapturedMessage {
        public string ConversationID { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Msg { get; set; }
    }
}
