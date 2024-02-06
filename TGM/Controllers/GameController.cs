using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using TGM.Models;
using TGM.Models.DataBase.Entities;

namespace TGM.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly TgmDBContext _context;
        public GameController(TgmDBContext context, ILogger<UserController> logger)
        {

            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _context.Games.Include(u => u.User).ToListAsync());
                         
        }

       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var @group = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

       
        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Group @group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }

      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var @game = await _context.Games.FindAsync(id);
            if (@game == null)
            {
                return NotFound();
            }
            return View(@game);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Game @game)
        {
            if (id != @game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@game);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var @game = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@game == null)
            {
                return NotFound();
            }

            return View(@game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Games == null)
            {
                return Problem("Entity set 'WebAppDBContext.Groups'  is null.");
            }
            var @game = await _context.Games.FindAsync(id);
            if (@game != null)
            {
                _context.Games.Remove(@game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return (_context.Games?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
