using System.ComponentModel.DataAnnotations.Schema;

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


        public EstadoPago Estado { get; set; }
        public TipoPago Tipo { get; set; }

        public required string? RutaImagen { get; set; }
        public required string? Notas { get; set; }

        public required Usuario? Usuario { get; set; }

        public void cambiarEstado(EstadoPago nuevoEstado)
        {
            Estado = nuevoEstado;
        }

        public DateTime RenovarFecha()
        {
            if (Frecuencia == TipoFrecuencia.Diario)
                return FechaVencimiento.AddDays(1);
            else if (Frecuencia == TipoFrecuencia.Semanal)
                return FechaVencimiento.AddDays(7);
            else if (Frecuencia == TipoFrecuencia.Mensual)
                return FechaVencimiento.AddMonths(1);
            else if (Frecuencia == TipoFrecuencia.Anual)
                return FechaVencimiento.AddYears(1);
            else
                throw new InvalidOperationException("Frecuencia no válida para un pago recurrente.");
        }

        public Pago ProcesarRecurrente()
        {
            if (!EsRecurrente)
                throw new InvalidOperationException("Este método solo aplica a pagos recurrentes.");

            // Marcar el pago actual como pagado
            Estado = EstadoPago.Pagado;

            // Crear un nuevo pago con la fecha renovada
            return new Pago
            {
                UsuarioId = UsuarioId,
                Concepto = Concepto,
                Monto = Monto,
                FechaVencimiento = RenovarFecha(),
                EsRecurrente = true,
                Frecuencia = Frecuencia,
                Prioridad = Prioridad,
                Estado = EstadoPago.Pendiente,
                Tipo = Tipo,
                RutaImagen = RutaImagen,
                Notas = Notas,
                Usuario = Usuario
            };
        }

        public void ProcesarUnico()
        {
            if (EsRecurrente)
                throw new InvalidOperationException("Este método solo aplica a pagos únicos.");

            Estado = EstadoPago.Pagado;

        }


        public Pago? VerificarYProcesar()
        {
            if (FechaVencimiento <= DateTime.Now)
            {
                if (EsRecurrente)
                {
                    return ProcesarRecurrente(); // Retorna un nuevo pago para ser agregado a la base de datos
                }
                else
                {
                    ProcesarUnico(); // Marca el pago único como pagado
                    return null; // No hay nuevo pago que agregar
                }
            }
            return null; // Si no está vencido, no hacer nada
        }
    }
}
