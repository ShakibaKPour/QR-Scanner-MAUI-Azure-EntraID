using RepRepair.Models.DatabaseModels;
using RepRepair.Services.DB;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using RepRepair.Extensions;
using RepRepair.Services.AlertService;

public class LanguageSettingsService
{
    private const string DefaultLanguageId = "dfd8bfab-6cee-4cc1-9a53-9ebc5740050f"; // Default GUID as a string
    private const string LanguageKey = "SelectedLanguage";
    private readonly IDatabaseService _databaseService;
    private readonly IAlertService _alertService;

    public ObservableCollection<Languages> AvailableLanguages { get; } = new ObservableCollection<Languages>();

    private Languages _currentLanguage;
    public Languages CurrentLanguage
    {
        get
        {
            // If _currentLanguage is null, try to load from preferences
            if (_currentLanguage == null)
            {
                var languageIdString = Preferences.Get(LanguageKey, DefaultLanguageId);
                if (Guid.TryParse(languageIdString, out Guid languageId))
                {
                    // Set from available languages, if possible
                    _currentLanguage = AvailableLanguages.FirstOrDefault(lang => lang.ID == languageId);
                }

                // If still null, set to a default value to avoid null reference exceptions
                if (_currentLanguage == null && AvailableLanguages.Count > 0)
                {
                    _currentLanguage = AvailableLanguages[0];
                }
            }
            return _currentLanguage ??= new Languages { ID = Guid.Parse(DefaultLanguageId), Language = "en-US" }; // Fallback default
        }
        set
        {
            _currentLanguage = value;
            // Store the language ID in preferences
            if (value != null)
            {
                 Preferences.Set(LanguageKey, _currentLanguage.ID.ToString());
            }
        }
    }

    public LanguageSettingsService()
    {
        _databaseService = ServiceHelper.GetService<IDatabaseService>();
        _alertService= ServiceHelper.GetService<IAlertService>();
    }

    public async Task FetchAndUpdateAvailableLanguages()
    {
        try
        {
            var languages = await _databaseService.GetAvailableLanguagesAsync();
            if (languages != null)
            {
                AvailableLanguages.Clear();
                foreach (var language in languages)
                {
                    AvailableLanguages.Add(language);
                }
                // Ensure CurrentLanguage is updated correctly
                CurrentLanguage = AvailableLanguages.FirstOrDefault(lang => lang.ID.ToString() == Preferences.Get(LanguageKey, DefaultLanguageId)) ?? CurrentLanguage;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch or update available languages: {ex.Message}");
            _alertService.ShowAlert("Error", "Failed to fetch or update available languages. Please refresh!");
            
        }
    }

    public async Task RefreshAvailableLanguages()
    {
        var languages = await _databaseService.GetAvailableLanguagesAsync();
        if (languages != null)
        {
            AvailableLanguages.Clear();
            foreach (var language in languages)
            {
                AvailableLanguages.Add(language);
            }
        }
       // await FetchAndUpdateAvailableLanguages(); // Reuse the logic in FetchAndUpdateAvailableLanguages
    }
}