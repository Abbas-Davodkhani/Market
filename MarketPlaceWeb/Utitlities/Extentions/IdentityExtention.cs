using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace MarketPlaceWeb.Utitlities.Extentions
{
    public static class IdentityExtention
    {
        public static long GetUserId(this ClaimsPrincipal claimsPrinciple)
        {
          
            if(claimsPrinciple != null)
            {
                var data = claimsPrinciple.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (data != null) return Convert.ToInt64(data.Value);
            }
            
            return default(long);
        }
        public static long GetUserId(this IPrincipal principle)
        {
            var user = (ClaimsPrincipal)principle;
            return user.GetUserId();
        }

    }
}
