using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models
{
    public class ActiveReading
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(70)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string DataFilepath { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ActiveSince { get; set; }

        public virtual Device Device { get; set; }

        public virtual User Owner { get; set; }

        public ReadingType ReadingType { get; set; }

        public List<Reading> Readings { get; set; }
    }
}
