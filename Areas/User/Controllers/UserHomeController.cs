using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using vsports.Data;
using vsports.Models;
using vsports.Models.AccountViewModels;
using vsports.Services;

namespace vsports.Controllers
{
	[Area("User")]
	[Authorize(Roles = "User")]
	public class UserHomeController : Controller
	{
        private readonly ILogger<UserHomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ICommon _iCommon;

        public UserHomeController(ILogger<UserHomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ICommon common)
		{
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _iCommon = common;
        }

        [HttpGet("/user/profile")]
		public async Task<IActionResult> Index()
		{
			ViewBag.user = HttpContext.User.Identity.Name;
           
            var userCheck = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var items = new ProfileVM
            {
                ApplicationUserMain = userCheck
            };

            return View(items);
		}

	}
}