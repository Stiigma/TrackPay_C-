using System.ComponentModel.DataAnnotations.Schema;
using TrackPay.DataStructures;

namespace TrackPay.Models
{
    public class PagosVista
    {
        public ColaConPrioridad<Pago>? Pagos { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SumatoriaMensual { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
