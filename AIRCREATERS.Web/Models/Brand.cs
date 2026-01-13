using System.ComponentModel.DataAnnotations;

namespace AIRCREATERS.Web.Models
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Establish Year")]

        public int Establishyear { get; set; }

        [Display(Name = "Brand Logo")]

        public string BrandLogo { get; set; }
    }
}
