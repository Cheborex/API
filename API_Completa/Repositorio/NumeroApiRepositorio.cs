using API_Completa.Datos;
using API_Completa.Modelos;
using API_Completa.Repositorio.IRepositorio;

namespace API_Completa.Repositorio
{
    public class NumeroApiRepositorio : Repositorio<NumeroApi>, INumeroApiRepositorio
    {
        private readonly AplicationDbContext _db;
        public NumeroApiRepositorio(AplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<NumeroApi> Actualizar(NumeroApi entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.NumeroApis.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
