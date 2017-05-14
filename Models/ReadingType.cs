using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models
{
    public class ReadingType
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public List<Reading> Readings { get; set; }

        public List<ActiveReading> ActiveReadings { get; set; }
    }
}
