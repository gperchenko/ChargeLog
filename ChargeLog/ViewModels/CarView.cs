using System.ComponentModel.DataAnnotations;

namespace ChargeLog.ViewModels
{
    public class CarView
    {
        [Required]
        [RegularExpression(@"^20\d\d$", ErrorMessage = "Invalid value.")]
        public string? Year { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Value is too long.")]
        [MinLength(3, ErrorMessage = "Value is too short")]
        public string? Make { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Value is too long.")]
        [MinLength(3, ErrorMessage = "Value is too short")]
        public string? Model { get; set; }
    }
}
