using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Pages;
using RepRepair.Services.ReportTypesService;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class ReportViewModel : BaseViewModel
{
        private readonly ReportServiceType _reportServiceType;

        public ICommand NavigateToVoiceRecordCommand { get; private set; }
        public ICommand NavigateToWriteCommand { get; private set; }
        public ICommand NavigateToDefectListCommand { get; private set; }

        public List<ReportType> ReportTypes => _reportServiceType.CachedReportTypes;

        private ObjectInfo _objectInfo;
        public ObjectInfo ObjectInfo
        {
            get => _objectInfo;
            set
            {
                if (_objectInfo != value)
                {
                    _objectInfo = value;
                    OnPropertyChanged(nameof(ObjectInfo));
                    UpdateObjectProperties();
                }
            }
        }

        public ReportViewModel()
        {
            _reportServiceType = ServiceHelper.GetService<ReportServiceType>() ?? throw new InvalidOperationException("ReportServiceType service not available");
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            NavigateToVoiceRecordCommand = new Command(async () => await NavigateToAsync(nameof(VoiceReportPage)));
            NavigateToWriteCommand = new Command(async () => await NavigateToAsync(nameof(WriteReportPage)));
            NavigateToDefectListCommand = new Command(async () => await NavigateToAsync(nameof(DefectListPage)));
        }

        private async Task NavigateToAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        private void UpdateObjectProperties()
        {
            OnPropertyChanged(nameof(ObjectInfo.Name));
            OnPropertyChanged(nameof(ObjectInfo.Location));
            OnPropertyChanged(nameof(ObjectInfo.QRCode));
        }
    }
