using System.ComponentModel.DataAnnotations;


namespace Chow_Patty_HW3.Models
{
    public class Language
    {
        public Int32 LanguageID { get; set; }

        [Display(Name ="Name")]
        public String LanguageName { get; set; }

        public List<Repository> Repositories { get; set; }
    }
}