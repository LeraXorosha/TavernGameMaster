using Microsoft.AspNetCore.Mvc;
using TGM.Models;

namespace TGM.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly TgmDBContext _context;
        public GameController(TgmDBContext context)
        {

            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
