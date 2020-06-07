using System.ComponentModel.DataAnnotations;

namespace Emarketing.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}