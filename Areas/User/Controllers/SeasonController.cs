using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using vsports.Data;
using vsports.Models.SeasonOnTournamentsVM;

namespace vsports.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class SeasonController : Controller
    {
        private readonly ILogger<SeasonController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly string userName;

        public SeasonController(ILogger<SeasonController> logger, IMemoryCache memoryCache, ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor accessor) {
            _memoryCache = memoryCache;
            _configuration = configuration;
            _context = context;
            userName = accessor.HttpContext.User.Identity.Name ?? "";
            _logger = logger;
        }

        [Route("season/{id}")]
        public IActionResult Index(int id)
        {
            var tour = _context.Tournaments.Where(x=>x.Id == id).FirstOrDefault();
            var rs = _context.SeasonOnTournaments.Include(x=>x.Tournaments).Where(x=>x.TournamentsId == id).ToList();
            var items = new SeasonOnTournamentsVM
            {
                TournamentsMain = tour,
                SeasonOnTournamentsList = rs
            };
            return View(items);
        }  
        [Route("season/details/{id}")]
        public IActionResult Details(int id)
        {
            
            var rs = _context.SeasonOnTournaments.Where(x=>x.Id == id).FirstOrDefault();
            if(rs != null)
            {
                var items = new SeasonOnTournamentsVM
                {
                    SeasonOnTournamentsMain = rs
                };
                return View(items);
            }
            return NotFound();
            
        } 
        [Route("season/add")]
        public IActionResult Add()
        {
            return View();
        }
        // trang chi tiết giải đấu
       

    }
}
