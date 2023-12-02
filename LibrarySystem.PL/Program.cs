using LibrarySystem.BLL.Services.BookService;
using LibrarySystem.BLL.Services.BorrowingService;
using LibrarySystem.BLL.Services.EmailService;
using LibrarySystem.BLL.Services.ImageService;
using LibrarySystem.BLL.Services.UserService;
using LibrarySystem.DAL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(
	CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option => {
		option.LoginPath = "/User/Login";
		option.ExpireTimeSpan = TimeSpan.FromMinutes(20);

	});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ConnectionSetting>(builder.Configuration.GetSection("ConnectionSetting"));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IEmailService, EmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();
