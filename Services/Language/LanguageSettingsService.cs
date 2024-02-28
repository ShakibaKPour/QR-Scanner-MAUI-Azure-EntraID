using RepRepair.Models.DatabaseModels;
using RepRepair.Services.DB;
using System.Collections.ObjectModel;
using System.Linq;

namespace RepRepair.Services.Language
{
    public class LanguageSettingsService
    {
        private const string DefaultLanguageId = "dfd8bfab-6cee-4cc1-9a53-9ebc5740050f"; // Default GUID as a string
        private const string LanguageKey = "SelectedLanguage";
        //public ObservableCollection<Languages> AvailableLanguages { get; } = new ObservableCollection<Languages>();
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
                }
                return _currentLanguage;
            }
            set
            {
                _currentLanguage = value;
                // Store the language ID in preferences
                Preferences.Set(LanguageKey, value?.ID.ToString() ?? DefaultLanguageId);
            }
        }

        public async Task FetchAndUpdateAvailableLanguages(IDatabaseService databaseService)
        {
            var languages = await databaseService.GetAvailableLanguagesAsync();
            if (languages != null)
            {
                AvailableLanguages.Clear();
                foreach (var language in languages)
                {
                    AvailableLanguages.Add(language);
                    Console.WriteLine(language.Language);
                }
                // Update CurrentLanguage based on the newly fetched languages and the ID stored in preferences
                _currentLanguage = AvailableLanguages.FirstOrDefault(lang => lang.ID.ToString() == Preferences.Get(LanguageKey, DefaultLanguageId));
            }
        }

        public async Task RefreshAvailableLanguages(IDatabaseService databaseService)
        {
            var languages = await databaseService.GetAvailableLanguagesAsync();
            if (languages != null)
            {
                AvailableLanguages.Clear();
                foreach (var language in languages)
                {
                    AvailableLanguages.Add(language);
                }
            }
        }
    }
}


























//using RepRepair.Models.DatabaseModels;
//using RepRepair.Services.DB;
//using System.Collections.ObjectModel;

//namespace RepRepair.Services.Language
//{
//    public class LanguageSettingsService
//    {
//        private const string DefaultLanguage = "en-US";
//        private const string LanguageKey = "SelectedLanguage";
//        public ObservableCollection<Languages> AvailableLanguages { get; } = new ObservableCollection<Languages>();

//        public string CurrentLanguage
//        {
//            get => Preferences.Get(LanguageKey, DefaultLanguage);
//            set => Preferences.Set(LanguageKey, value);
//        }

//        public async Task FetchAndUpdateAvailableLanguages(IDatabaseService databaseService)
//        {
//            var languages = await databaseService.GetAvailableLanguagesAsync();

//            if (languages != null)
//            {
//                AvailableLanguages.Clear();
//                foreach (var language in languages)
//                {
//                    AvailableLanguages.Add(language);
//                    Console.Write($"Language {language}");
//                }

//            }
//        }

//    }
//}
