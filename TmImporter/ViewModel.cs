using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace TmImporter
{

    public class ViewModel : INotifyPropertyChanged
    {

        private Sdltm tm;
        private Xliff xliff;
        private XliffImporter importer;
        private string tmPath;
        private string xliffPath;

        private string currentClient;
        private string currentStatus;
        private string job;
        private string tp;
        
        public ICommand BrowseTmCommand { get; set; }
        public ICommand BrowseXliffCommand { get; set; }
        public ICommand ImportXliffCommand { get; set; }
        public ICommand TestCommand { get; set; }

        public ObservableCollection<string> Clients { get; set; }
        public ObservableCollection<string> Statuses { get; set; }
        public ObservableCollection<TmField> TmFields { get; set; }

        public string Job
        {
            get
            {
                return job;
            }
            set
            {
                job = value;
                RaisePropertyChanged("Job");
            }
        }

        public string TP
        {
            get
            {
                return tp;
            }
            set
            {
                tp = value;
                RaisePropertyChanged("TP");
            }
        }

        public string CurrentClient
        {
            get
            {
                return currentClient;
            }
            set
            {
                currentClient = value;
                RaisePropertyChanged("CurrentClient");
            }
        }


        public string CurrentStatus
        {
            get
            {
                return currentStatus;
            }
            set
            {
                currentStatus = value;
                RaisePropertyChanged("CurrentStatus");
            }
        }

        public Sdltm Tm
        {
            get
            {
                return tm;
            }
            set
            {
                tm = value;
                RaisePropertyChanged("Tm");
            }
        }

        public string TmPath
        {
            get
            {
                return tmPath;
            }
            set
            {
                if (value != null && value.EndsWith("sdltm"))
                {
                    tmPath = value;
                    RaisePropertyChanged("TmPath");
                }
            }
        }

        public string XliffPath
        {
            get
            {
                return xliffPath;
            }
            set
            {
                if (value != null && value.EndsWith("sdlxliff"))
                {
                    xliffPath = value;
                    RaisePropertyChanged("XliffPath");
                }
            }
        }

        public ViewModel()
        {
            LoadCommands();
            Clients = new ObservableCollection<string>();
            Statuses = new ObservableCollection<string>();
            TmFields = new ObservableCollection<TmField>();
        }

        void LoadCommands()
        {
            BrowseTmCommand = new RelayCommand(BrowseForTm, CanBrowseForTm);
            BrowseXliffCommand = new RelayCommand(BrowseForXliff, CanBrowseForXliff);
            ImportXliffCommand = new RelayCommand(ImportXliff, CanImportXliff);
            TestCommand = new RelayCommand(Test, CanTest);
        }

        bool CanTest()
        {
            return true;
        }

        void Test()
        {
            foreach (var entry in TmFields)
            {
                MessageBox.Show("Entry: " + entry.Name + "\n" + "Selected value: " + entry.SelectedValue);
            }
        }

        private bool CanImportXliff()
        {
            return (Tm != null && xliff != null);
        }

        private void ImportXliff()
        {
            try
            {
                Xliff[] xliffArray = { xliff };
                importer = new XliffImporter();
                importer.ImportXliff(Tm, xliffArray, Job, CurrentClient, CurrentStatus, TP);
                MessageBox.Show("Done");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong : (\n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            
        }

        private bool CanBrowseForXliff()
        {
            return true;
        }

        private void BrowseForXliff()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SDL Xliffs (*.sdlxliff)|*.sdlxliff";
            if (openFileDialog.ShowDialog() == true)
            {
                XliffPath = openFileDialog.FileName;
                xliff = new Xliff(XliffPath);
            }
        }

        private bool CanBrowseForTm()
        {
            return true;
        }

        private void BrowseForTm()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SDL TMs (*.sdltm)|*.sdltm";
            if (openFileDialog.ShowDialog() == true)
            {
                TmPath = openFileDialog.FileName;
                Tm = new Sdltm(TmPath);
                foreach (string client in Tm.Clients)
                {
                    Clients.Add(client);
                    CurrentClient = Clients[0];
                }
                foreach (string status in Tm.Statuses)
                {
                    Statuses.Add(status);
                }

                int indexOfEvsEnd = Statuses.IndexOf("EVS End");
                if (indexOfEvsEnd == -1)
                {
                    indexOfEvsEnd = 0;
                }
                CurrentStatus = Statuses[indexOfEvsEnd];
                
                //foreach (var entry in Tm.Fields)
                //{
                //    MessageBox.Show(entry.Name + "\n" + entry.IsPicklist + "\n" + entry.PicklistValues);
                //}
                // Testing TM Fields
                foreach (var entry in Tm.Fields)
                {
                    TmFields.Add(entry);
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
