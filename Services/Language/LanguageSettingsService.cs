namespace RepRepair.Services.Language
{
    public class LanguageSettingsService
    {
        private const string DefaultLanguage = "en-US";
        private const string LanguageKey = "SelectedLanguage";

        public string CurrentLanguage
        {
            get => Preferences.Get(LanguageKey, DefaultLanguage);
            set => Preferences.Set(LanguageKey, value);
        }

    }
}
