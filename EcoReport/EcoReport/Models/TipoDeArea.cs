using System.ComponentModel.DataAnnotations;

namespace EcoReport.Models
{
    public class TipoDeArea
    {
        [Key]
        public int Id { get; set; }

        public string Classificacao { get; set; }
    }
}
