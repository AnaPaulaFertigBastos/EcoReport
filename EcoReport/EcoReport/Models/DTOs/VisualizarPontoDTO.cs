using System.ComponentModel.DataAnnotations;

namespace EcoReport.Models.DTOs
{
    public class VisualizarPontoDTO
    {
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }

        public string? Arquivo { get; set; }

        public bool Ativo { get; set; }
        
        public DateTime Data { get; set; } = DateTime.UtcNow;

        public List<string> Classificacoes { get; set; }
        
    }
}
