using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
