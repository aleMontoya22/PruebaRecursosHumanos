using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RecursosHumanos.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RecursosHumanos.LoginController
{
    public class LoginController : Controller
    {
        private readonly DBContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public LoginController(DBContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ingresar([FromBody] Usuario usuario)
        {
            string hashIngresado = HashPasswordSHA256(usuario.Contraseña);

            var userInDb = _context.Usuarios
                .FirstOrDefault(u => u.Email == usuario.Email && u.Contraseña == hashIngresado);

            if (userInDb == null)
                return Json(new { success = false, message = "Credenciales incorrectas." });

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "¡A ingresado al Sistema!");

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, userInDb.Nombre),
        new Claim(ClaimTypes.Email, userInDb.Email)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Json(new { success = true });
        }

        private string HashPasswordSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }

     
}
