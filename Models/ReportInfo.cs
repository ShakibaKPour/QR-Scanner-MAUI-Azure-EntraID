namespace RepRepair.Models;

public class ReportInfo
{
    public int ReportId { get; set; }
    public int VoiceMessageId { get; set; }
    public string TextContent { get; set; }
    public string EmailContent { get; set; }
    public DateTime ReportedDate { get; set; }
}
