using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterMVC.Attributes;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegisterMVC.Controllers
{
    public class ChatController : Controller
    {
        [CheckSignInAttribute]
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("_username");
            ViewBag.username = username;
            return View();
        }
    }
}

