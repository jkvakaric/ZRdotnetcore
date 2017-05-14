using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models.DeviceViewModels
{
    public class DeviceDeleteViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters and numbers.")]
        public string Hostname { get; set; }
    }
}