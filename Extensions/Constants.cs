namespace RepRepair.Extensions;

public static class Constants
{
    /// <summary>
    /// The base URI for the Datasync service.
    /// </summary>
    public static string ServiceUri = "https://reprepair.azurewebsites.net";

    /// <summary>
    /// The application (client) ID for the native app within Microsoft Entra ID
    /// </summary>
    public static string ApplicationId = "dff3c905-f0d7-4071-99cf-9cb059eb6fcd";

    /// <summary>
    /// The list of scopes to request
    /// </summary>
    public static string[] Scopes = new[]
    {
       // "api://dff3c905-f0d7-4071-99cf-9cb059eb6fcd/user_impersonation",
        "api://dff3c905-f0d7-4071-99cf-9cb059eb6fcd/User.Read",
        "api://dff3c905-f0d7-4071-99cf-9cb059eb6fcd/WriteToDatabase"
    };

    public static string speechKey = "e1e2299f4ccd49e3b2c3859420c5ae25";
    public static string speechRegion = "swedencentral";

    public static string translateKey = "fb58a08c2f454884bc7b434f40794193";
    public static string translateRegion = "swedencentral";
    public static string translateEndpoint = "https://api.cognitive.microsofttranslator.com";
}
