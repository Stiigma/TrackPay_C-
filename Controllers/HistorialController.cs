using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackPay.DataStructures;
using TrackPay.datos;
using TrackPay.Models;

namespace TrackPay.Controllers
{
    public class HistorialController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistorialController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Historial()
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(usuarioActual))
            {
                return RedirectToAction("Entrada", "Entrada");
            }

            var pagos = _context.Pagos.Include(p => p.Usuario).Where(p => p.Usuario.UserName == usuarioActual).OrderBy(p => p.FechaVencimiento).ToList();
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UserName == usuarioActual);


            decimal sumatoriaTotal = 0;
            decimal sumatoriaMensual = 0;
            int totalActivos = 0;
            Pila<Pago> stackHistorial = new Pila<Pago>();

            foreach (var pago in pagos)
            {
                if (pago.Estado != EstadoPago.cancelado && pago.Estado != EstadoPago.cancelado)
                    totalActivos += 1;

                if (pago.Estado == EstadoPago.Pagado)
                {
                    if (pago.FechaVencimiento.Month == DateTime.Now.Month && pago.FechaVencimiento.Year == DateTime.Now.Year)
                        sumatoriaMensual += pago.Monto;

                    sumatoriaTotal += pago.Monto;
                    stackHistorial.Push(pago);
                }

            }




            var VistaModelo = new HistorialVista
            {
                PilaHst = stackHistorial,
                SumatoriaMensual = sumatoriaMensual,
                SumatoriaTotal = sumatoriaTotal,
                Activos = totalActivos,
                Usuario = usuario
            };


            return View(VistaModelo);
        }


    }
}
