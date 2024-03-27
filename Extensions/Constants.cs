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
        "api://dff3c905-f0d7-4071-99cf-9cb059eb6fcd/user_impersonation",
        "api://dff3c905-f0d7-4071-99cf-9cb059eb6fcd/User.Read",
        "api://dff3c905-f0d7-4071-99cf-9cb059eb6fcd/WriteToDatabase"
    };
}
