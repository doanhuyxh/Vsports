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

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}