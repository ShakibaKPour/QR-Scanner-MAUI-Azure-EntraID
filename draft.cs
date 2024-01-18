//using IntelliJ.Lang.Annotations;
//using Java.Awt.Font;
//using Java.Lang;
//using Microsoft.Maui;
//using System.ComponentModel;
//using System.Windows.Input;
//using static Android.Icu.Text.CaseMap;
//using static System.Net.Mime.MediaTypeNames;

//public class ObjectInfo
//{
//    public int ObjectId { get; set; }
//    public string Name { get; set; }
//    public string Location { get; set; }
//    public string QRCode { get; set; }
//}

//public class ReportInfo
//{
//    public int ReportId { get; set; }
//    public int VoiceMessageId { get; set; }
//    public string TextContent { get; set; }
//    public string EmailContent { get; set; }
//    public DateTime ReportedDate { get; set; }
//}


//public class VoiceMessageInfo
//{
//    public int Id { get; set; }
//    public string Language { get; set; }
//    public string FilePathOrAudio { get; set; } // You might want to adjust this depending on how you're handling audio data
//    public string Transcription { get; set; }
//    public string Translation { get; set; }
//}


//public class DefectInfo
//{
//    public int DefectId { get; set; }
//    public int ReportId { get; set; }
//    public int ObjectId { get; set; }
//}



//public class HomeViewModel : INotifyPropertyChanged
//{
//    public ICommand ScanCommand { get; private set; }
//    // Add other properties and commands as needed

//    public HomeViewModel()
//    {
//        ScanCommand = new Command(OnScan);
//    }

//    private void OnScan()
//    {
//        // Logic to navigate to ScanPage
//    }

//    public event PropertyChangedEventHandler PropertyChanged;
//    protected virtual void OnPropertyChanged(string propertyName)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//}
//```

//### 2. ScanViewModel

//Manages the QR code scanning and retrieves `ObjectInfo`.

//```csharp
//public class ScanViewModel : INotifyPropertyChanged
//{
//    private string qrCode;

//    public string QRCode
//    {
//        get => qrCode;
//        set
//        {
//            qrCode = value;
//            OnPropertyChanged(nameof(QRCode));
//            // Additional logic after QR code is set
//        }
//    }

//    // Other properties and commands for handling scanning

//    public event PropertyChangedEventHandler PropertyChanged;
//    protected virtual void OnPropertyChanged(string propertyName)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//}
//```

//### 3. ReportViewModel

//Handles the creation and submission of a report.

//```csharp
//public class ReportViewModel : INotifyPropertyChanged
//{
//    public ReportInfo Report { get; set; }
//    public ICommand SubmitReportCommand { get; private set; }

//    public ReportViewModel()
//    {
//        Report = new ReportInfo();
//        SubmitReportCommand = new Command(SubmitReport);
//    }

//    private void SubmitReport()
//    {
//        // Logic to submit the report
//    }

//    public event PropertyChangedEventHandler PropertyChanged;
//    protected virtual void OnPropertyChanged(string propertyName)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//}
//```

//### 4. VoiceRecordViewModel

//Handles voice recording, transcription, and translation.

//```csharp
//public class VoiceRecordViewModel : INotifyPropertyChanged
//{
//    public VoiceMessageInfo VoiceMessage { get; set; }
//    public ICommand RecordCommand { get; private set; }
//    public ICommand SubmitVoiceMessageCommand { get; private set; }

//    public VoiceRecordViewModel()
//    {
//        VoiceMessage = new VoiceMessageInfo();
//        RecordCommand = new Command(RecordVoiceMessage);
//        SubmitVoiceMessageCommand = new Command(SubmitVoiceMessage);
//    }

//    private void RecordVoiceMessage()
//    {
//        // Logic for voice recording
//    }

//    private void SubmitVoiceMessage()
//    {
//        // Logic to submit the voice message
//    }

//    public event PropertyChangedEventHandler PropertyChanged;
//    protected virtual void OnPropertyChanged(string propertyName)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//}

//### 1. HomePage (Welcome and Language Selection)

//```xml
//< ContentPage xmlns = "http://schemas.microsoft.com/dotnet/2021/maui"
//             xmlns: x = "http://schemas.microsoft.com/winfx/2009/xaml"
//             x: Class = "YourAppNamespace.HomePage"
//             Title = "Home" >

//    < ContentPage.BindingContext >
//        < local:HomeViewModel />
//    </ ContentPage.BindingContext >

//    < StackLayout Padding = "20" >
//        < Label Text = "Welcome to the App" FontSize = "Large" />
//        < !--Add language selection UI here -->

//        <Button Text="Scan QR Code" Command="{Binding ScanCommand}"/>
//    </StackLayout>
//</ContentPage>
//```

//### 2. ScanPage (QR Code Scanning)

//```xml
//<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
//             xmlns:x = "http://schemas.microsoft.com/winfx/2009/xaml"
//             x: Class = "YourAppNamespace.ScanPage"
//             Title = "Scan QR Code" >

//    < ContentPage.BindingContext >
//        < local:ScanViewModel />
//    </ ContentPage.BindingContext >

//    < StackLayout Padding = "20" >
//        < Label Text = "Scan the QR Code on the Object" />
//        < !--Implement QR code scanner UI here -->
//        <!-- Show scanned QR code data or object details -->
//    </StackLayout>
//</ContentPage>
//```

//### 3. ReportPage (Reporting Malfunction)

//```xml
//<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
//             xmlns:x = "http://schemas.microsoft.com/winfx/2009/xaml"
//             x: Class = "YourAppNamespace.ReportPage"
//             Title = "Report Malfunction" >

//    < ContentPage.BindingContext >
//        < local:ReportViewModel />
//    </ ContentPage.BindingContext >

//    < StackLayout Padding = "20" >
//        < Entry Placeholder = "Enter Text Content" Text = "{Binding Report.TextContent}" />
//        < Entry Placeholder = "Enter Email Content" Text = "{Binding Report.EmailContent}" />
//        < !--Add other UI elements for reporting malfunction -->

//        <Button Text="Submit Report" Command="{Binding SubmitReportCommand}"/>
//    </StackLayout>
//</ContentPage>
//```

//### 4. VoiceRecordPage (Voice Recording and Transcription)

//```xml
//<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
//             xmlns:x = "http://schemas.microsoft.com/winfx/2009/xaml"
//             x: Class = "YourAppNamespace.VoiceRecordPage"
//             Title = "Voice Record" >

//    < ContentPage.BindingContext >
//        < local:VoiceRecordViewModel />
//    </ ContentPage.BindingContext >

//    < StackLayout Padding = "20" >
//        < Label Text = "Record your message" />
//        < Button Text = "Start Recording" Command = "{Binding RecordCommand}" />
//        < !--Add UI for showing transcription -->

//        <Button Text="Submit" Command="{Binding SubmitVoiceMessageCommand}"/>
//    </StackLayout>
//</ContentPage>
//### 1. Azure Function Service

//This service interacts with your Azure Functions for tasks like voice transcription, translation, and database interactions.

//```csharp
//public class AzureFunctionService
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _baseUri = "https://your-azure-function-app.azurewebsites.net/";

//    public AzureFunctionService(HttpClient httpClient)
//    {
//        _httpClient = httpClient;
//    }

//    public async Task<string> TranscribeVoiceAsync(string voiceFilePath)
//    {
//        // Implement the logic to send the voice file to the Azure Function and get the transcription
//    }

//    public async Task<string> TranslateTextAsync(string text, string targetLanguage)
//    {
//        // Implement the logic to send the text to the Azure Function for translation
//    }

//    // Add other methods for interacting with Azure Functions as needed
//}
//```

//### 2. Database Service

//This service handles interactions with your Azure SQL database. You'll typically use an API endpoint or direct database access, depending on your architecture.

//```csharp
//public class DatabaseService
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _apiBaseUrl = "https://your-api-endpoint/";

//    public DatabaseService(HttpClient httpClient)
//    {
//        _httpClient = httpClient;
//    }

//    public async Task<bool> SaveReportAsync(ReportInfo report)
//    {
//        // Implement the logic to save the report to the database
//        // This might involve sending the report data to an API endpoint
//    }

//    // Add other methods for different database operations as needed
//}
//```

//### 3. Voice Recording Service

//This service manages the voice recording functionality.

//```csharp
//public class VoiceRecordingService
//{
//    public VoiceRecordingService()
//    {
//        // Initialize the voice recording service
//    }

//    public async Task StartRecordingAsync()
//    {
//        // Implement the logic to start voice recording
//    }

//    public async Task<string> StopRecordingAsync()
//    {
//        // Implement the logic to stop the recording and return the file path or audio data
//    }

//    // Add other functionalities related to voice recording as needed
//}
//### Utilities

//Let's create a simple utility for language management as an example:

//```csharp
//public static class LanguageManager
//{
//    public static string GetLanguageName(string languageCode)
//    {
//        // Add logic to convert language code to a readable language name
//        // For example, "en" -> "English", "fr" -> "French", etc.
//        return languageCode; // Placeholder, replace with actual implementation
//    }

//    // Add other utility methods as needed, such as for language conversion, etc.
//}
//```

//### Navigation

//In.NET MAUI, navigation can be handled in various ways. Here’s an example using a Navigation Service that you can integrate with your MVVM setup:

//First, define an interface for navigation:

//```csharp
//public interface INavigationService
//{
//    Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;
//    Task GoBackAsync();
//}
//```

//Then, implement this interface:

//```csharp
//public class NavigationService : INavigationService
//{
//    private readonly Dictionary<Type, Type> _mappings; // Maps ViewModels to Views

//    public NavigationService()
//    {
//        _mappings = new Dictionary<Type, Type>();
//        CreatePageViewModelMappings();
//    }

//    private void CreatePageViewModelMappings()
//    {
//        // Map ViewModels to Views here
//        _mappings.Add(typeof(HomeViewModel), typeof(HomePage));
//        _mappings.Add(typeof(ScanViewModel), typeof(ScanPage));
//        // Add other mappings
//    }

//    public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
//    {
//        var pageType = _mappings[typeof(TViewModel)];
//        var page = Activator.CreateInstance(pageType) as Page;

//        if (page != null)
//        {
//            await Application.Current.MainPage.Navigation.PushAsync(page);
//        }
//    }

//    public async Task GoBackAsync()
//    {
//        await Application.Current.MainPage.Navigation.PopAsync();
//    }
//}

//### Integrating Navigation in ViewModels

//Here's how you might use the `NavigationService` in a ViewModel:

//```csharp
//public class HomeViewModel : BaseViewModel
//{
//    private readonly INavigationService _navigationService;

//    public ICommand GoToScanPageCommand { get; }

//    public HomeViewModel(INavigationService navigationService)
//    {
//        _navigationService = navigationService;
//        GoToScanPageCommand = new Command(async () => await _navigationService.NavigateToAsync<ScanViewModel>());
//    }

//    // Other ViewModel properties and methods
//}

