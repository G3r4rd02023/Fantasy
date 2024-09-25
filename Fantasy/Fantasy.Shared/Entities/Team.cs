using Fantasy.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace Fantasy.Shared.Entities
{
    public class Team
    {
        public int Id { get; set; }

        [Display(Name = "Team", ResourceType = typeof(Literals))]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
        public string Name { get; set; } = null!;

        public string? Image { get; set; }

        public Country Country { get; set; } = null!;

        public int CountryId { get; set; }
    }
}