using System.Diagnostics;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterMVC.DataAccess;
using RegisterMVC.Models;

namespace RegisterMVC.Controllers;

public class HomeController : Controller
{
    public const string SessionKeyName = "_role";

    private readonly UserDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, UserDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Register([Bind("Email,Username,Password,ConfirmPassword")] User user)
    {
        if (ModelState.IsValid)
        {
            var emailExists = await _context.users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            var usernameExists = await _context.users.Where(u => u.Username == user.Username).FirstOrDefaultAsync();
            if(emailExists != null) return Ok("user with this email already exists");
            if (usernameExists != null) return Ok("User with this username already exists");
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _context.RoleUser.AddAsync(new RoleUser { UserId = user.Id, RoleId = 3 });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        var item = await _context.users.Include(x => x.RoleUsers).Where(u => u.Username == user.Username).FirstOrDefaultAsync();
        if (item != null)
        {
            bool verifiedPassword = BCrypt.Net.BCrypt.Verify(user.Password, item.Password);
            if (verifiedPassword)
            {
                var roleIds = item.RoleUsers;
                string allRoles = "";
                foreach (var roleId in roleIds)
                {
                    var role = await _context.roles.FirstOrDefaultAsync(u => u.Id == roleId.RoleId);
                    allRoles += role.Name;
                    allRoles += ",";
                }
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
                {
                    HttpContext.Session.SetString(SessionKeyName, allRoles);
                    HttpContext.Session.SetString("_username", item.Username);
                    HttpContext.Session.SetString("_connectionId", HttpContext.Connection.Id);
                }
                return View(nameof(Index));
            }
            else
            {
                return Ok("Wrong password");
            }
        }
        return Ok("User not found");
    }
}

