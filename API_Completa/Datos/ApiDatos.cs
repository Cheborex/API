using API_Completa.Modelos.Dto;

namespace API_Completa.Datos
{
    public static class ApiDatos
    {
        public static List<ApiDto> apiList = new List<ApiDto>
        {
            new ApiDto {Id = 1, Nombre = "Vista a la piscina", Ocupantes = 3, MetrosCuadrados = 500 },
            new ApiDto {Id = 2, Nombre = "Vista a la playa", Ocupantes = 5, MetrosCuadrados = 800 }
        };
    }
}
