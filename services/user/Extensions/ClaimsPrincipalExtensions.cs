using System.Security.Claims;

namespace user.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetPersonGuid(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("personGuid")?.Value;

            return Guid.TryParse(claim, out var guid)
                ? guid
                : null;
        }
    }
}
