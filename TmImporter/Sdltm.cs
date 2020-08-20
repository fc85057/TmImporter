using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace TmImporter
{
    class Sdltm
    {
        private FileBasedTranslationMemory tm;

        public FileBasedTranslationMemory TmAccess
        {
            get { return tm; }
        }

        public string SourceLanguage
        {
            get
            {
                return tm.LanguageDirection.SourceLanguage.TwoLetterISOLanguageName;
            }
        }

        public string TargetLanguage
        {
            get
            {
                return tm.LanguageDirection.TargetLanguage.TwoLetterISOLanguageName;
            }
        }

        public string LanguageDirection
        {
            get
            {
                return (SourceLanguage + "-" + TargetLanguage).ToUpper();
            }
        }

        public ITranslationMemoryLanguageDirection ExportSettings
        {
            get
            {
                return tm.LanguageDirection;
            }
        }

        public int TranslationUnits
        {
            get
            {
                return tm.GetTranslationUnitCount();
            }
        }


        public Sdltm(string path)
        {
            // need to load relevant properties from "internal" tm data
            tm = new FileBasedTranslationMemory(path);
        }

    }
}
