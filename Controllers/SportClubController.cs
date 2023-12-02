using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using vsports.Data;
using vsports.Models;
using vsports.Models.SportClubVM;

namespace vsports.Controllers
{
    public class SportClubController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public SportClubController(ILogger<HomeController> logger, IMemoryCache memoryCache, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _context = context;
        }

        [Route("SportClub")]
        public IActionResult Index()
        {
            var rs = _memoryCache.Get("all_sport_club");
            if (rs == null)
            {
                List<SportClubVM> sportClubs = _context.SportClub
                                .Include(sc => sc.Owner)
                                .Include(sc => sc.Sport)
                                .Where(sc => sc.IsDelete == false)
                                .Select(sc => new SportClubVM
                                {
                                    Id = sc.Id,
                                    Name = sc.Name,
                                    SportId = sc.SportId,
                                    SportName = sc.Sport.Name,
                                    SportsCoach = sc.SportsCoach,
                                    AvatarImage = sc.AvatarImage,
                                    BackgroudImage = sc.BackgroudImage,
                                    Address = sc.Address,
                                    PhoneNumber = sc.PhoneNumber,
                                    Email = sc.Email,
                                    Description = sc.Description,
                                    ClubRules = sc.ClubRules,
                                    Status = sc.Status,
                                    Point = sc.Point,
                                    OwnerId = sc.OwnerId,
                                    OwnerName = sc.Owner.FullName
                                })
                                .ToList();
                _memoryCache.Set("all_sport_club", sportClubs);

                return View(sportClubs);

            }
            else
            {
                return View((List<SportClubVM>)rs);
            }
        }

        [Route("SportClub/{id}")]
        public async Task<IActionResult> SportDetail([FromRoute] int id)
        {
            SportClubVM sportClubVM = await _context.SportClub.FirstOrDefaultAsync(i=>i.Id == id);
            sportClubVM.OwnerName =  _context.Users.FirstOrDefault(i => i.Id == sportClubVM.OwnerId).FullName;
            sportClubVM.SportName = _context.Sport.FirstOrDefault(i => i.Id == sportClubVM.SportId).Name;
            sportClubVM.clubMembers = await _context.ClubMember.Where(i=>i.SportClubId == sportClubVM.Id).ToListAsync();
            ViewBag.CountMember = sportClubVM.clubMembers.Count;
            return View(sportClubVM);
        }

        [Route("SportClub/{id}/member")]
        public async Task<IActionResult> Member([FromRoute] int id)
        {
            SportClubVM sportClubVM = await _context.SportClub.FirstOrDefaultAsync(i=>i.Id == id);
            sportClubVM.OwnerName =  _context.Users.FirstOrDefault(i => i.Id == sportClubVM.OwnerId).FullName;
            sportClubVM.SportName = _context.Sport.FirstOrDefault(i => i.Id == sportClubVM.SportId).Name;
            sportClubVM.clubMembers = await _context.ClubMember.Where(i=>i.SportClubId == sportClubVM.Id).ToListAsync();
            ViewBag.CountMember = sportClubVM.clubMembers.Count;
            return View(sportClubVM);
        }

    }
}