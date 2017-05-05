using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models.ManageViewModels
{
    public class ChangeInfoViewModel
    {
        [StringLength(70)]
        [RegularExpression(@"^[\sa-zA-Z0-9]*$", ErrorMessage = "The field must contain only letters, numbers and spaces.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }
}