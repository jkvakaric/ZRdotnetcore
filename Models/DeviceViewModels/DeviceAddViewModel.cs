using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models.DeviceViewModels
{
    public class DeviceAddViewModel
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters and numbers.")]
        public string Hostname { get; set; }

        [Required]
        [Display(Name = "Device Type")]
        public string DeviceType { get; set; }

        public List<DeviceType> DeviceTypeList { get; set; }
    }
}