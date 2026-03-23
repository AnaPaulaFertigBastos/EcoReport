using System.ComponentModel.DataAnnotations;

namespace EcoReport.Models.DTOs
{
    public class PontoRequestDTO
    {
        public string? OutraClassificacao { get; set; }
        public List<int>? Tipos { get; set; }
        public string Descricao { get; set; }

        [Required]
        public double Lat { get; set; }

        [Required]
        public double Lon { get; set; }
        public IFormFile? Arquivo { get; set; }


    }
}
