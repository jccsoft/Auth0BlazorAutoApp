namespace BlazorAuto.Endpoints;

public static class ServerEndpoints
{
    public static void MapServerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/user", (HttpContext httpContext) =>
        {
            var user = httpContext.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                return user.Identity.Name;
            }

            return "No autenticado";
        });
    }
}
