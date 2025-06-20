using System;
using AureusVitalis.Data;
using AureusVitalis.Infrastructure;   // EmailOptions
using AureusVitalis.Services;         // AuthService, CertificateService
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath         = "/Auth/Login";
        o.LogoutPath        = "/Auth/Logout";
        o.Cookie.Name       = "AureusAuth";
        o.ExpireTimeSpan    = TimeSpan.FromDays(7);
        o.SlidingExpiration = true;
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "dailyHelper",
    pattern: "DailyHelper/{action=Selection}/{id?}",
    defaults: new { controller = "DailyHelper" });

app.MapControllerRoute(
    name: "education",
    pattern: "Education/{action=Main}/{id?}",
    defaults: new { controller = "Education" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();
app.Run();