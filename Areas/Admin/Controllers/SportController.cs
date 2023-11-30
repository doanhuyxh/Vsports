using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vsports.Data;
using vsports.Models;
using vsports.Models.SportVM;

namespace vsports.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class SportController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SportController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> GetData()
		{
			JsonResultVM json = new JsonResultVM();
			json.StatusCode = 200;
			json.Data = await _context.Sport.Where(i => i.IsDelete == false).ToListAsync();
			return Ok(json);
		}

		public async Task<IActionResult> AddEditData(int id = 0)
		{
			SportVM sportVM = new SportVM();
			if (id != 0)
			{
				sportVM = await _context.Sport.FirstOrDefaultAsync(x => x.Id == id);
			}

			return PartialView("AddEditData", sportVM);
		}

		[HttpPost]
		public async Task<IActionResult> SaveData(SportVM vm)
		{
			JsonResultVM json = new JsonResultVM();
			try
			{
				Sport sport = new Sport();
				if (vm.Id != 0)
				{
					sport = await _context.Sport.FirstOrDefaultAsync(i => i.Id == vm.Id);
					sport.Name = vm.Name;
					_context.Update(sport);
					_context.SaveChanges();
				}
				else
				{
					sport = vm;
					sport.Created = DateTime.Now;
					sport.IsDelete = false;
					_context.Add(sport);
					_context.SaveChanges();
				}
				json.Data = sport;
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
			Sport sport = await _context.Sport.FirstOrDefaultAsync(i => i.Id == id);
			sport.IsDelete = true;
			_context.Update(sport);
			_context.SaveChanges();
			json.StatusCode = 200;
			return Ok(json);
		}
	}
}
