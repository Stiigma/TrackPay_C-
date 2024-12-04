using System.ComponentModel.DataAnnotations.Schema;
using TrackPay.DataStructures;

namespace TrackPay.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public required string Concepto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool EsRecurrente { get; set; }
        public TipoFrecuencia Frecuencia { get; set; }

        public int Prioridad { get; set; } 

        // Cambiar a Enums
        public EstadoPago Estado { get; set; }
        public TipoPago Tipo { get; set; }

        public required string RutaImagen { get; set; }
        public required string Notas { get; set; }

        public required Usuario Usuario { get; set; }
    }
}
