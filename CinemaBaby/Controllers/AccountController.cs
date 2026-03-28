using CinemaBaby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace CinemaBaby.Controllers
{
    public class AccountController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public AccountController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        // ================= LOGIN =================
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // 🔴 ADMIN
            var admin = _context.Admins
                .FirstOrDefault(a => a.Username == username && a.Password == password);

            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username), // 🔥 SỬA Ở ĐÂY
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal
                );

                return RedirectToAction("Index", "Movies");
            }

            // 🔵 CUSTOMER
            var customer = _context.Customers
                .FirstOrDefault(c => c.Username == username && c.Password == password);

            if (customer != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, customer.Username), // 🔥 SỬA Ở ĐÂY
                    new Claim(ClaimTypes.Role, "Customer")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal
                );

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu rồi bạn ơi!";
            return View();
        }

        // ================= REGISTER =================
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            if (_context.Customers.Any(c => c.Username == customer.Username))
            {
                ViewBag.Error = "Tài khoản đã tồn tại!";
                return View();
            }

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ================= LOGOUT =================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // ================= ADMIN =================
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var user = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Customers.FirstOrDefault(c => c.CustomerId == id);

            if (user != null)
            {
                _context.Customers.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ================= 🔐 ĐỔI MẬT KHẨU =================

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                ViewBag.Error = "Không xác định được người dùng!";
                return View();
            }

            // 🔴 ADMIN
            var admin = _context.Admins.FirstOrDefault(a => a.Username == username);

            if (admin != null)
            {
                if (admin.Password != oldPassword)
                {
                    ViewBag.Error = "Mật khẩu cũ không đúng!";
                    return View();
                }

                admin.Password = newPassword;
                _context.SaveChanges();

                ViewBag.Success = "Đổi mật khẩu thành công!";
                return View();
            }

            // 🔵 CUSTOMER
            var customer = _context.Customers.FirstOrDefault(c => c.Username == username);

            if (customer != null)
            {
                if (customer.Password != oldPassword)
                {
                    ViewBag.Error = "Mật khẩu cũ không đúng!";
                    return View();
                }

                customer.Password = newPassword;
                _context.SaveChanges();

                ViewBag.Success = "Đổi mật khẩu thành công!";
                return View();
            }

            ViewBag.Error = "Không tìm thấy tài khoản!";
            return View();
        }
    }
}