using Microsoft.AspNetCore.Mvc;

namespace RecursosHumanos.LoginController
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
