using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Pages;
using RepRepair.Services.ReportTypesService;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class ReportViewModel : BaseViewModel
{
    public ICommand NavigateToVoiceRecordCommand { get; set; }

    public ICommand NavigateToWriteCommand { get; set; }

    public ICommand NavigateToDefectListCommand { get; set; }

    public List<ReportType> ReportTypes
    {
        get => _reportServiceType.CachedReportTypes;
    }

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
                UpdateObjectProperties(_objectInfo);
            }
        }
    }
    private readonly ReportServiceType _reportServiceType;
    public ReportViewModel()
    {
        _reportServiceType = ServiceHelper.GetService<ReportServiceType>();
        NavigateToVoiceRecordCommand = new Command(async () => await NavigateToVoiceRecordCommandAsync());
        NavigateToWriteCommand = new Command(async ()=> await NavigateToWriteCommandAsync());
        NavigateToDefectListCommand= new Command(async()=> await NavigateToDefectListCommandAsync());
    }
    private void UpdateObjectProperties(ObjectInfo objectInfo)
    {
        if (objectInfo != null)
        {
            OnPropertyChanged(nameof(ObjectInfo.Name));
            OnPropertyChanged(nameof(ObjectInfo.Location));
            OnPropertyChanged(nameof(ObjectInfo.QRCode));
        }
    }
    private async Task NavigateToVoiceRecordCommandAsync()
    {
        await Shell.Current.GoToAsync(nameof(VoiceReportPage));
    }

    private async Task NavigateToWriteCommandAsync()
    {
        await Shell.Current.GoToAsync(nameof(WriteReportPage));
    }

    private async Task NavigateToDefectListCommandAsync()
    {
        await Shell.Current.GoToAsync(nameof(DefectListPage));
    }

}
