using API_Completa.Modelos;

namespace API_Completa.Repositorio.IRepositorio
{
    public interface IApiRepositorio : IRepositorio<Api> 
    {
        Task<Api> Actualizar (Api entidad);
    }
}
