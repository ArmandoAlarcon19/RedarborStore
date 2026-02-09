namespace RedarborStore.Infrastructure.Auth;

public class Auth0Settings
{
    public const string SectionName = "Auth0";

    /// <summary>
    /// Auth0 domain
    /// Example: your-tenant.auth0.com
    /// </summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Auth0 API Identifier (audience)
    /// Example: https://redarborstore-api
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Auth0 SPA Application Client ID (for Scalar UI)
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Custom namespace for roles claim in JWT
    /// </summary>
    public string RolesNamespace { get; set; } = "https://redarborstore.com";

    /// <summary>
    /// Computed authority URL
    /// </summary>
    public string Authority => $"https://{Domain}/";
}