﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models
{
    public class Reading
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(70)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        [StringLength(255)]
        public string ReadValue { get; set; }

        public virtual Device Device { get; set; }

        public virtual ReadingType ReadingType { get; set; }

        public virtual ActiveReading ActiveReading { get; set; }

        public virtual User Owner { get; set; }
    }
}
