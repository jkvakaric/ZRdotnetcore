using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models
{
    public class ActiveReading
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string DataFilepath { get; set; }

        public virtual Device Device { get; set; }
    }
}
