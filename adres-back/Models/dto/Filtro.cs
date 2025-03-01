namespace adres.Models.Dto;

public class FiltroDto
{
    public decimal? Presupuesto { get; set; }
    public string? Unidad { get; set; }
    public decimal? ValorTotal { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }


    public string Validate()
    {
        string error = "";
        if (Unidad == null)
        {
            Unidad = "";
        }
        if (Presupuesto <= 0)
        {
            error = "El presupuesto no puede ser 0";
        }
        if (ValorTotal <= 0)
        {
            error = "El valor total no puede ser 0";
        }
        if (FechaDesde != null && FechaHasta != null && FechaDesde > FechaHasta)
        {
            error = "La fecha desde no puede ser mayor a la fecha hasta";
        }
        return error;
    }
}