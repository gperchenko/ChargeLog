using System.ComponentModel.DataAnnotations;

namespace ChargeLog.ViewModels
{
    public class ImportView
    {
        [Required]
        public string? ImportType { get; set; }
    }
}
