using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Pages;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;
using RepRepair.Services.ReportTypesService;
using RepRepair.Services.ScanningService;
using System.Windows.Input;

namespace RepRepair.ViewModels;


public class DefectListViewModel : BaseViewModel
{

    private readonly IScanningService _scanningService;
    private readonly IAlertService _alertService;
    private readonly IDatabaseService _databaseService;
    private readonly LanguageSettingsService _languageSettingsService;
    private readonly ReportServiceType _reportServiceType;

    private List<DefectList> _defects;
    public List<DefectList> Defects
    {
        get => _defects;
        set
        {
            _defects = value;
            OnPropertyChanged(nameof(Defects));
        }
    }

    private DefectList _selectedDefect;
    public DefectList SelectedDefect
    {
        get => _selectedDefect;
        set
        {
            _selectedDefect = value;
            OnPropertyChanged(nameof(SelectedDefect));
        }
    }

    public ICommand SubmitDefectCommand { get; private set; }
    public ICommand OnRefresh { get; private set; }

    public List<ReportType> ReportTypes => _reportServiceType.CachedReportTypes;
    public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;
    public Languages SelectedLanguage
    {
        get => _languageSettingsService.CurrentLanguage;
        set
        {
            if (_languageSettingsService.CurrentLanguage != value)
            {
                _languageSettingsService.CurrentLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }
    }

    public DefectListViewModel()
    {
        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        _scanningService = ServiceHelper.GetService<IScanningService>();
        _alertService = ServiceHelper.GetService<IAlertService>();
        _reportServiceType = ServiceHelper.GetService<ReportServiceType>();
        _databaseService = ServiceHelper.GetService<IDatabaseService>();

        InitializeCommands();
        SubscribeToEvents();
        ValidateIsScanned();
        LoadDefectListAsync();
    }
    private void InitializeCommands()
    {
        OnRefresh = new Command(async () => await RefreshCommandExecuted());
        SubmitDefectCommand = new Command(async () => await SubmitDefect());
    }

    private void SubscribeToEvents()
    {
        _scanningService.ScannedObjectChanged += (objectInfo) =>
        {
            OnPropertyChanged(nameof(ObjectInfo));
        };
    }

    private async void ValidateIsScanned()
    {
        if (_scanningService.CurrentScannedObject == null)
        {
            await _alertService.ShowAlertAsync("Alert", "Start by scanning the QR code", "OK");
            await Shell.Current.GoToAsync("///ScanPage");
        }
    }

    private async Task LoadDefectListAsync()
    {
        try
        {
            var list = await _databaseService.GetDefectListAsync();
            Defects = list;
        }
        catch (Exception ex)
        {
            await _alertService.ShowAlertAsync("Error", "Failed to load defect list. Please click on refresh!", "OK");
        }
    }

    private async Task RefreshCommandExecuted()
    {
        try
        {
            await _languageSettingsService.RefreshAvailableLanguages();
            await _reportServiceType.RefreshReportTypes();
            await LoadDefectListAsync();
        }
        catch (Exception ex)
        {
            await _alertService.ShowAlertAsync("Error", $"Failed to refresh data: {ex.Message}", "OK");
        }
    }

    private async Task SubmitDefect()
    {
        try
        {
            if (SelectedDefect == null)
            {
                await _alertService.ShowAlertAsync("Alert", "Choose a defect", "OK");
                return;
            }

            var reportType = _reportServiceType.CachedReportTypes.FirstOrDefault(r => r.TypeOfReport == "Defect List");
            if (reportType == null)
            {
                await _reportServiceType.RefreshReportTypes();
                reportType = _reportServiceType.CachedReportTypes.FirstOrDefault(r => r.TypeOfReport == "Defect List");
            }

            var newReportData = new ReportInfo
            {
                SelectedLanguage = _languageSettingsService.CurrentLanguage.ID,
                OriginalFaultReport = SelectedDefect.Description,
                TranslatedFaultReport = null,
                TypeOfReport = reportType?.ID,
                QRCodeString = _scanningService.CurrentScannedObject.QRCode,
            };

            var success = await _databaseService.InsertReportAsync(newReportData, _languageSettingsService.CurrentLanguage);
            if (success)
            {
                await Shell.Current.GoToAsync(nameof(ThankYouPage));
                _scanningService.ResetScan();
            }
            else
            {
                await _alertService.ShowAlertAsync("Error", "Failed to submit the defect.", "OK");
            }
        }
        catch (Exception ex)
        {
            await _alertService.ShowAlertAsync("Error", $"An error occurred while submitting the defect: {ex.Message}", "OK");
        }
    }
}