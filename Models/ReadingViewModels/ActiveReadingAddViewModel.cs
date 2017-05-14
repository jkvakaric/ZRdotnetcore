using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models.ReadingViewModels
{
    public class ActiveReadingAddViewModel
    {
        [Required]
        [StringLength(70)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Data Filepath")]
        public string DataFilepath { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        [Display(Name = "Reading Type")]
        public string ReadingType { get; set; }

        public List<ReadingType> ReadingTypeList { get; set; }
    }
}
