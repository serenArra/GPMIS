using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}