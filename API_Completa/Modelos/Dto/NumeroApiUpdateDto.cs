using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace API_Completa.Modelos.Dto
{
    public class NumeroApiUpdateDto
    {
        [Required]
        public int ApiNo { get; set; }

        [Required]
        public int ApiId { get; set; }
        public string DetalleEspecial { get; set; }
    }
}
