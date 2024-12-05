using TrackPay.DataStructures;

namespace TrackPay.Models
{
    public class HistorialVista
    {
        public Pila<Pago>? PilaHst { get; set; }
        public decimal SumatoriaMensual { get; set; }
        public decimal SumatoriaTotal { get; set; }
        public int Activos { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
