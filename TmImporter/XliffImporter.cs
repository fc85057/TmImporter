using System.Collections.Generic;
using System.Linq;
using Studio.AssemblyResolver;
using Sdl.Core.Globalization;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace TmImporter
{
    public class XliffImporter
    {


        public XliffImporter()
        {
            AssemblyResolver.Resolve();
        }

        public void ImportXliff(Sdltm sdlTm, Xliff[] xliffs, string job, string client, string status, string tp)
        {
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(sdlTm.Path);
            TranslationMemoryImporter importer = new TranslationMemoryImporter(sdlTm.TmAccess.LanguageDirection);
            GetImportSettings(importer.ImportSettings, tm, job, client, status, tp);

            foreach (Xliff xliff in xliffs)
            {
                importer.Import(xliff.Path);
            }

        }

        public void ImportXliff(Sdltm tm, Xliff[] xliffs, TmField[] tmFields)
        {
            TranslationMemoryImporter importer = new TranslationMemoryImporter(tm.TmAccess.LanguageDirection);
            GetImportSettings(importer.ImportSettings, tm, tmFields);

            foreach (Xliff xliff in xliffs)
            {
                importer.Import(xliff.Path);
            }
        }

        private void GetImportSettings(ImportSettings importSettings, Sdltm tm, TmField[] tmFields)
        {
            importSettings.CheckMatchingSublanguages = false;
            importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
            ConfirmationLevel[] levels = { ConfirmationLevel.ApprovedSignOff, ConfirmationLevel.ApprovedTranslation, ConfirmationLevel.Translated };
            importSettings.ConfirmationLevels = levels;
            importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.KeepMostRecent;

            FieldValues fieldValuesCollection = new FieldValues();
            foreach (TmField fieldInTm in tm.Fields)
            {
                foreach (TmField tmField in tmFields)
                {
                    if (fieldInTm.Name == tmField.Name)
                    {

                        if (string.IsNullOrEmpty(tmField.SelectedValue))
                            continue;

                        FieldDefinition fieldDefinition = tm.TmAccess.FieldDefinitions[tmField.Name];
                        FieldValue fieldValue = fieldDefinition.CreateValue();
                        FieldValueType fieldValueType = fieldDefinition.ValueType;

                        if (fieldValueType == FieldValueType.SingleString || fieldValueType == FieldValueType.SinglePicklist)
                        {
                            fieldValue.Parse(tmField.SelectedValue);
                        }
                        else if (fieldValueType == FieldValueType.MultipleString || fieldValueType == FieldValueType.MultiplePicklist)
                        {
                            fieldValue.Add(tmField.SelectedValue);
                        }

                        fieldValuesCollection.Add(fieldValue);

                    }
                }
            }

            importSettings.ProjectSettings = fieldValuesCollection;
        }

        private void GetImportSettings(ImportSettings importSettings, FileBasedTranslationMemory tm, string job, string client, string status, string tp)
        {
            importSettings.CheckMatchingSublanguages = false;
            importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
            ConfirmationLevel[] levels = { ConfirmationLevel.ApprovedSignOff, ConfirmationLevel.ApprovedTranslation, ConfirmationLevel.Translated };
            importSettings.ConfirmationLevels = levels;
            importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.KeepMostRecent;

            // List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>();
            Dictionary<FieldDefinition, string> fieldDefinitions = new Dictionary<FieldDefinition, string>();
            FieldValues fieldValuesCollection = new FieldValues();
            FieldDefinition jobField = tm.FieldDefinitions["Job"];
            FieldDefinition clientField = tm.FieldDefinitions["Client"];
            FieldDefinition statusField = tm.FieldDefinitions["Status"];
            FieldDefinition tpField = tm.FieldDefinitions["T/P"];

            // Finally seems to work
            fieldDefinitions.Add(jobField, job);
            fieldDefinitions.Add(clientField, client);
            fieldDefinitions.Add(statusField, status);
            fieldDefinitions.Add(tpField, tp);

            for (int i = 0; i < fieldDefinitions.Count; i++)
            {
                if (string.IsNullOrEmpty(fieldDefinitions.ElementAt(i).Value))
                {
                    continue;
                }

                FieldValueType fieldType = fieldDefinitions.ElementAt(i).Key.ValueType;
                FieldValue fieldValue = fieldDefinitions.ElementAt(i).Key.CreateValue();

                if (fieldType == FieldValueType.SingleString || fieldType == FieldValueType.SinglePicklist)
                {
                    fieldValue.Parse(fieldDefinitions.ElementAt(i).Value);
                }
                else if (fieldType == FieldValueType.MultipleString || fieldType == FieldValueType.MultiplePicklist)
                {
                    fieldValue.Add(fieldDefinitions.ElementAt(i).Value);
                }

                fieldValuesCollection.Add(fieldValue);

            }

            importSettings.ProjectSettings = fieldValuesCollection;

        }

    }
}
