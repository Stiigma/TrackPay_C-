using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackPay.datos;
using TrackPay.Models;
using TrackPay.DataStructures;

[Route("api/test")]
public class TestController : Controller
{
    private readonly ApplicationDbContext _context;

    public TestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("mostrar-pagos")]
    public IActionResult MostrarPagos()
    {
        // Recuperar los datos de la base de datos
        var pagos = _context.Pagos.Include(p => p.Usuario).ToList();

        // Imprimir los datos en la consola
        foreach (var pago in pagos)
        {
            Console.WriteLine($"Pago ID: {pago.Id}, Concepto: {pago.Concepto}, Monto: {pago.Monto}, FechaVencimiento: {pago.FechaVencimiento}");
        }

        // Procesar con ColaPrioridad
        var colaPrioridad = new ColaConPrioridad<Pago>();
        foreach (var pago in pagos)
        {
            colaPrioridad.Enqueue(pago, pago.FechaVencimiento, pago.Prioridad); // Usar la prioridad del pago
        }

        if (colaPrioridad.EsVacia())
        {
            Console.WriteLine("VACIO");
            ViewBag.Message = "No hay pagos disponibles para mostrar.";
            return View("EmptyQueue"); // Mostrar una vista alternativa
        }

        foreach (var item in colaPrioridad.ToList())
        {
            Console.WriteLine($"Pago ID: {item.Id}, Concepto: {item.Concepto}, Monto: {item.Monto}");
        }

        // Pasar la lista ordenada a la vista
        return View(colaPrioridad);
    }
}
