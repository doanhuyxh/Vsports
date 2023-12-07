using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using vsports.Data;
using vsports.Middleware;
using vsports.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMvc();

//Add connetdatabase
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("MSSQL")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

builder.Services.AddMemoryCache(option =>
{
    option.ExpirationScanFrequency = TimeSpan.FromSeconds(30);
});

builder.Services.AddResponseCaching();

builder.Services.AddScoped<UserManager<ApplicationUser>>();

builder.Services.AddScoped<IIdentityDataInitializer, IdentityDataInitializer>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(7);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

//thêm session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Vsport";
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Thời gian không hoạt động trước khi phiên hết hạn
    options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập thông qua HTTP
    options.Cookie.IsEssential = true; // Đảm bảo phiên vẫn hoạt động ngay cả khi người dùng không đồng ý cookie
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "WebAppVsport"; // Tên cookie
    options.Cookie.Domain = ""; // Tên miền cookie áp dụng (nếu có)
    options.Cookie.Path = "/"; // Đường dẫn cookie áp dụng
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Chính sách bảo mật (SameAsRequest, Always, None)
    options.Cookie.HttpOnly = true; // Cookie chỉ được truy cập bằng HTTP (không bằng JavaScript)
    options.Cookie.SameSite = SameSiteMode.Strict; // Giới hạn cookie chỉ được gửi trong cùng nguồn (Strict, Lax, None)
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Thời gian hết hạn của cookie
    options.SlidingExpiration = true; // Cho phép cập nhật thời gian hết hạn khi có hoạt động từ người dùng
    options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn chuyển hướng khi truy cập bị từ chối
});


builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = long.MaxValue;
    options.MaxRequestBodyBufferSize = int.MaxValue;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();
app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.OnStarting(state =>
    {
        var httpContext = (HttpContext)state;
        httpContext.Response.Headers.Remove("Server");
        httpContext.Response.Headers.Remove("X-Powered-By");
        return Task.CompletedTask;
    }, context);

    await next();
});


app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        context.Context.Response.Headers.Add("Cache-Control", "private, max-age=172800");
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "user",
    areaName: "User",
    pattern: "User/{controller=UserHome}/{action=Index}/{id?}");


app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

app.UseStatusCodePages(async context =>
{
	var response = context.HttpContext.Response;

	if (response.StatusCode == 404)
	{
		response.Redirect("/not-found");
	}

});
app.MapRazorPages();
app.MapControllers();


app.Run();
