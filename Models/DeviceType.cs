using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models
{
    public class DeviceType
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string Name { get; set; }

        [StringLength(1000)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string Description { get; set; }

        public virtual List<Device> Devices { get; set; }
    }
}
