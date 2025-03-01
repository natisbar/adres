using adres.Models;
using Microsoft.EntityFrameworkCore;

namespace adres.Persistence;

public class AdquisicionesContext : DbContext
{
    public AdquisicionesContext(DbContextOptions<AdquisicionesContext> options)
        : base(options)
    {
    }

    public DbSet<Adquisicion> Adquisiciones { get; set; }
    public DbSet<Historico> Historicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Adquisicion>(adquisicion => {
            adquisicion.ToTable("adquisicion");
            adquisicion.HasKey(a => a.Id);
            adquisicion.Property(a => a.Presupuesto).IsRequired();
            adquisicion.Property(a => a.Fecha)
                .HasConversion(
                    v => v.Date,   // Convierte DateTime a solo la fecha
                    v => v.Date  
                )
                .HasColumnType("date");
        });
        modelBuilder.Entity<Historico>(historico => {
            historico.ToTable("historico");
            historico.HasKey(a => a.Id);
            historico.Property(a => a.AdquisicionId).IsRequired();
            historico.HasOne(a => a.Adquisicion).WithMany().HasForeignKey(a => a.AdquisicionId);
        });
    }

}