namespace RepRepair.Models.DatabaseModels;

public class ReportInfo
{
    public int ReportId { get; set; }
    public int? VoiceMessageId { get; set; }
    public string? TextContent { get; set; }
    public DateTime ReportedDate { get; set; }
}
