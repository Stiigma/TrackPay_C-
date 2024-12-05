using System.ComponentModel.DataAnnotations;

namespace TrackPay.Models
{
    public class UsuarioRegistroVista
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [Display(Name = "Nombre Completo")]
        public string Nombre_com { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El número es obligatorio.")]
        [Phone(ErrorMessage = "Ingrese un número válido.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
        public string Nacionalidad { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaNa { get; set; }
    }
}
