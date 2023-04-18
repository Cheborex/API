using System.ComponentModel.DataAnnotations;

namespace API_Completa.Modelos.Dto
{
    public class ApiDto
    {
        public int Id { get; set; }

        [Required] // Para que el nombre sea si o si requerido
        [MaxLength(30)] // Para que el nombre tenga un maximo de 30 caracteres
        public string Nombre { get; set; }
        public string Detalle { get; set; }

        [Required] // Para que la Tarifa sea si o si requida
        public double Tarifa { get; set; }
        public int Ocupantes { get; set; }
        public int MetrosCuadrados { get; set; }
        public string ImagenUrl { get; set; }
        public string Amenidad { get; set; }
    }
}
