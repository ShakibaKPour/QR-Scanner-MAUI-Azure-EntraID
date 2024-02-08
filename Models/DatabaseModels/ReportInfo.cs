namespace RepRepair.Models.DatabaseModels;

//public class ReportInfo
//{
//    public int ReportId { get; set; }
//    public int? VoiceMessageId { get; set; }
//    public string? TextContent { get; set; }
//    public DateTime ReportedDate { get; set; }
//}

public class ReportInfo 
{
    public int ReportId { get; set; }
    public string QRCode { get; set; }
    public string? OriginalFaultReport { get; set; }
    public string? TranslatedFaultReport { get; set; }
   // public DateTime ReportDate { get; set; }
    public string? SelectedLanguage { get; set; }
    public string? TypeOfReport { get; set; } //or public list<TypeOfReport> typeOfReport {get; set} = new();
}