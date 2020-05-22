using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTC.ModelView
{
    using Model;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    internal class Program : ViewModel
    {
        private Model modelIn = new Model();
        private Model modelOut = new Model();

        public List<string> Drives { get; set; } = new List<string>();
        public string InputDrive { get; set; }
        public string InputName { get; set; }
        public ObservableCollection<string> InputFiles { get; set; }
        public string OutputDrive { get; set; }
        public string OutputName { get; set; }
        public ObservableCollection<string> OutputFiles { get; set; }


        private ICommand _comboBoxClick = null;
        public ICommand ComboBoxClick
        {
            get
            {
                if (_comboBoxClick == null)
                {
                    _comboBoxClick = new RelayCommand(
                        arg => { Drives=modelIn.getLogicalDrives(); modelOut.getLogicalDrives(); onPropertyChanged(nameof(Drives)); },
                        arg => true
                     );
                }
                return _comboBoxClick;
            }
        }
       
        private ICommand _changeInputDrive = null;
        public ICommand ChangeInputDrive
        {
            get
            {
                if (_changeInputDrive == null)
                {
                    _changeInputDrive = new RelayCommand(
                        arg => { modelIn.setPath(InputDrive); InputFiles = new ObservableCollection<string>(modelIn.getCurrentFilesAndFolders()) ; onPropertyChanged(nameof(InputFiles));},
                        arg => true
                     );
                }
                return _changeInputDrive;
            }
        }

        private ICommand _changeOutputDrive = null;
        public ICommand ChangeOutputDrive
        {
            get
            {
                if (_changeOutputDrive == null)
                {
                    _changeOutputDrive = new RelayCommand(
                        arg => { modelOut.setPath(OutputDrive); OutputFiles = new ObservableCollection<string>(modelOut.getCurrentFilesAndFolders()); onPropertyChanged(nameof(OutputFiles)); },
                        arg => true
                     );
                }
                return _changeOutputDrive;
            }
        }
        
        private ICommand _changeInputPath = null;
        public ICommand ChangeInputPath
        {
            get
            {
                if (_changeInputPath == null)
                {
                    _changeInputPath = new RelayCommand(
                        arg => { modelIn.setPath(InputName); InputFiles = new ObservableCollection<string>(modelIn.getCurrentFilesAndFolders()); InputName = modelIn.Name; onPropertyChanged(nameof(InputFiles), nameof(InputName)); },
                        arg => true
                     );
                }
                return _changeInputPath;
            }
        }

        private ICommand _changeOutputPath = null;
        public ICommand ChangeOutputPath
        {
            get
            {
                if (_changeOutputPath == null)
                {
                    _changeOutputPath = new RelayCommand(
                        arg => { modelOut.setPath(OutputName); OutputFiles = new ObservableCollection<string>(modelOut.getCurrentFilesAndFolders()); OutputName = modelOut.Name; onPropertyChanged(nameof(OutputFiles),nameof(OutputName)); },
                        arg => true
                     );
                }
                return _changeOutputPath;
            }
        }

        private ICommand _copy = null;
        public ICommand Copy
        {
            get
            {
                if (_copy == null)
                {
                    _copy = new RelayCommand(
                        arg => { modelIn.CopyFile(modelOut); InputName = modelIn.Name; OutputName = modelOut.Name; InputFiles = new ObservableCollection<string>(modelIn.getCurrentFilesAndFolders()); OutputFiles = new ObservableCollection<string>(modelOut.getCurrentFilesAndFolders()); onPropertyChanged(nameof(InputName), nameof(OutputName), nameof(OutputFiles),nameof(InputFiles)); },
                        arg => (!string.IsNullOrEmpty(InputName)) && (!string.IsNullOrEmpty(OutputName))
                     );
                }
                return _copy;
            }
        }

    }
}
