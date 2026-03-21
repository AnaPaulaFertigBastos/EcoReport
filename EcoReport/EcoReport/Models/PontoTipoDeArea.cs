using System.ComponentModel.DataAnnotations;

namespace EcoReport.Models
{
    public class PontoTipoDeArea
    {
        [Key]
        public int Id { get; set; }

        // Foreign key
        [Required]
        public int PontoId { get; set; }
        public int? TipoDeAreaId { get; set; }
        public string? Descricao { get; set; }

        // Navegacao foreign key

        public TipoDeArea TipoDeArea { get; set; }
        public Ponto Ponto { get; set; }
    }
}
