using Microsoft.AspNetCore.Mvc;
using TrackPay.datos;
using TrackPay.Models;

namespace TrackPay.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrarController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View(new UsuarioRegistroVista());
        }

        [HttpPost]
        public IActionResult Registrar(UsuarioRegistroVista model)
        {
            if (ModelState.IsValid)
            {

                if (_context.Usuarios.Any(u => u.UserName == model.UserName))
                {
                    ViewBag.ErrorMessage = "El nombre de usuario ya existe.";
                    return View(model);
                }


                var newUser = new Usuario
                {
                    UserName = model.UserName,
                    Nombre_com = model.Nombre_com,
                    CorreoElectronico = model.CorreoElectronico,
                    Numero = model.Numero,
                    Contrasena = model.Contrasena,
                    Nacionalidad = model.Nacionalidad,
                    FechaNa = model.FechaNa
                };


                _context.Usuarios.Add(newUser);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Usuario registrado exitosamente.";
                return RedirectToAction("Entrada", "Entrada");
            }

            return View(model);
        }
    }
}

