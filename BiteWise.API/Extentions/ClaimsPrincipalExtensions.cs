using System.Security.Claims;

namespace BiteWise.Extentions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (claim is not null)
        {
            return claim.Value;
        }

        throw new NullReferenceException();
    }
}