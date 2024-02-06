using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TGM.Models;
using TGM.Models.DataBase.Entities;

namespace TGM.Controllers
{
    public class UserController : Controller

    {
        private readonly ILogger<UserController> _logger;
        private readonly TgmDBContext _context;
        public UserController(TgmDBContext context, ILogger<UserController> logger)
        {

            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserProfiles.Include(u => u.User).ToListAsync());
        }
    }
}
