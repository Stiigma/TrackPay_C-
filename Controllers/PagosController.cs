using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackPay.DataStructures;
using TrackPay.datos;
using TrackPay.Models;

namespace TrackPay.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int tipoPago = -1)
        {

            var usuarioActual = HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(usuarioActual))
            {
                return RedirectToAction("Entrada", "Entrada");
            }

            // Obtener los pagos del usuario actual
            var pagos_validar = _context.Pagos.Include(p => p.Usuario).Where(p => p.Usuario.UserName == usuarioActual).ToList();
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UserName == usuarioActual);

            var colaPrioridad = new ColaConPrioridad<Pago>();
            decimal sumatoriaMensual = 0;
            foreach (var pago in pagos_validar)
            {
                var pagoNuevo = pago.VerificarYProcesar();
                if (pagoNuevo != null)
                {
                    _context.Pagos.Add(pagoNuevo);

                }
            }
            _context.SaveChanges();

            var pagos_validados = _context.Pagos.Include(p => p.Usuario).Where(p => p.Usuario.UserName == usuarioActual).ToList();
            foreach (var pago in pagos_validados)
            {

                if ((tipoPago == 0 && !pago.EsRecurrente) || (tipoPago == 1 && pago.EsRecurrente) || tipoPago == -1)
                {
                    if (pago.Estado != EstadoPago.cancelado && pago.Estado != EstadoPago.Pagado)
                    {
                        colaPrioridad.Enqueue(pago, pago.FechaVencimiento, pago.Prioridad);
                        if (pago.FechaVencimiento.Month == DateTime.Now.Month && pago.FechaVencimiento.Year == DateTime.Now.Year)
                        {
                            sumatoriaMensual += pago.Monto;
                        }
                    }
                }



            }



            var VistaModelo = new PagosVista
            {
                Pagos = colaPrioridad,
                SumatoriaMensual = sumatoriaMensual,
                Usuario = usuario
            };

            ViewBag.TipoPago = tipoPago;

            return View(VistaModelo);
        }

        [HttpGet("api/payments/{id}")]
        public IActionResult GetPayment(int id)
        {
            var pago = _context.Pagos.FirstOrDefault(p => p.Id == id);
            if (pago == null)
                return NotFound();

            return Ok(new
            {
                nombre = pago.Concepto,
                fechaVencimiento = pago.FechaVencimiento,
                temporalidad = pago.Frecuencia,
                tipo = pago.Tipo,
                cantidad = pago.Monto
            });
        }

        [HttpPost("api/payments/update")]
        public IActionResult UpdatePayment([FromBody] PagoEditarVista PagoActualizado)
        {

            if (PagoActualizado == null)
            {
                Console.WriteLine("El modelo recibido es nulo.");
                return BadRequest("No se recibieron datos.");
            }

            var pago = _context.Pagos.FirstOrDefault(p => p.Id == PagoActualizado.Id);
            if (pago == null)
            {
                return NotFound("No se encontró el pago.");
            }


            pago.Concepto = PagoActualizado.Concepto;
            pago.Monto = PagoActualizado.Monto;
            pago.FechaVencimiento = PagoActualizado.FechaVencimiento;


            if (Enum.TryParse(typeof(TipoFrecuencia), PagoActualizado.Frecuencia, true, out var frecuenciaEnum))
            {
                pago.Frecuencia = (TipoFrecuencia)frecuenciaEnum;
            }
            else
            {
                return BadRequest("Frecuencia no válida.");
            }


            if (Enum.TryParse(typeof(TipoPago), PagoActualizado.Tipo, true, out var tipoEnum))
            {
                pago.Tipo = (TipoPago)tipoEnum;
            }
            else
            {
                return BadRequest("Tipo de pago no válido.");
            }


            _context.SaveChanges();

            return Ok(new { message = "Pago actualizado correctamente." });
        }


        [HttpPost]
        public IActionResult FormPagoUnico(string fecha_fin, string Hora, Pago pago)
        {
            Console.WriteLine("Iniciando método FormPagoUnico...");
            Console.WriteLine($"Fecha_fin: {fecha_fin}, Hora: {Hora}");
            Console.WriteLine($"Pago.Concepto: {pago.Concepto}, Pago.Monto: {pago.Monto}, Pago.Prioridad: {pago.Prioridad}");

            var usuarioActual = HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(usuarioActual))
            {
                Console.WriteLine("Usuario no autenticado, redirigiendo a Entrada...");
                return RedirectToAction("Entrada", "Entrada");
            }

            Console.WriteLine($"Usuario autenticado: {usuarioActual}");

            var usuario = _context.Usuarios.FirstOrDefault(u => u.UserName == usuarioActual);

            if (usuario == null)
            {
                Console.WriteLine("Usuario no encontrado en la base de datos.");
                ModelState.AddModelError("", "No se encontró el usuario.");
                return RedirectToAction("Entrada", "Entrada");
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState es válido.");

                if (DateTime.TryParse(fecha_fin, out DateTime fechaParsed) &&
                    TimeSpan.TryParse(Hora, out TimeSpan horaParsed))
                {
                    pago.FechaVencimiento = fechaParsed.Add(horaParsed);
                    Console.WriteLine($"FechaVencimiento combinada: {pago.FechaVencimiento}");
                }
                else
                {
                    Console.WriteLine("Error al parsear Fecha_fin o Hora.");
                    ModelState.AddModelError("", "Fecha u hora inválidas.");
                    return View(pago);
                }

                pago.EsRecurrente = false;
                pago.Frecuencia = 0;
                pago.Estado = 0;
                pago.Notas = "Ninguna";
                pago.RutaImagen = "Ninguna";
                pago.UsuarioId = usuario.Id;

                Console.WriteLine($"Preparando para agregar el pago a la base de datos...");
                Console.WriteLine($"Pago: Concepto={pago.Concepto}, Monto={pago.Monto}, Prioridad={pago.Prioridad}");

                _context.Pagos.Add(pago);
                _context.SaveChanges();

                Console.WriteLine("Pago único agregado correctamente.");
                TempData["Mensaje"] = "Pago único agregado correctamente.";
                return RedirectToAction("index", "Pagos");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error en ModelState: {error.ErrorMessage}");
                }
                return View(pago);
            }


        }

        // Procesar el formulario de pagos recurrentes
        [HttpPost]
        public IActionResult GuardarPagoRecurrente(string fecha_fin, string Hora, Pago pago)
        {
            // Recuperar el usuario autenticado
            var usuarioActual = HttpContext.User.Identity.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UserName == usuarioActual);

            if (usuario == null)
            {

                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {

                if (DateTime.TryParse(fecha_fin, out DateTime fechaParsed) &&
                    TimeSpan.TryParse(Hora, out TimeSpan horaParsed))
                {
                    pago.FechaVencimiento = fechaParsed.Add(horaParsed);
                }
                else
                {

                    ModelState.AddModelError("", "Fecha u hora inválidas.");
                    return View(pago);
                }
                pago.EsRecurrente = true;
                pago.UsuarioId = usuario.Id;
                pago.Notas = "Ninguna";
                pago.RutaImagen = "Ninguna";
                pago.Estado = 0;
                _context.Pagos.Add(pago);
                _context.SaveChanges();

                TempData["Mensaje"] = "Pago recurrente agregado correctamente.";
                return RedirectToAction("Index"); // Redirigir al listado de pagos
            }

            return View("Error");
        }


        [HttpPost("api/payments/cancel")]
        public IActionResult CancelPayment([FromBody] int paymentId)
        {

            var pago = _context.Pagos.FirstOrDefault(p => p.Id == paymentId);

            if (pago == null)
            {
                return NotFound("No se encontró el pago.");
            }


            pago.cambiarEstado(EstadoPago.cancelado);


            _context.SaveChanges();

            return Ok(new { message = "El pago ha sido cancelado correctamente.", pagoId = pago.Id });
        }
    }
}
