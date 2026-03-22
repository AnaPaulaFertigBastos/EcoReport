namespace EcoReport.Models.DTOs
{
    public class PontoRequestDTO
    {
        public string? OutraClassificacao { get; set; }
        public List<int>? Tipos { get; set; }
        public string Descricao { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public IFormFile? Arquivo { get; set; }


    }
}
