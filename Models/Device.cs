using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models
{
    public class Device
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters and numbers.")]
        public string Hostname { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AddedOn { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedOn { get; set; }

        public virtual DeviceType DeviceType { get; set; }

        public virtual List<ActiveReading> ActiveReadings { get; set; }

        public List<Reading> Readings { get; set; }

        public virtual User User { get; set; }
    }
}
