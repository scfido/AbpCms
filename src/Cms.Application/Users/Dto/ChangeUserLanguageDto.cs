using System.ComponentModel.DataAnnotations;

namespace Cms.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}