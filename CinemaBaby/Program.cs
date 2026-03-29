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


// 🔥🔥🔥 SEED DATA (ADMIN + CINEMAS)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CinemaBookingDbContext>();

    // tạo DB nếu chưa có
    context.Database.EnsureCreated();

    // ===== ADMIN =====
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

    // ===== CINEMAS (20 rạp) =====
    if (!context.Cinemas.Any())
    {
        context.Cinemas.AddRange(
            new Cinema { Name = "CGV Vincom Đồng Khởi", Address = "TP.HCM", Latitude = 10.7769, Longitude = 106.7009 },
            new Cinema { Name = "CGV Crescent Mall", Address = "Quận 7, TP.HCM", Latitude = 10.7290, Longitude = 106.7219 },
            new Cinema { Name = "Lotte Cinema Nam Sài Gòn", Address = "Quận 7, TP.HCM", Latitude = 10.7420, Longitude = 106.7210 },
            new Cinema { Name = "Galaxy Nguyễn Du", Address = "Quận 1, TP.HCM", Latitude = 10.7758, Longitude = 106.6934 },
            new Cinema { Name = "BHD Star Bitexco", Address = "Quận 1, TP.HCM", Latitude = 10.7717, Longitude = 106.7041 },

            new Cinema { Name = "CGV Vincom Bà Triệu", Address = "Hà Nội", Latitude = 21.0112, Longitude = 105.8500 },
            new Cinema { Name = "Lotte Cinema Hà Nội Center", Address = "Hà Nội", Latitude = 21.0300, Longitude = 105.8000 },
            new Cinema { Name = "CGV Royal City", Address = "Hà Nội", Latitude = 21.0020, Longitude = 105.8160 },
            new Cinema { Name = "Beta Cineplex Mỹ Đình", Address = "Hà Nội", Latitude = 21.0285, Longitude = 105.7800 },
            new Cinema { Name = "Galaxy Mipec Long Biên", Address = "Hà Nội", Latitude = 21.0400, Longitude = 105.9000 },

            new Cinema { Name = "CGV Vincom Đà Nẵng", Address = "Đà Nẵng", Latitude = 16.0544, Longitude = 108.2022 },
            new Cinema { Name = "Lotte Cinema Đà Nẵng", Address = "Đà Nẵng", Latitude = 16.0600, Longitude = 108.2200 },
            new Cinema { Name = "Galaxy Đà Nẵng", Address = "Đà Nẵng", Latitude = 16.0471, Longitude = 108.2068 },

            new Cinema { Name = "CGV Vincom Hải Phòng", Address = "Hải Phòng", Latitude = 20.8449, Longitude = 106.6881 },
            new Cinema { Name = "Lotte Cinema Hải Phòng", Address = "Hải Phòng", Latitude = 20.8500, Longitude = 106.6800 },

            new Cinema { Name = "CGV Vincom Cần Thơ", Address = "Cần Thơ", Latitude = 10.0452, Longitude = 105.7469 },
            new Cinema { Name = "Lotte Cinema Cần Thơ", Address = "Cần Thơ", Latitude = 10.0300, Longitude = 105.7800 },

            new Cinema { Name = "CGV Vincom Biên Hòa", Address = "Đồng Nai", Latitude = 10.9574, Longitude = 106.8426 },
            new Cinema { Name = "Lotte Cinema Bình Dương", Address = "Bình Dương", Latitude = 11.3254, Longitude = 106.4770 },

            new Cinema { Name = "CGV Vincom Nha Trang", Address = "Khánh Hòa", Latitude = 12.2388, Longitude = 109.1967 }
        );

        context.SaveChanges();
    }
}

app.Run();