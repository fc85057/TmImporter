using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace TmImporter
{
    public class Sdltm
    {
        private FileBasedTranslationMemory tm;

        public string Path;

        public FileBasedTranslationMemory TmAccess
        {
            get { return tm; }
        }

        public string Name
        {
            get
            {
                return tm.Name;
            }
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

        public IList<string> Clients
        {
            get
            {
                return tm.FieldDefinitions["Client"].PicklistItemNames;
            }
        }

        public IList<string> Statuses
        {
            get
            {
                return tm.FieldDefinitions["Status"].PicklistItemNames;
            }
        }


        public Sdltm(string path)
        {
            // need to load relevant properties from "internal" tm data
            tm = new FileBasedTranslationMemory(path);
            Path = path;
        }

    }
}
