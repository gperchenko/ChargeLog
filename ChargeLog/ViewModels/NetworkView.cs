using ChargeLog.Models;
using System.ComponentModel.DataAnnotations;

namespace ChargeLog.ViewModels
{
    public class NetworkView
    {
        [Required]
        [StringLength(150, ErrorMessage = "Name is too long.")]
        [MinLength(3, ErrorMessage = "Name is too short")]
        public string? Name { get; set; }
        public string? Rate { get; set; } = "0";
        public bool HaveAccount { get; set; } = false;       
        public bool IsPartner { get; set; } = false;
        public ChargeType DefaultChargeType { get; set; }
    }
}
