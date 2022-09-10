using Microsoft.AspNetCore.Http;

namespace MarketPlaceWeb.Utitlities.Extentions
{
    public static class HttpContextExtention
    {
        public static string GetUserIp(this HttpContext httpContext)
        {
            // Get User Ip
            return httpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
