using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vsports.Data;

namespace vsports.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalUser = _context.Users.Where(i => i.IsActive == true).ToList().Count;
            ViewBag.Club = _context.SportClub.Where(i => i.IsDelete == false).ToList().Count;
            ViewBag.Sport = _context.Sport.Where(i => i.IsDelete == false).ToList().Count;            
            ViewBag.Tournaments = _context.Tournaments.Where(i=>i.IsDelete == false).ToList().Count;

            return View();
        }
    }
}
