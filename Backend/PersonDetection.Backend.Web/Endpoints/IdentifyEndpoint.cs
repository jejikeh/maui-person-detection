using System.Security.Claims;

namespace PersonDetection.Backend.Web.Endpoints;

public static class IdentifyEndpoint
{
    public static Dictionary<string, string> Handler(ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
}