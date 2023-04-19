using API_Completa.Modelos;
using Microsoft.EntityFrameworkCore;

namespace API_Completa.Datos
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base (options) 
        {

        }        
        public DbSet<Api> Apis { get; set; }
        public DbSet<NumeroApi> NumeroApis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Api>().HasData(
                new Api
                {
                    Id= 1,
                    Nombre="Nueva Api",
                    Detalle="Una nueva y magnifica Api",
                    ImagenUrl="",
                    Ocupantes=5,
                    MetrosCuadrados=50,
                    Tarifa=200,
                    Amenidad="",
                    FechaCreacion=DateTime.Now,
                    FechaActualizacion=DateTime.Now
                },
                new Api
                {
                    Id = 2,
                    Nombre = "Nueva Api 2",
                    Detalle = "Una nueva y magnifica Api 2",
                    ImagenUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 50,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }
            );
        }
    }
}
