using ChatNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ChatNet.Filters;

namespace ChatNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ConversationDAL convDAL = null;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            convDAL = new ConversationDAL();
        }

        [TypeFilter(typeof(JwtAuthorizeFilter))]
        public IActionResult Index()
        {
            TempData["IsLogin"] = null;

            return View();
        }

        [TypeFilter(typeof(JwtAuthorizeFilter))]
        [HttpPost]
        public IActionResult _ConversationPanel(string conversationid)
        {
            Conversation conv = convDAL.GetConversation(conversationid);

            return PartialView("_ConversationPanel", conv);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
