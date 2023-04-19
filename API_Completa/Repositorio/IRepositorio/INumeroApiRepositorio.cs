using API_Completa.Modelos;

namespace API_Completa.Repositorio.IRepositorio
{
    public interface INumeroApiRepositorio : IRepositorio<NumeroApi> 
    {
        Task<NumeroApi> Actualizar (NumeroApi entidad);
    }
}
