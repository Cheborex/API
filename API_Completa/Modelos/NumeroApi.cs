using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace API_Completa.Modelos
{
    public class NumeroApi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApiNo { get; set; }
        [Required]
        public int ApiId { get; set; }
        [ForeignKey("ApiId")]
        public Api Api { get; set; }
        public string DetalleEspecial { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }


    }
}
