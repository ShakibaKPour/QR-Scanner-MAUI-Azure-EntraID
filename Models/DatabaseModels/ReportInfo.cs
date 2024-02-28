namespace RepRepair.Models.DatabaseModels;
public class ReportInfo 
{
    public Guid ReportId { get; set; }
    public string QRCodeString { get; set; }
    public Guid QRCode { get; set; }
    public string? OriginalFaultReport { get; set; }
    public string? TranslatedFaultReport { get; set; }
    public Guid? SelectedLanguage { get; set; }
    public Guid? TypeOfReport { get; set; } //or public list<TypeOfReport> typeOfReport {get; set} = new();
}