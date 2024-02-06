using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TGM.Models.DataBase.Entities;
using TGM.Models.ViewModels;
using TGM.Models;
using Microsoft.EntityFrameworkCore;
using TGM.Models.DataBase;

namespace TGM.Controllers
{
    public class AccountController : Controller
    {
		private readonly ILogger<AccountController> _logger;
		private readonly TgmDBContext _db;

		public AccountController(ILogger<AccountController> logger, TgmDBContext context)
		{
			_logger = logger;
			_db = context;
		}

		// GET: Account/Profile
		[Authorize]
		public IActionResult Profile()
		{
			var currentLogin = HttpContext.User.Identity.Name;

			var profile = _db.Users.Where(u => u.Login == currentLogin)
				.Include(u => u.Role)
				.Include(u => u.Profile)
				.SingleOrDefault();

			return View(profile);
		}

		// GET: Account/Register
		public IActionResult Register()
		{
			return View();
		}

		// POST: Account/Register
		[HttpPost]
		public IActionResult Register(RegisterModel regUser)
		{
			if (!ModelState.IsValid || regUser.Password != regUser.ConfirmPassword)
			{
				ModelState.AddModelError("isRegFailed", "Пароль некорректный");
				return View(regUser);
			}
			if (_db.Users.Where(u => u.Login == regUser.Login || u.Login == regUser.Email || u.Email == regUser.Login || u.Email == regUser.Email).Any())
			{
				ModelState.AddModelError("isRegFailed", "Логин или почта уже существуют");
				return View(regUser);
			}

			var user = new User();
			user.Login = regUser.Login;
			user.Email = regUser.Email;
			user.Password = regUser.Password.ToHash();
			user.RoleId = 2;

			_db.Users.Add(user);
			_db.SaveChangesAsync().Wait();

			return RedirectToAction(nameof(Login));
		}

		// GET: Account/Login
		public IActionResult Login()
		{
			return View();
		}

		// POST: Account/Login
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel loginUser, bool failed = false)
		{

			var userToLogin = await _db.Users
				.Where(u =>
					u.Login == loginUser.LoginOrEmail ||
					u.Email == loginUser.LoginOrEmail)
				.Include(u => u.Role)
				.SingleOrDefaultAsync();
			if (userToLogin is null)
			{
				_logger.LogWarning("At {time} Failed login attempt was made with {login}", DateTime.Now.ToString("u"), loginUser.LoginOrEmail);
				ModelState.AddModelError("isLoginFailed", "Неверный логин");
				return View(loginUser);
			}
			if (userToLogin?.Password != loginUser.Password.ToHash())
			{
				_logger.LogWarning("At {time} Failed login attempt was made with {login}", DateTime.Now.ToString("u"), loginUser.LoginOrEmail);
				ModelState.AddModelError("isLoginFailed", "Неверный пароль");
				return View(loginUser);
			}

			Authenticate(userToLogin);
			switch (userToLogin.Role.Name)
			{
				case "player":
					return RedirectToAction("Index", "Player");
					break;
				case "master":
					return RedirectToAction("Index","Game");
					break;
			}
			return RedirectToAction("Index", "Home");
		}

		private void Authenticate(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
			};

			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

			HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id)).Wait();
		}

		[Authorize]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
			return RedirectToAction(nameof(Login), "Account");
		}


	}
}
