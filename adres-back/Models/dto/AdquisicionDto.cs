using System.ComponentModel.DataAnnotations;

namespace adres.Models.Dto;

public class AdquisicionDto
    {
        public Guid Id { get; set; }
        public required decimal Presupuesto { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s\u00C0-\u00FF]*$", ErrorMessage = "La unidad debe contener solamente letras y números")]
        public required string Unidad { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s\u00C0-\u00FF]*$", ErrorMessage = "El tipo bien o servicio debe contener solamente letras y números")]
        public required string TipoBienServicio { get; set; }
        public required long Cantidad { get; set; }
        public required decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public required DateTime Fecha { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-\u00C0-\u00FF]+$", ErrorMessage = "El Proveedor debe contener solamente letras, números, puntos o guiones")]
        public required string Proveedor { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-\,\u00C0-\u00FF]+$", ErrorMessage = "La Documentacion debe contener solamente letras, números, puntos, comas o guiones")]
        public required string Documentacion { get; set; }        
    }