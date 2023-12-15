using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using vsports.Data;
using vsports.Models;
using vsports.Models.AccountViewModels;
using Bogus.Bson;

namespace vsports.Controllers
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;
        private readonly IConfiguration _iConfiguration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, ILogger<LoginViewModel> logger, IConfiguration iConfiguration)
        {
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _iConfiguration = iConfiguration;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login() { return View(); }

        [HttpGet]
        public IActionResult Register() { return View(); }

        [Route("/tai-khoan/dang-nhap")]

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && !user.IsActive)
            {
                return Json(new { code = 404, message = "Tài khoản bị khoá" });


            }
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(user);
                try
                {

                    var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role.FirstOrDefault()!),
                };

                    // Xây dựng ClaimsIdentity
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Thiết lập các thuộc tính xác thực (nếu có)
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // Thiết lập cho phép lưu cookie vĩnh viễn (remember me)
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1), // Thiết lập thời gian hết hạn sau 2 giờ
                    };


                    // Đăng ký phiên đăng nhập hiện tại vào HttpContext
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    if (role.Contains("Admin"))
                    {
                        return Json(new { code = 208, message = "Thành công", red = "/Admin" });
                    }
                    else
                    {
                        return Json(new { code = 200, message = "Thành công", section = true });
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }


            }
            else
            {

                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu bị lỗi");
                return Json(new { code = 400, message = "Tài khoản hoặc mật khẩu không tồn tại" });

            }


        }

        [Route("/tai-khoan/dang-ky-tai-khoan")]

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                // Username already exists
                ModelState.AddModelError("UserName", "The account already exists on the system");
                return Json(new { code = 400, message = "The account already exists on the system" });

            }
            if (model.ConfirmPassword != model.PasswordHash)
            {
                return Json(new { code = 400, message = "Passwords are not the same" });
            }
            ApplicationUser user = model;
            user.avatarImage = "/upload/img_avatar/blank_avatar.png";
            user.IsActive = true;
            user.Email = model.UserName;
            user.FullName = model.UserName;
            user.UserName = model.UserName;
            var result = await _userManager.CreateAsync(user, model.PasswordHash);

            if (result.Succeeded)
            {
                //_icommon.SendEmail(user);
                // Set role member
                await _userManager.AddToRoleAsync(user, "User");
                // Automatically sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                return Json(new { code = 200, message = "Thành công", section = true });
            }
            foreach (var error in result.Errors)
            {

                ModelState.AddModelError("", error.Description);
                return Json(new { code = 400, message = "Mật khẩu phải có ít nhất 1 chữ hoa và ký tự đặc biệt" });

            }

            return Json(new { code = 400, message = "Kiểm tra lại các trường" });

        }


        [Route("/dang-xuat")]

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
         new AuthenticationProperties { RedirectUri = "/Home/Index" }
          );
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            JsonResultVM json = new JsonResultVM();

            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null && !user.IsActive)
                {
                    json.StatusCode = 202;
                    json.Data = model;
                    json.Message = "Tài khoản bị khóa";
                    return Ok(json);
                }
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(user);

                    var claims = new List<Claim> {
                                     new Claim(ClaimTypes.Name, user.UserName),
                                     new Claim(ClaimTypes.Email, user.Email),
                                     new Claim(ClaimTypes.Role, role.FirstOrDefault()!),
                         };

                    // Xây dựng ClaimsIdentity
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Thiết lập các thuộc tính xác thực (nếu có)
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // Thiết lập cho phép lưu cookie vĩnh viễn (remember me)
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1), // Thiết lập thời gian hết hạn sau 2 giờ
                    };


                    // Đăng ký phiên đăng nhập hiện tại vào HttpContext
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    json.StatusCode = 200;
                    json.Data = user;
                    json.Message = "";
                    return Ok(json);
                }
                else
                {
                    json.Data += model;
                    json.Message = "Tài khoản hoặc mật khẩu không đúng";
                    json.StatusCode = 202;
                    return Ok(json);

                }

            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 404;
                return Ok(json);
            }
        }

    }
}
