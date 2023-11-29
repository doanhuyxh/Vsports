using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using vsports.Models;

namespace vsports.Controllers
{
	[Area("User")]
	[Authorize(Roles = "User")]
	public class UserHomeController : Controller
	{

		public UserHomeController()
		{
		}

		public IActionResult Index()
		{
			return View();
		}

	}
}