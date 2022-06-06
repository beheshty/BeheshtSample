using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static long GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var loggedInUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if(loggedInUserId == null)
            {
                return 0;
            }

            if (long.TryParse(loggedInUserId, out long userId))
            {
                return userId;
            }
            else
            {
                throw new Exception("Invalid type provided");
            }
        }
    }
}
