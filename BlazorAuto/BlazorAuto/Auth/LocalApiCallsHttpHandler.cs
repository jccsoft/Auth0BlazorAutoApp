using Microsoft.Net.Http.Headers;

namespace BlazorAuto.Auth;

public class LocalApiCallsHttpHandler(
    IHttpContextAccessor httpContextAccessor,
    ILogger<LocalApiCallsHttpHandler> logger) : HttpClientHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            var cookies = httpContext.Request.Cookies;
            try
            {
                foreach (var cookie in cookies)
                {
                    var cookieValue = Uri.EscapeDataString(cookie.Value);
                    request.Headers.Add("Cookie", new CookieHeaderValue(cookie.Key, cookieValue).ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, "Error adding cookies to the request");
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}
