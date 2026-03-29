using CinemaBaby.Models;
using CinemaBaby.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Kết nối SQLite
builder.Services.AddDbContext<CinemaBookingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Repository
builder.Services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// 🔥🔥🔥 FIX LOGIN - TẠO ADMIN CHẮC CHẮN
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CinemaBookingDbContext>();

    // tạo DB nếu chưa có
    context.Database.EnsureCreated();

    // nếu chưa có admin thì tạo
    if (!context.Admins.Any())
    {
        context.Admins.Add(new Admin
        {
            Username = "admin",
            Password = "123",
            FullName = "Administrator"
        });

        context.SaveChanges();
    }
}

app.Run();