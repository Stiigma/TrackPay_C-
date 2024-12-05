using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrackPay.datos;
namespace TrackPay.Controllers
{
    public class EntradaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntradaController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Entrada()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Entrada(string username, string password)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.UserName == username && u.Contrasena == password);
            if (user != null)
            {
                // identidad del usuario
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.CorreoElectronico)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // usuario
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Pagos");
            }

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
