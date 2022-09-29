using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterMVC.Areas.Admin.Attributes;
using RegisterMVC.DataAccess;


namespace RegisterMVC.Areas.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserDbContext _context;
        public HomeController(UserDbContext context)
        {
            _context = context;
        }

        [RoleAttribute(Role = "Admin")]
        public IActionResult Index()
        {
            var context = _context.RoleUser.Include(r => r.Role).Include(r => r.User);
            return View(context);
        }
    }
}

