using System.ComponentModel.DataAnnotations;

namespace EcoReport.Models
{
    public class Ponto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public double Lat { get; set; }

        [Required]
        public double Lon { get; set; }

        public string ArquivoDes { get; set; }
        public string Arquivo { get; set; }

        public bool Ativo { get; set; }

        // Foreign key
        public int TipoDeAreaId { get; set; }

        // Navegacao foreign key

        public TipoDeArea TipoDeArea { get; set; }
    }
}
