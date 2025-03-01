namespace adres.Models;

public class Adquisicion
    {
        public Guid Id { get; set; }
        public required decimal Presupuesto { get; set; }
        public required string Unidad { get; set; }
        public required string TipoBienServicio { get; set; }
        public required long Cantidad { get; set; }
        public required decimal ValorUnitario { get; set; }
        public required decimal ValorTotal { get; set; }
        public required DateTime Fecha { get; set; }
        public required string Proveedor { get; set; }
        public required string Documentacion { get; set; }
        public bool Activo { get; set; }
        
    }