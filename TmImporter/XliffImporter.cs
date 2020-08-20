using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studio.AssemblyResolver;
using Sdl.Core.Globalization;
using Sdl.Core.Api;
using Sdl.Core.TM.ImportExport;
using Sdl.Core.LanguageProcessing;
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

        private void GetImportSettings(ImportSettings importSettings, FileBasedTranslationMemory tm, string job, string client, string status, string tp)
        {
            importSettings.CheckMatchingSublanguages = false;
            importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
            ConfirmationLevel[] levels = { ConfirmationLevel.ApprovedSignOff, ConfirmationLevel.ApprovedTranslation, ConfirmationLevel.Translated };
            importSettings.ConfirmationLevels = levels;
            importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.KeepMostRecent;

            FieldValues fieldValuesCollection = new FieldValues();
            FieldDefinition jobField = tm.FieldDefinitions["Job"];
            FieldDefinition clientField = tm.FieldDefinitions["Client"];
            FieldDefinition statusField = tm.FieldDefinitions["Status"];
            FieldDefinition tpField = tm.FieldDefinitions["T/P"];

            FieldValue jobValue = jobField.CreateValue();
            jobValue.Name = "Job";
            jobValue.Add(job);
            fieldValuesCollection.Add(jobValue);
            /*
            FieldValue tpValue = tpField.CreateValue();
            tpValue.Name = "T/P";
            tpValue.Add(tp);
            fieldValuesCollection.Add(tpValue);
            
            FieldValue clientValue = clientField.CreateValue();
            clientValue.Name = "Client";
            clientValue.Add(client);
            fieldValuesCollection.Add(clientValue);

            FieldValue statusValue = statusField.CreateValue();
            statusValue.Name = "Status";
            statusValue.Add(status);
            fieldValuesCollection.Add(statusValue);
            */
            importSettings.ProjectSettings = fieldValuesCollection;
        }

    }
}
