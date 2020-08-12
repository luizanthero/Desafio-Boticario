using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace boticario.Options
{
    public class UserTokenOptions
    {
        public static string GetClaimTypesNameValue(IIdentity identity)
        {
            string result = ((ClaimsIdentity)identity).Claims
                .Where(item => item.Type.Equals(ClaimTypes.Name)).Select(item => item.Value).FirstOrDefault();

            return result;
        }
    }
}
