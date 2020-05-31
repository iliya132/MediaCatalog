using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MediaCatalog.Model.DTO;
using MediaCatalog.Model.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace MediaCatalog.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IProgramsProvider TVProgramsProvider;
        private IMediaInfoProvider MediaInfo;

        #region DataSources
        private ObservableCollection<TV_ProgramDTO> _programs = new ObservableCollection<TV_ProgramDTO>();
        public ObservableCollection<TV_ProgramDTO> TV_Programs
        {
            get
            {
                return _programs;
            }
            set
            {
                _programs = value;
            }
        }
        #endregion

        #region CurrentValues
        private TV_ProgramDTO _selectedProgram = new TV_ProgramDTO();
        public TV_ProgramDTO SelectedProgram
        {
            get
            {
                return _selectedProgram;
            }
            set
            {
                _selectedProgram = value;
                RaisePropertyChanged("SelectedProgram");
            }
        }

        private TV_ProgramDTO _editedProgram = new TV_ProgramDTO();
        public TV_ProgramDTO EditedProgram
        {
            get
            {
                return _editedProgram;
            }
            set
            {
                _editedProgram = value;
                RaisePropertyChanged("EditedProgram");
                
            }
        }
        #endregion

        #region Commands

        public RelayCommand AddNewProgramCommand { get; set; }
        public RelayCommand<TV_ProgramDTO> SelectProgramCommand { get; set; }
        public RelayCommand<TV_ProgramDTO> EditProgramCommand { get; set; }
        public RelayCommand<TV_ProgramDTO> DeleteProgramCommand { get; set; }
        public RelayCommand SelectAvatarFileCommand { get; set; }
        public RelayCommand<DragEventArgs> DropCommand { get; set; }
        public RelayCommand SelectVideoFiles { get; set; }
        public RelayCommand<string> StartVideoCommand { get; set; }
        public RelayCommand<MediaFileDTO> DeleteMediaCommand { get; set; }

        #endregion

        public MainViewModel(IProgramsProvider programsProvider, IMediaInfoProvider mediaInfoProvider)
        {
            TVProgramsProvider = programsProvider;
            MediaInfo = mediaInfoProvider;
            InitializeCommands();
            InitializeData();
        }

        private void InitializeCommands()
        {
            AddNewProgramCommand = new RelayCommand(AddNewProgram);
            SelectProgramCommand = new RelayCommand<TV_ProgramDTO>(SelectProgram);
            EditProgramCommand = new RelayCommand<TV_ProgramDTO>(EditProgram);
            DeleteProgramCommand = new RelayCommand<TV_ProgramDTO>(DeleteProgram);
            SelectAvatarFileCommand = new RelayCommand(SelectAvatar);
            DropCommand = new RelayCommand<System.Windows.DragEventArgs>(HandleDrop);
            SelectVideoFiles = new RelayCommand(SelectMedia);
            StartVideoCommand = new RelayCommand<string>(StartVideo);
            DeleteMediaCommand = new RelayCommand<MediaFileDTO>(DeleteMedia);
        }

        private void InitializeData()
        {
            ReloadTVPrograms();
        }

        private void ReloadTVPrograms()
        {
            TV_Programs.Clear();
            foreach (TV_ProgramDTO program in TVProgramsProvider.GetPrograms())
            {
                TV_Programs.Add(program);
            }

            if (TV_Programs.Count > 0 && SelectedProgram == null)
            {
                SelectedProgram = TV_Programs[0];
            }
        }

        private void AddNewProgram()
        {
            EditedProgram = new TV_ProgramDTO();
            View.EditProgramWindow editWindow = new View.EditProgramWindow();
            if (editWindow.ShowDialog() == true)
            {
                TVProgramsProvider.AddProgramAsync(EditedProgram);
                TV_Programs.Add(EditedProgram);
            }
        }

        private void SelectProgram(TV_ProgramDTO program)
        {
            SelectedProgram = program;
        }

        private void EditProgram(TV_ProgramDTO program)
        {
            EditedProgram = new TV_ProgramDTO
            {
                Actors = program.Actors,
                AvatarSourcePath = program.AvatarSourcePath,
                Description = program.Description,
                Name = program.Name,
                YearEstablished = program.YearEstablished
            };

            View.EditProgramWindow editWindow = new View.EditProgramWindow();
            if (editWindow.ShowDialog() == true)
            {
                program.Actors = EditedProgram.Actors;
                program.AvatarSourcePath = EditedProgram.AvatarSourcePath;
                program.Description = EditedProgram.Description;
                program.Name = EditedProgram.Name;
                program.YearEstablished = EditedProgram.YearEstablished;
                TVProgramsProvider.CommitChangesAsync();
                ReloadTVPrograms();
            }
        }

        private void DeleteProgram(TV_ProgramDTO program)
        {
            if (program == null)
            {
                return;
            }
            TV_Programs.Remove(program);
            TVProgramsProvider.Remove(program);
            if (SelectedProgram == program && TV_Programs.Count > 0)
            {
                SelectedProgram = TV_Programs[0];
            }
        }

        private void DeleteMedia(MediaFileDTO mediaFile)
        {
            SelectedProgram.MediaFiles.Remove(mediaFile);
            RaisePropertyChanged("SelectedProgram");
            TVProgramsProvider.RemoveVideoFile(mediaFile);
        }

        private void SelectAvatar()
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "Изображения (*.jpg, *.png)|*.jpg;*.png";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                EditedProgram.AvatarSourcePath = openFile.FileName;
                RaisePropertyChanged("EditedProgram");
            }
        }

        private void HandleDrop(System.Windows.DragEventArgs DroppedArgs)
        {
            if (!IsDroppedDataAvailable(DroppedArgs))
            {
                return;
            }
            string[] fileNames = GetDroppedFileNames(DroppedArgs);
            AddVideosAsync(fileNames);
        }

        private bool IsDroppedDataAvailable(System.Windows.DragEventArgs args)
        {
            return args.Data.GetDataPresent(DataFormats.FileDrop);
        }

        private string[] GetDroppedFileNames(System.Windows.DragEventArgs args)
        {
            return (string[])args.Data.GetData(DataFormats.FileDrop);
        }

        private void SelectMedia()
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddVideosAsync(dialog.FileNames);
            }
        }

        private async void AddVideosAsync(string[] Files)
        {
            TV_ProgramDTO parentProgram = SelectedProgram;
            foreach (string file in Files)
            {
                if (!MediaInfo.IsMediaFile(file))
                {
                    continue;
                }

                await Task.Run(() =>
                {
                    MediaFileDTO video = MediaInfo.GetMediaFileInfo(file, SelectedProgram);
                    App.Current.Dispatcher.Invoke(() => parentProgram.MediaFiles.Add(video));
                    RaisePropertyChanged("SelectedProgram");
                });
            }
            TVProgramsProvider.CommitChangesAsync();
        }

        private void StartVideo(string fileName)
        {
            
            if (fileName != null && File.Exists(fileName))
            {
                Process.Start(fileName);
            }
        }
    }
}