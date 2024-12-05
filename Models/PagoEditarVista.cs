namespace TrackPay.Models
{
    public class PagoEditarVista
    {
        public int Id { get; set; }
        public string? Concepto { get; set; }

        public DateTime FechaVencimiento { get; set; }
        public string? Frecuencia { get; set; }
        public string? Tipo { get; set; }
        public decimal Monto { get; set; }
    }
}
