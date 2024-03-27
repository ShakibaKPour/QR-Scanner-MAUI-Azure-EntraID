namespace RepRepair.Services.Cognitive;

public interface IAzureCognitiveService
{
    public Task<string> TranscribeAudioAsync();

}
