using API_Completa.Datos;
using API_Completa.Modelos;
using API_Completa.Repositorio.IRepositorio;

namespace API_Completa.Repositorio
{
    public class ApiRepositorio : Repositorio<Api>, IApiRepositorio
    {
        private readonly AplicationDbContext _db;
        public ApiRepositorio(AplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Api> Actualizar(Api entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.Apis.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
