namespace BlazorAuto.Client.Auth;

public sealed class Auth0Options
{
    public const string ConfigurationSection = "Auth0";
    public const string HttpClientName = "Auth0HttpClientName";

    public string Domain { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
