namespace ReBook.Utilities
{
    public enum Language
    {
        Unknown, Abkhaz, Afar, Afrikaans, Akan, Albanian, Amharic, Arabic, Aragonese, Armenian, Assamese, Avaric, Avestan, Aymara, Azerbaijani, Bambara, Bashkir, Basque, Belarusian, Bengali, Bihari, Bislama, Bosnian, Breton, Bulgarian, Burmese, Catalan, Chamorro, Chechen, Chichewa, Chinese, Chuvash, Cornish, Corsican, Cree, Croatian, Czech, Danish, Dutch, Dzongkha, English, Esperanto, Estonian, Ewe, Faroese, Fijian, Finnish, French, Galician, Georgian, German, Greek, Modern, GuaranÃ­, Gujarati, Haitian, Hausa, Hebrew, Herero, Hindi, HiriMotu, Hungarian, Interlingua, Indonesian, Interlingue, Irish, Igbo, Inupiaq, Ido, Icelandic, Italian, Inuktitut, Japanese, Javanese, Kalaallisut, Greenlandic, Kannada, Kanuri, Kashmiri, Kazakh, Khmer, Kikuyu, Gikuyu, Kinyarwanda, Kyrgyz, Komi, Kongo, Korean, Kurdish, Kwanyama, Kuanyama, Latin, Luxembourgish, Letzeburgesch, Ganda, Limburgish, Limburgan, Limburger, Lingala, Lao, Lithuanian, Latvian, Manx, Macedonian, Malagasy, Malay, Malayalam, Maltese, MÄori, Marathi, Marshallese, Mongolian, Nauru, Navajo, Navaho, NorthNdebele, Nepali, Ndonga, NorwegianNynorsk, Norwegian, Nuosu, SouthNdebele, Occitan, Ojibwe, Ojibwa, OldChurchSlavonic, ChurchSlavic, ChurchSlavonic, OldBulgarian, OldSlavonic, Oromo, Oriya, Ossetian, Ossetic, Panjabi, Punjabi, PAli, Persian, Polish, Pashto, Pushto, Portuguese, Quechua, Romansh, Kirundi, Romanian, Russian, Sanskrit, Sardinian, Sindhi, NorthernSami, Samoan, Sango, Serbian, ScottishGaelic, Shona, Sinhala, Sinhalese, Slovak, Slovene, Somali, SouthernSotho, Spanish, Sundanese, Swahili, Swati, Swedish, Tamil, Telugu, Tajik, Thai, Tigrinya, TibetanStandard, Tibetan, Central, Turkmen, Tagalog, Tswana, Tonga, Turkish, Tsonga, Tatar, Twi, Tahitian, Uyghur, Uighur, Ukrainian, Urdu, Uzbek, Venda, VietNamese, Volap, Walloon, Welsh, Wolof, WesternFrisian, Xhosa, Yiddish, Yoruba, Zhuang, Chuang, Zulu
    }

    public static class Languages
    {
        private static string json = "[{'Code':'un','Name':'Unknown'},{'Code':'ab','Name':'Abkhaz'},{'Code':'aa','Name':'Afar'},{'Code':'af','Name':'Afrikaans'},{'Code':'ak','Name':'Akan'},{'Code':'sq','Name':'Albanian'},{'Code':'am','Name':'Amharic'},{'Code':'ar','Name':'Arabic'},{'Code':'an','Name':'Aragonese'},{'Code':'hy','Name':'Armenian'},{'Code':'as','Name':'Assamese'},{'Code':'av','Name':'Avaric'},{'Code':'ae','Name':'Avestan'},{'Code':'ay','Name':'Aymara'},{'Code':'az','Name':'Azerbaijani'},{'Code':'bm','Name':'Bambara'},{'Code':'ba','Name':'Bashkir'},{'Code':'eu','Name':'Basque'},{'Code':'be','Name':'Belarusian'},{'Code':'bn','Name':'Bengali'},{'Code':'bh','Name':'Bihari'},{'Code':'bi','Name':'Bislama'},{'Code':'bs','Name':'Bosnian'},{'Code':'br','Name':'Breton'},{'Code':'bg','Name':'Bulgarian'},{'Code':'my','Name':'Burmese'},{'Code':'ca','Name':'Catalan'},{'Code':'ch','Name':'Chamorro'},{'Code':'ce','Name':'Chechen'},{'Code':'ny','Name':'Chichewa;'},{'Code':'zh','Name':'Chinese'},{'Code':'cv','Name':'Chuvash'},{'Code':'kw','Name':'Cornish'},{'Code':'co','Name':'Corsican'},{'Code':'cr','Name':'Cree'},{'Code':'hr','Name':'Croatian'},{'Code':'cs','Name':'Czech'},{'Code':'da','Name':'Danish'},{'Code':'nl','Name':'Dutch'},{'Code':'dz','Name':'Dzongkha'},{'Code':'en','Name':'English'},{'Code':'eo','Name':'Esperanto'},{'Code':'et','Name':'Estonian'},{'Code':'ee','Name':'Ewe'},{'Code':'fo','Name':'Faroese'},{'Code':'fj','Name':'Fijian'},{'Code':'fi','Name':'Finnish'},{'Code':'fr','Name':'French'},{'Code':'gl','Name':'Galician'},{'Code':'ka','Name':'Georgian'},{'Code':'de','Name':'German'},{'Code':'el','Name':'Greek,Modern'},{'Code':'gn','Name':'GuaranÃ­'},{'Code':'gu','Name':'Gujarati'},{'Code':'ht','Name':'Haitian'},{'Code':'ha','Name':'Hausa'},{'Code':'he','Name':'Hebrew'},{'Code':'hz','Name':'Herero'},{'Code':'hi','Name':'Hindi'},{'Code':'ho','Name':'HiriMotu'},{'Code':'hu','Name':'Hungarian'},{'Code':'ia','Name':'Interlingua'},{'Code':'id','Name':'Indonesian'},{'Code':'ie','Name':'Interlingue'},{'Code':'ga','Name':'Irish'},{'Code':'ig','Name':'Igbo'},{'Code':'ik','Name':'Inupiaq'},{'Code':'io','Name':'Ido'},{'Code':'is','Name':'Icelandic'},{'Code':'it','Name':'Italian'},{'Code':'iu','Name':'Inuktitut'},{'Code':'ja','Name':'Japanese'},{'Code':'jv','Name':'Javanese'},{'Code':'kl','Name':'Kalaallisut,Greenlandic'},{'Code':'kn','Name':'Kannada'},{'Code':'kr','Name':'Kanuri'},{'Code':'ks','Name':'Kashmiri'},{'Code':'kk','Name':'Kazakh'},{'Code':'km','Name':'Khmer'},{'Code':'ki','Name':'Kikuyu,Gikuyu'},{'Code':'rw','Name':'Kinyarwanda'},{'Code':'ky','Name':'Kyrgyz'},{'Code':'kv','Name':'Komi'},{'Code':'kg','Name':'Kongo'},{'Code':'ko','Name':'Korean'},{'Code':'ku','Name':'Kurdish'},{'Code':'kj','Name':'Kwanyama,Kuanyama'},{'Code':'la','Name':'Latin'},{'Code':'lb','Name':'Luxembourgish,Letzeburgesch'},{'Code':'lg','Name':'Ganda'},{'Code':'li','Name':'Limburgish,Limburgan,Limburger'},{'Code':'ln','Name':'Lingala'},{'Code':'lo','Name':'Lao'},{'Code':'lt','Name':'Lithuanian'},{'Code':'lv','Name':'Latvian'},{'Code':'gv','Name':'Manx'},{'Code':'mk','Name':'Macedonian'},{'Code':'mg','Name':'Malagasy'},{'Code':'ms','Name':'Malay'},{'Code':'ml','Name':'Malayalam'},{'Code':'mt','Name':'Maltese'},{'Code':'mi','Name':'MÄori'},{'Code':'mr','Name':'Marathi'},{'Code':'mh','Name':'Marshallese'},{'Code':'mn','Name':'Mongolian'},{'Code':'na','Name':'Nauru'},{'Code':'nv','Name':'Navajo,Navaho'},{'Code':'nb','Name':'Norwegian'},{'Code':'nd','Name':'NorthNdebele'},{'Code':'ne','Name':'Nepali'},{'Code':'ng','Name':'Ndonga'},{'Code':'nn','Name':'NorwegianNynorsk'},{'Code':'no','Name':'Norwegian'},{'Code':'ii','Name':'Nuosu'},{'Code':'nr','Name':'SouthNdebele'},{'Code':'oc','Name':'Occitan'},{'Code':'oj','Name':'Ojibwe,Ojibwa'},{'Code':'cu','Name':'OldChurchSlavonic,ChurchSlavic,ChurchSlavonic,OldBulgarian,OldSlavonic'},{'Code':'om','Name':'Oromo'},{'Code':'or','Name':'Oriya'},{'Code':'os','Name':'Ossetian,Ossetic'},{'Code':'pa','Name':'Panjabi,Punjabi'},{'Code':'pi','Name':'PAli'},{'Code':'fa','Name':'Persian'},{'Code':'pl','Name':'Polish'},{'Code':'ps','Name':'Pashto,Pushto'},{'Code':'pt','Name':'Portuguese'},{'Code':'qu','Name':'Quechua'},{'Code':'rm','Name':'Romansh'},{'Code':'rn','Name':'Kirundi'},{'Code':'ro','Name':'Romanian'},{'Code':'ru','Name':'Russian'},{'Code':'sa','Name':'Sanskrit'},{'Code':'sc','Name':'Sardinian'},{'Code':'sd','Name':'Sindhi'},{'Code':'se','Name':'NorthernSami'},{'Code':'sm','Name':'Samoan'},{'Code':'sg','Name':'Sango'},{'Code':'sr','Name':'Serbian'},{'Code':'gd','Name':'ScottishGaelic'},{'Code':'sn','Name':'Shona'},{'Code':'si','Name':'Sinhala,Sinhalese'},{'Code':'sk','Name':'Slovak'},{'Code':'sl','Name':'Slovene'},{'Code':'so','Name':'Somali'},{'Code':'st','Name':'SouthernSotho'},{'Code':'es','Name':'Spanish'},{'Code':'su','Name':'Sundanese'},{'Code':'sw','Name':'Swahili'},{'Code':'ss','Name':'Swati'},{'Code':'sv','Name':'Swedish'},{'Code':'ta','Name':'Tamil'},{'Code':'te','Name':'Telugu'},{'Code':'tg','Name':'Tajik'},{'Code':'th','Name':'Thai'},{'Code':'ti','Name':'Tigrinya'},{'Code':'bo','Name':'TibetanStandard,Tibetan,Central'},{'Code':'tk','Name':'Turkmen'},{'Code':'tl','Name':'Tagalog'},{'Code':'tn','Name':'Tswana'},{'Code':'to','Name':'Tonga'},{'Code':'tr','Name':'Turkish'},{'Code':'ts','Name':'Tsonga'},{'Code':'tt','Name':'Tatar'},{'Code':'tw','Name':'Twi'},{'Code':'ty','Name':'Tahitian'},{'Code':'ug','Name':'Uyghur,Uighur'},{'Code':'uk','Name':'Ukrainian'},{'Code':'ur','Name':'Urdu'},{'Code':'uz','Name':'Uzbek'},{'Code':'ve','Name':'Venda'},{'Code':'vi','Name':'VietNamese'},{'Code':'vo','Name':'Volap'},{'Code':'wa','Name':'Walloon'},{'Code':'cy','Name':'Welsh'},{'Code':'wo','Name':'Wolof'},{'Code':'fy','Name':'WesternFrisian'},{'Code':'xh','Name':'Xhosa'},{'Code':'yi','Name':'Yiddish'},{'Code':'yo','Name':'Yoruba'},{'Code':'za','Name':'Zhuang,Chuang'},{'Code':'zu','Name':'Zulu'}]";

        public static readonly List<IsoLanguage> List = JsonConvert.DeserializeObject<List<IsoLanguage>>(json);
        public static readonly List<string> Names = List.Select(l => l.Name).ToList();
        public static readonly List<string> Codes = List.Select(l => l.Code).ToList();

        public static Language GetLanguageFromCode(string code)
        {
            if (!String.IsNullOrEmpty(code))
            {
                var result = List.Where(c => c.Code == code.ToLower()).FirstOrDefault();

                if (result == null) return Language.Unknown;

                return (Language)System.Enum.Parse(typeof(Language), result.Name);
            }
            return Language.Unknown;
        }
        public static string GetNameFromCode(string code)
        {
            if (!String.IsNullOrEmpty(code))
            {
                var result = List.Where(c => c.Code.ToLower() == code.ToLower()).FirstOrDefault();

                if (result == null) return "Unknown";

                return result.Name;
            }
            return "Unknown";
        }

        public static string GetCodeFromName(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var result = List.Where(c => c.Name.ToLower() == name.ToLower()).FirstOrDefault();

                if (result == null) return "un";

                return result.Code;
            }
            return "un";
        }

        public class IsoLanguage
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }
    }
}
