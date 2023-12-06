using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using vsports.Data;

namespace vsports.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly string userName;

        public TournamentsController(IMemoryCache memoryCache, ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor accessor) {
            _memoryCache = memoryCache;
            _configuration = configuration;
            _context = context;
            userName = accessor.HttpContext.User.Identity.Name ?? "";
        }

        [Route("leagues")]
        public IActionResult Index()
        {
            return View();
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
