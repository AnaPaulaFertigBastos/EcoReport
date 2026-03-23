using System.ComponentModel.DataAnnotations;

namespace EcoReport.Models.DTOs
{
    public class PontoSalvoDTO
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
