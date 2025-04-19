using System.ComponentModel.DataAnnotations;

namespace Ejercicio1_CRUD_Clientes.Models
{
    public class Cliente
    {
        [Key]
        [Required(ErrorMessage = "El CUIT no puede estar vacio.")]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "El CUIT debe tener exactamente 11 números.")]
        public string Cuit { get; set; }

        [Required(ErrorMessage = "La razon social no puede estar vacia.")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "El telefono no puede estar vacio.")]
        public long Telefono { get; set; }

        [Required(ErrorMessage = "La direccion no puede estar vacia.")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El estado no puede estar vacio.")]
        public bool Activo { get; set; }
    }
}
