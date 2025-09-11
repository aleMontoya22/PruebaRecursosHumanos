using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecursosHumanos.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace RecursosHumanos.LoginController
{    
    public class SignUpController : Controller
    {
        private readonly DBContext _context;

        public SignUpController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Registrar([FromBody] Usuario usuario)
        {
            Console.WriteLine("Método Registrar llamado");
            Console.WriteLine($"Datos recibidos: {usuario?.Nombre}, {usuario?.Email}");

            try
            {
                if (usuario == null || string.IsNullOrWhiteSpace(usuario.Nombre))
                    return Json(new { success = false, message = "Datos inválidos" });

                usuario.Contraseña = HashPasswordSHA256(usuario.Contraseña);
                _context.Usuarios.Add(usuario);
                int result = _context.SaveChanges();

                Console.WriteLine($"Resultado SaveChanges: {result}");

                return Json(new { success = result > 0 });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "Error interno" });
            }
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
    }
}
