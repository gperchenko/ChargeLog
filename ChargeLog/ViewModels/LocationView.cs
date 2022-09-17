using System.ComponentModel.DataAnnotations;

namespace ChargeLog.ViewModels
{
    public class LocationView
    {
        [Required]
        [StringLength(150, ErrorMessage = "Name is too long.")]
        [MinLength(3, ErrorMessage = "Name is too short")]
        public string? Name { get; set; }

        [StringLength(150, ErrorMessage = "Address is too long.")]
        [MinLength(3, ErrorMessage = "Address is too short")]
        public string? Address { get; set; }    
    }
}
