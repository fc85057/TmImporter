using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;

namespace TmImporter
{

    public class ViewModel : INotifyPropertyChanged
    {

        private Sdltm tm;
        private Xliff xliff;
        private string tmPath;
        private string xliffPath;
        
        public ICommand BrowseTmCommand { get; set; }
        public ICommand BrowseXliffCommand { get; set; }

        public ObservableCollection<string> Clients { get; set; }
        public ObservableCollection<string> Statuses { get; set; }

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
        }

        void LoadCommands()
        {
            BrowseTmCommand = new RelayCommand(BrowseForTm, CanBrowseForTm);
            BrowseXliffCommand = new RelayCommand(BrowseForXliff, CanBrowseForXliff);
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
                }
                foreach (string status in Tm.Statuses)
                {
                    Statuses.Add(status);
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
