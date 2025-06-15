using AureusVitalis.Data;
using AureusVitalis.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IAuthService, AuthService>();

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

app.UseAuthorization();

// Добавляем маршруты для всех контроллеров
app.MapControllerRoute(
    name: "dailyHelper",
    pattern: "DailyHelper/{action=Selection}/{id?}",
    defaults: new { controller = "DailyHelper" });

app.MapControllerRoute(
    name: "education",
    pattern: "Education/{action=Main}/{id?}",
    defaults: new { controller = "Education" });

// Оставляем дефолтный маршрут последним
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();