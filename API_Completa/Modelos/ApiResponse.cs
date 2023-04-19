using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace API_Completa.Modelos
{
    public class ApiResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsExitoso { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Resultado { get; set; }
    }
}
