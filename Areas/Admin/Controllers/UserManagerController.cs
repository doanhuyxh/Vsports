using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using vsports.Data;
using vsports.Models;
using vsports.Models.AccountViewModels;

namespace vsports.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _memoryCache;

        public UserManagerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMemoryCache memoryCache)
        {
            _context = context;
            _userManager = userManager;
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
                json.Message = "";
                json.StatusCode = 200;
                var rs = _memoryCache.Get("all_user");
                if (rs != null)
                {
                    json.Data = rs;
                }
                else
                {
                    List<ApplicationUser> data = _context.Users.FromSqlRaw("SELECT * FROM AspNetUsers").AsNoTracking().ToList<ApplicationUser>();
                    json.Data = data;
                    _memoryCache.Set("all_user", data);
                }
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Data = ex;
                json.Message = ex.Message;
                json.StatusCode = 500;
                return Ok(json);
            }
        }


        public async Task<IActionResult> AddEditUser(string id)
        {
            UserAddEdit vm = new UserAddEdit();

            if (id != "0")
            {
                vm = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            }

            return PartialView(vm);
        }


        public async Task<IActionResult> AddManyUser(int number)
        {
            _memoryCache.Remove("all_user");
            try
            {
                for (int i = 0; i < number; i++)
                {
                    ApplicationUser fakeUsers = UserFaker.GenerateFakeUsers(i+1).Generate();
                    await _userManager.CreateAsync(fakeUsers, "12345678");
                    await _userManager.AddToRoleAsync(fakeUsers, "User");
                }
                return Json(new
                {
                    StatusCode = 200,
                });
            }
            catch (Exception ex) { }
            {
                return Json(new
                {
                    StatusCode = 500,
                });
            }
        }


    }
}
