using Bogus;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using vsports.Data;
using vsports.Models;
using vsports.Models.SportClubVM;
using vsports.Models.SportVM;

namespace vsports.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class SportClubController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public SportClubController(ApplicationDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> GetData()
		{
			JsonResultVM json = new JsonResultVM();
			try
			{
				json.StatusCode = 200;
				var rs = _memoryCache.Get("all_sport_club");
				if (rs == null)
				{
					List<SportClubVM> sportClubs = _context.SportClub
									.Include(sc => sc.Owner)
									.Include(sc => sc.Sport)
									.Where(sc=>sc.IsDelete == false)
									.Select(sc=> new SportClubVM
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

					json.Data = sportClubs;
				}
				else
				{
					json.Data = rs;
				}

				return Ok(json);
			}
			catch (Exception ex)
			{
				json.Message = ex.Message;
				json.StatusCode = 500;
				json.Data = ex;
				return Ok(json);
			}

		}

		public async Task<IActionResult> AddEditData(int id = 0)
		{
			SportClubVM sportCludVM = new SportClub();
			if (id != 0)
			{
				sportCludVM = await _context.SportClub.FirstOrDefaultAsync(x => x.Id == id);
			}

			return PartialView("AddEditData", sportCludVM);
		}

		[HttpPost]
		public async Task<IActionResult> SaveData(SportClubVM vm)
		{
			JsonResultVM json = new JsonResultVM();
			try
			{
				SportClub sportClud = new SportClub();
				if (vm.Id != 0)
				{
					sportClud = await _context.SportClub.FirstOrDefaultAsync(i => i.Id == vm.Id);
					_context.Entry(sportClud).CurrentValues.SetValues(vm);
					_context.Update(sportClud);
					_context.SaveChanges();
				}
				else
				{
					sportClud = vm;
					sportClud.Created = DateTime.Now;
					sportClud.IsDelete = false;
					_context.Add(sportClud);
					_context.SaveChanges();
				}
				json.Data = sportClud;
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
			SportClub sportClud = await _context.SportClub.FirstOrDefaultAsync(i => i.Id == id);
			sportClud.IsDelete = true;
			_context.Update(sportClud);
			_context.SaveChanges();
			json.StatusCode = 200;
			return Ok(json);
		}

		public async Task<IActionResult> Detail(int id)
		{
			SportClubVM vm = await _context.SportClub.FirstOrDefaultAsync(i=>i.Id == id);
			vm.OwnerName = _context.Users.FirstOrDefault(i=>i.Id == vm.OwnerId).FullName;
			vm.SportName = _context.Sport.FirstOrDefault(i=>i.Id == vm.SportId).Name;
			return PartialView("Detail", vm);
		}

		public ActionResult FakeSporClub()
		{
			_memoryCache.Remove("all_sport_club");

            List<ApplicationUser> users = _context.ApplicationUser.ToList();

			foreach (var user in users)
			{
				SportClub sport = new Faker<SportClub>()
										.RuleFor(s=>s.Name, f=>f.Company.CompanyName())
										.RuleFor(s=>s.SportId, f=>f.Random.Int(1,2))
										.RuleFor(s=>s.OwnerId, f=>user.Id)
										.RuleFor(s=>s.SportsCoach, f=>f.Name.FindName())
										.RuleFor(s=>s.AvatarImage, "/upload/img_avatar/blank_avatar.png")
										.RuleFor(s=>s.BackgroudImage, "/upload/img_backgroud/img_bg_bank.png")
										.RuleFor(s=>s.Address, f=>f.Address.FullAddress())
										.RuleFor(s=>s.PhoneNumber, f=>f.Phone.PhoneNumber())
										.RuleFor(s=>s.Email, f=>f.Internet.Email())
										.RuleFor(s=>s.Description, f=>f.Lorem.Paragraph())
										.RuleFor(s=>s.ClubRules, f=>f.Lorem.Paragraph())
										.RuleFor(s=>s.Status, "Public")
										.RuleFor(s=>s.Point, f=>f.Random.Int(1, 1000))
										.RuleFor(s=>s.Created, DateTime.Now)
										.RuleFor(s=>s.IsDelete, false)
                                        .Generate();
				_context.Add(sport);
				_context.SaveChanges();
			}

			return Ok(new { StatusCode =200 });
		}
	}
}
