using System;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models.ReadingViewModels
{
    public class ReadingDeleteFromAllByNameViewModel
    {
        [Required]
        [StringLength(70)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string Name { get; set; }
    }
}
