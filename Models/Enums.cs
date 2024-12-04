namespace TrackPay.Models
{
    public enum EstadoPago
    {
        Pendiente = 0,
        Pagado = 1,
        notificado = 2,
        cancelado = 3
    }

    public enum TipoPago
    {
        Bancario = 0,
        Entretenimiento = 1,
        Salud = 2,
        Servicio = 3
    }

    public enum TipoFrecuencia
    {
        Ninguna = 0,
        Diario = 1,
        Semanal = 2, 
        Semestral = 3,
        Anual = 4,
    }
    
}
