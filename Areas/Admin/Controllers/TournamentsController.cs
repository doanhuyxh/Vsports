using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using vsports.Models;
using vsports.Models.TournamentsVM;
using vsports.Data;
using Bogus;

namespace vTournamentss.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TournamentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memory;

        public TournamentsController(ApplicationDbContext context, IMemoryCache memory)
        {
            _context = context;
            _memory = memory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            json.StatusCode = 200;
            List<TournamentsVM> tournaments = _context.Tournaments
                                              .Include(t => t.Organizer)
                                               .Include(t => t.Sport)
                                               .Where(t => t.IsDelete == false)
                                               .Select(t => new TournamentsVM
                                               {
                                                   Id = t.Id,
                                                   SportId = t.SportId,
                                                   UserId = t.UserId,
                                                   Name = t.Name,
                                                   SportName = t.Sport.Name,
                                                   OrganizerName = t.Organizer.FullName,
                                               }).ToList();

            json.Data = tournaments;
            return Ok(json);
        }

        public async Task<IActionResult> AddEditData(int id = 0)
        {
            TournamentsVM TournamentsVM = new TournamentsVM();
            if (id != 0)
            {
                TournamentsVM = await _context.Tournaments.FirstOrDefaultAsync(x => x.Id == id);
            }

            return PartialView("AddEditData", TournamentsVM);
        }

        [HttpPost]
        public async Task<IActionResult> SaveData(TournamentsVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                Tournaments Tournaments = new Tournaments();
                if (vm.Id != 0)
                {
                    Tournaments = await _context.Tournaments.FirstOrDefaultAsync(i => i.Id == vm.Id);
                    Tournaments.Name = vm.Name;
                    _context.Update(Tournaments);
                    _context.SaveChanges();
                }
                else
                {
                    Tournaments = vm;
                    Tournaments.Created = DateTime.Now;
                    Tournaments.IsDelete = false;
                    _context.Add(Tournaments);
                    _context.SaveChanges();
                }
                json.Data = Tournaments;
                json.Message = "";
                json.StatusCode = 200;

                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Data = vm;
                json.Message = ex.Message;
                json.StatusCode = 500;
                return Ok(json);
            }
        }

        public async Task<IActionResult> DeleteData(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Tournaments Tournaments = await _context.Tournaments.FirstOrDefaultAsync(i => i.Id == id);
            Tournaments.IsDelete = true;
            _context.Update(Tournaments);
            _context.SaveChanges();
            json.StatusCode = 200;
            return Ok(json);
        }

        public IActionResult Detail(int id)
        {
            return PartialView("Detail");
        }

        public IActionResult AddFakeData()
        {
            List<ApplicationUser> users = _context.Users.ToList();

            foreach (var user in users)
            {
                Tournaments tournaments = new Faker<Tournaments>()
                                               .RuleFor(t => t.IsDelete, f => false)
                                               .RuleFor(t => t.Created, DateTime.Now)
                                               .RuleFor(t => t.Name, f => f.Lorem.Lines(1))
                                               .RuleFor(t => t.UserId, user.Id)
                                               .RuleFor(t => t.SportId, f => f.Random.Int(1, 2))
                                               .Generate();
                _context.Add(tournaments);
                _context.SaveChanges();
            }

            return Ok();
        }
    }
}
