namespace RepRepair.Models;

public class VoiceMessageInfo
{
    public int Id { get; set; }
    public string Language { get; set; }
    public string AudioFilePath { get; set; } // Filepath or binary data ???
    public string Transcription { get; set; }
    public string Translation { get; set; }
}
