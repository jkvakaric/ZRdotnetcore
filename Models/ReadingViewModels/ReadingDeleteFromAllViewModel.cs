using System;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models.ReadingViewModels
{
    public class ReadingDeleteFromAllViewModel
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
    }
}
