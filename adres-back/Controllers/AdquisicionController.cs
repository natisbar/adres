using adres.Models;
using adres.Models.Dto;
using adres.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace adres.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdquisicionController : ControllerBase
{
    private readonly ILogger<AdquisicionController> _logger;
    private readonly AdquisicionesContext _dbContext;
    private readonly IMapper _mapper;
    public const string MsgNoExisteAdquisicion = "No existe la adquisición";
    public const string MsgSinCambios = "No se han realizado cambios";
    public const string MsgValorTotalSuperaPresupuesto = "El valor total no puede superar el presupuesto.";

    public AdquisicionController(IMapper mapper, ILogger<AdquisicionController> logger, AdquisicionesContext dbContext)
    {
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
    }   

    [HttpGet(Name = "GetAllAdquisicion")]
    public IResult Listar()
    {
        List<Adquisicion> adquisicionesActivas = _dbContext.Adquisiciones.Where(a => a.Activo).ToList();
        List<AdquisicionDto> adquisicionDtoList = adquisicionesActivas.Select(_mapper.Map<AdquisicionDto>).ToList();
        return Results.Ok(adquisicionDtoList);
    }

    [HttpPost("filtro", Name = "GetAllAdquisicionFiltro")]
    public IResult ListarFiltro([FromBody] FiltroDto filtroDto)
    {
        List<Adquisicion>? adquisicionesActivas = null;
        List<AdquisicionDto>? adquisicionDtoList = null;
        if (filtroDto == null)
        {
            adquisicionesActivas = _dbContext.Adquisiciones.Where(a => a.Activo).ToList();
        }
        else
        {
            string error = filtroDto.Validate();
            if (!string.IsNullOrEmpty(error))
            {
                return Results.BadRequest(error);
            }
            var query = ObtenerPosiblesResultadosFiltro(filtroDto);
            adquisicionDtoList = query.Select(_mapper.Map<AdquisicionDto>).ToList();
        }
        return Results.Ok(adquisicionDtoList);
    }


    [HttpPost(Name = "SaveAdquisicion")]
    public IResult Guardar([FromBody] AdquisicionDto adquisicionDto)
    {
        Adquisicion adquisicion = _mapper.Map<Adquisicion>(adquisicionDto);
        adquisicion.Id = Guid.NewGuid();
        adquisicion.Activo = true;  
        adquisicion.ValorTotal = adquisicion.Cantidad * adquisicion.ValorUnitario;
        if (adquisicion.ValorTotal > adquisicion.Presupuesto){
            return Results.BadRequest(MsgValorTotalSuperaPresupuesto);
        }
        _dbContext.Adquisiciones.Add(adquisicion);
        _dbContext.SaveChanges();
        SaveHistorico(TipoCambio.Guardar, "Se guarda adquisición", adquisicion);
        return Results.Ok();
    }


    [HttpPut("{id}")]
    public IResult Update([FromBody] AdquisicionDto adquisicionDto, [FromRoute] Guid id)
    {
        var adquisicionActual = _dbContext.Adquisiciones.Where(a => a.Id == id && a.Activo).FirstOrDefault();
        if (adquisicionActual == null)
        {
            return Results.NotFound(MsgNoExisteAdquisicion);
        } 

        Adquisicion adquisicion = _mapper.Map<Adquisicion>(adquisicionDto);
        List<string> cambiosList = ValidarCambios(adquisicionActual, adquisicion);
        cambiosList.ForEach(Console.WriteLine);
        if (cambiosList.Count != 0)
        {
            string detalleCambio = string.Join(", ", cambiosList);
            SaveHistorico(TipoCambio.Actualizar, detalleCambio, adquisicionActual);
        }
        else {
            return Results.BadRequest(MsgSinCambios);
        }

        adquisicionActual.Presupuesto = adquisicion.Presupuesto;
        adquisicionActual.Unidad = adquisicion.Unidad;
        adquisicionActual.TipoBienServicio = adquisicion.TipoBienServicio;
        adquisicionActual.Cantidad = adquisicion.Cantidad;
        adquisicionActual.ValorUnitario = adquisicion.ValorUnitario;
        adquisicionActual.ValorTotal = adquisicion.Cantidad * adquisicion.ValorUnitario;
        adquisicionActual.Fecha = adquisicion.Fecha;
        adquisicionActual.Proveedor = adquisicion.Proveedor;
        adquisicionActual.Documentacion = adquisicion.Documentacion;
        if (adquisicionActual.ValorTotal > adquisicionActual.Presupuesto){
            return Results.BadRequest(MsgValorTotalSuperaPresupuesto);
        }
        _dbContext.SaveChanges();
        return Results.Ok();
    }

    [HttpDelete("{id}")]
    public IResult Delete([FromRoute] Guid id)
    {
        var adquisicionActual = _dbContext.Adquisiciones.Where(a => a.Id == id && a.Activo).FirstOrDefault();
        if (adquisicionActual == null)
        {
            return Results.NotFound(MsgNoExisteAdquisicion);
        } else {
            adquisicionActual.Activo = false;
            _dbContext.SaveChanges();
            SaveHistorico(TipoCambio.Inactivar, "Se inactiva adquisición", adquisicionActual);
        }
        return Results.Ok();
    }


    private IQueryable<Adquisicion> ObtenerPosiblesResultadosFiltro(FiltroDto filtroDto){
        var query = _dbContext.Adquisiciones.AsQueryable();
            if (filtroDto.FechaDesde == null && filtroDto.FechaHasta == null){
                if (filtroDto.Presupuesto != null && filtroDto.ValorTotal != null)
                {
                    query = query.Where(a => a.Presupuesto <= filtroDto.Presupuesto.Value && a.ValorTotal <= filtroDto.ValorTotal.Value
                        && a.Unidad.Contains(filtroDto.Unidad) && a.Activo);
                }
                else if (filtroDto.Presupuesto != null && filtroDto.ValorTotal == null )
                {
                    query = query.Where(a => a.Presupuesto <= filtroDto.Presupuesto.Value && a.Unidad.Contains(filtroDto.Unidad) && a.Activo);
                }
                else if (filtroDto.Presupuesto == null && filtroDto.ValorTotal != null)
                {
                    query = query.Where(a => a.ValorTotal <= filtroDto.ValorTotal.Value && a.Unidad.Contains(filtroDto.Unidad) && a.Activo);
                }
                else
                {
                    query = query.Where(a => a.Unidad.Contains(filtroDto.Unidad) && a.Activo);
                }
            }
            else if (filtroDto.FechaDesde != null && filtroDto.FechaHasta != null){
                if (filtroDto.Presupuesto != null && filtroDto.ValorTotal != null)
                {
                    query = query.Where(a => a.Presupuesto <= filtroDto.Presupuesto.Value && a.ValorTotal <= filtroDto.ValorTotal.Value
                        && a.Unidad.Contains(filtroDto.Unidad) && a.Fecha >= filtroDto.FechaDesde && a.Fecha <= filtroDto.FechaHasta && a.Activo);
                }
                else if (filtroDto.Presupuesto != null && filtroDto.ValorTotal == null)
                {
                    query = query.Where(a => a.Presupuesto <= filtroDto.Presupuesto.Value && a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha >= filtroDto.FechaDesde && a.Fecha <= filtroDto.FechaHasta && a.Activo);
                }
                else if (filtroDto.Presupuesto == null && filtroDto.ValorTotal != null)
                {
                    query = query.Where(a => a.ValorTotal <= filtroDto.ValorTotal.Value && a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha >= filtroDto.FechaDesde && a.Fecha <= filtroDto.FechaHasta && a.Activo);
                }
                else
                {
                    query = query.Where(a => a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha >= filtroDto.FechaDesde && a.Fecha <= filtroDto.FechaHasta && a.Activo);
                }
            }
            else if (filtroDto.FechaDesde != null && filtroDto.FechaHasta == null){
                if (filtroDto.Presupuesto != null && filtroDto.ValorTotal != null)
                {
                    query = query.Where(a => a.Presupuesto <= filtroDto.Presupuesto.Value && a.ValorTotal <= filtroDto.ValorTotal.Value
                        && a.Unidad.Contains(filtroDto.Unidad) && a.Fecha >= filtroDto.FechaDesde && a.Activo);
                }
                else if (filtroDto.Presupuesto != null && filtroDto.ValorTotal == null)
                {
                    query = query.Where(a => a.Presupuesto <= filtroDto.Presupuesto.Value && a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha >= filtroDto.FechaDesde && a.Activo);
                }
                else if (filtroDto.Presupuesto == null && filtroDto.ValorTotal != null)
                {
                    query = query.Where(a => a.ValorTotal <= filtroDto.ValorTotal.Value && a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha >= filtroDto.FechaDesde && a.Activo);
                }
                else
                {
                    query = query.Where(a => a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha >= filtroDto.FechaDesde && a.Activo);
                }
            }
            else if (filtroDto.FechaDesde == null && filtroDto.FechaHasta != null){
                if (filtroDto.Presupuesto != null && filtroDto.ValorTotal != null)
                {
                    query = query.Where(a => a.Presupuesto <= filtroDto.Presupuesto.Value && a.ValorTotal <= filtroDto.ValorTotal.Value
                        && a.Unidad.Contains(filtroDto.Unidad) && a.Fecha <= filtroDto.FechaHasta && a.Activo);
                }
                else if (filtroDto.Presupuesto != null && filtroDto.ValorTotal == null)
                {
                    query = query.Where(a => a.Presupuesto <= filtroDto.Presupuesto.Value && a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha <= filtroDto.FechaHasta && a.Activo);
                }
                else if (filtroDto.Presupuesto == null && filtroDto.ValorTotal != null)
                {
                    query = query.Where(a => a.ValorTotal <= filtroDto.ValorTotal.Value && a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha <= filtroDto.FechaHasta && a.Activo);
                }
                else
                {
                    query = query.Where(a => a.Unidad.Contains(filtroDto.Unidad)
                        && a.Fecha <= filtroDto.FechaHasta && a.Activo);
                }
            }

           return query;
    }

    private List<string> ValidarCambios(Adquisicion adquisicionActual, Adquisicion adquisicion){
        var cambios = new List<string>();

        if (adquisicionActual.Presupuesto != adquisicion.Presupuesto)
            cambios.Add($"Presupuesto cambiado de {adquisicionActual.Presupuesto} a {adquisicion.Presupuesto}");
        
        if (adquisicionActual.Unidad != adquisicion.Unidad)
            cambios.Add($"Unidad cambiada de {adquisicionActual.Unidad} a {adquisicion.Unidad}");
        
        if (adquisicionActual.TipoBienServicio != adquisicion.TipoBienServicio)
            cambios.Add($"TipoBienServicio cambiado de {adquisicionActual.TipoBienServicio} a {adquisicion.TipoBienServicio}");
        
        if (adquisicionActual.Cantidad != adquisicion.Cantidad)
            cambios.Add($"Cantidad cambiada de {adquisicionActual.Cantidad} a {adquisicion.Cantidad}");
        
        if (adquisicionActual.ValorUnitario != adquisicion.ValorUnitario)
            cambios.Add($"ValorUnitario cambiado de {adquisicionActual.ValorUnitario} a {adquisicion.ValorUnitario}");
        
        if (adquisicionActual.ValorTotal != adquisicion.Cantidad * adquisicion.ValorUnitario)
            cambios.Add($"ValorTotal recalculado de {adquisicionActual.ValorTotal} a {adquisicion.Cantidad * adquisicion.ValorUnitario}");
        
        if (adquisicionActual.Fecha != adquisicion.Fecha)
            cambios.Add($"Fecha cambiada de {adquisicionActual.Fecha} a {adquisicion.Fecha}");
        
        if (adquisicionActual.Proveedor != adquisicion.Proveedor)
            cambios.Add($"Proveedor cambiado de {adquisicionActual.Proveedor} a {adquisicion.Proveedor}");
        
        if (adquisicionActual.Documentacion != adquisicion.Documentacion)
            cambios.Add($"Documentación cambiada de {adquisicionActual.Documentacion} a {adquisicion.Documentacion}");
        
        return cambios;
    }


    private void SaveHistorico(TipoCambio tipoCambio, string detalleCambio, Adquisicion adquisicion)
    {
        var historico = new Historico
        {
            Id = Guid.NewGuid(),
            AdquisicionId = adquisicion.Id,
            FechaCreacion = DateTimeOffset.Now,
            TipoCambio = tipoCambio,
            DetalleCambio = detalleCambio
        };
        _dbContext.Historicos.Add(historico);
        _dbContext.SaveChanges();

    }
}
