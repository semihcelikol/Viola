using System;
using System.Collections.Generic;
using System.Linq;

namespace Viola.Models
{
    public class Language
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string WebLanguageId { get; set; }
        public bool Default { get; set; }


        public static List<Language> GetList()
        {
            List<Language> ret = new List<Language>();

            ret.Add(new Language {
                Id = "en-US",
                DisplayName = "English",
                WebLanguageId = "en",
                Default = true
            });

            ret.Add(new Language
            {
                Id = "tr-TR",
                DisplayName = "Türkçe",
                WebLanguageId = "tr",
            });

            return ret;
        }

        public static Language GetDefault()
        {
            return Language.GetList().First(x => x.Default = true);
        }
    }
}