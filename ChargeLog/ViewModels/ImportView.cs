using System.ComponentModel.DataAnnotations;

namespace ChargeLog.ViewModels
{
    public class ImportView
    {
        [Required]
        public string? ImportType { get; set; }

        [Required]
        public string? FileName { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}
