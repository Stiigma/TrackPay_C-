using TrackPay.DataStructures;

namespace TrackPay.Models
{
    public class Usuario
    {

        public int Id { get; set; }

        public required string UserName { get; set; }
        public required string Nombre_com { get; set; }
        public required string CorreoElectronico { get; set; }
        public required string Numero { get; set; }

        public required string Contrasena { get; set; }

        public required string Nacionalidad { get; set; }

        public required DateTime FechaNa { get; set; }


        public List<Pago> Pagos { get; set; } = new List<Pago>();

    }
}
