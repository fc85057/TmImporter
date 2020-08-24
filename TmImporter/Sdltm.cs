using System.Collections.Generic;
using Sdl.LanguagePlatform.TranslationMemory;
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

        public IList<TmField> Fields
        {
            get
            {
                IList<TmField> tmFields = new List<TmField>();
                foreach (FieldDefinition fieldDefinition in tm.FieldDefinitions)
                {
                    if (fieldDefinition.ValueType == FieldValueType.SinglePicklist || fieldDefinition.ValueType == FieldValueType.MultiplePicklist)
                    {
                        List<string> picklistValues = new List<string>();
                        foreach(var picklistItem in fieldDefinition.PicklistItemNames)
                        {
                            picklistValues.Add(picklistItem);
                        }
                        TmField tmField = new TmField(fieldDefinition.Name, true, picklistValues);
                        tmFields.Add(tmField);
                    }
                    else if (fieldDefinition.ValueType == FieldValueType.SingleString || fieldDefinition.ValueType == FieldValueType.MultipleString)
                    {
                        TmField tmField = new TmField(fieldDefinition.Name, false, null);
                        tmFields.Add(tmField);
                    }
                }

                return tmFields;
                /*
                IList<string> fieldDefinitions = new List<string>();
                foreach (FieldDefinition fieldDefinition in tm.FieldDefinitions)
                {
                    fieldDefinitions.Add(fieldDefinition.Name);
                }
                return fieldDefinitions;
                */
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
