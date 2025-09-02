using Microsoft.AspNetCore.Mvc;

namespace RecursosHumanos.LoginController
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
