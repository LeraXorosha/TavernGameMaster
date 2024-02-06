using Microsoft.AspNetCore.Mvc;

namespace TGM.Controllers
{
    public class PlayerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
