using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using vsports.Data;
using vsports.Models;

namespace vsports.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class TournamentsController : Controller
    {
        private readonly ILogger<TournamentsController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly string userName;
        private readonly UserManager<ApplicationUser> _userManager;


        public TournamentsController(ILogger<TournamentsController> logger, IMemoryCache memoryCache, ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor accessor, UserManager<ApplicationUser> userManager)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
            _context = context;
            userName = accessor.HttpContext.User.Identity.Name ?? "";
            _logger = logger;
            _userManager = userManager;
        }

        [Route("/user/leagues")]
        public IActionResult Index()
        {
            ViewBag.user = HttpContext.User.Identity.Name;

            var user = _context.ApplicationUser.Where(x => x.UserName == userName).FirstOrDefault();
            if (user != null)
            {
                var rs = _context.Tournaments.Include(x=>x.SportClubOnTournaments).Where(x => x.UserId == user.Id).Take(12).ToList();

                return View(rs);
            }
            return View();

        } 
        [HttpPost("/user/leagues/add")]
        public async Task<IActionResult> Add (Tournaments model)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {

                    var userCheck = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                    model.SportId = 1;
                    model.Created = DateTime.Now;
                    model.UserId = userCheck.Id;
                    model.Point = 0;
                    model.IsDelete = false;
                    model.BackgroudImage = "/upload/img/cover.jpg";
                    model.AvatarImage = "/upload/img/vstation.jpg";

                    _context.Tournaments.Add(model);
                    await _context.SaveChangesAsync();

                    return Json(new { code = 200, message = "Thành công" });
                }
                return Json(new { code = 404, message = "Bạn chưa đăng nhập" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 404, message = ex.Message });

            }



        }
        // trang chi tiết giải đấu
        [Route("leagues/{id}")]
        public IActionResult Detail(int id)
        {
            return View();
        }

        // câu lạc bộ trong giải đấu
        [Route("leagues/{id}/teams")]
        public IActionResult Teams(int id)
        {
            return PartialView("_Teams");
        }

        //lich thi dau
        [Route("leagues/{id}/schedules")]
        public IActionResult Schedules(int id)
        {
            return PartialView("_Schedules");
        }
        // trang hiển thi chọn lịch xuất
        [Route("leagues/{id}/schedules/export")]
        public IActionResult Export(int id)
        {
            return View();
        }

        [Route("leagues/{id}/schedules/export/1")]
        public IActionResult ExportImg1(int id)
        {
            return View();
        }

        [Route("leagues/{id}/schedules/export/2")]
        public IActionResult ExportImg2(int id)
        {
            return View();
        }

        [Route("leagues/{id}/schedules/export/3")]
        public IActionResult ExportImg3(int id)
        {
            return View();
        }


    }
}
