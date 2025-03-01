using System.Text.Json.Serialization;

namespace adres.Models;

public class Historico
    {
        public Guid Id { get; set; }
        public Guid AdquisicionId { get; set; }
        public TipoCambio TipoCambio { get; set; }
        public string DetalleCambio { get; set; }
        private DateTimeOffset _FechaCreacion;
        public DateTimeOffset FechaCreacion
        {
            get => _FechaCreacion;
            set => _FechaCreacion = value.ToUniversalTime();
        }
        [JsonIgnore]
        public virtual Adquisicion Adquisicion { get; set; }
    }

public enum TipoCambio{
    Guardar,
    Actualizar,
    Inactivar,
    Activar
}