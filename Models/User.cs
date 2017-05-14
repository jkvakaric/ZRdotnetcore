using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models
{
    public class User
    {
        public string UserId { get; set; }

        [StringLength(70)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string FullName { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string Username { get; set; }

        [StringLength(70)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual List<Device> Devices { get; set; }

        public List<Reading> Readings { get; set; }

        public List<ActiveReading> ActiveReadings { get; set; }
    }
}