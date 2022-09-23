using System.ComponentModel.DataAnnotations;

namespace ChargeLog.ViewModels
{
    public class GroupView
    {
        [Required]
        [StringLength(50, ErrorMessage = "Value is too long.")]
        [MinLength(3, ErrorMessage = "Value is too short")]
        public string? Name { get; set; }
    }
}
