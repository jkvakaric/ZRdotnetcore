using System;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models
{
    public class Reading
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        [StringLength(255)]
        public string ReadValue { get; set; }

        public Device Device { get; set; }

        public ReadingType ReadingType { get; set; }
    }
}
