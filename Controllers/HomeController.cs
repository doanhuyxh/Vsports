using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using vsports.Data;
using vsports.Models;
using vsports.Models.SportClubVM;

namespace vsports.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly string userName;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache, ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor accessor, UserManager<ApplicationUser> userManger)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _context = context;
            _userManager = userManger;
            userName = accessor.HttpContext.User.Identity.Name??"";
        }

        public IActionResult Index()
        {
            ViewBag.user = userName;
            return View();
        }

    }
}