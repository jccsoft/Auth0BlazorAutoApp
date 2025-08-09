using Auth0.AspNetCore.Authentication;
using BlazorAuto.Client.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlazorAuto.Auth;

public static class Auth0Extensions
{
    public static WebApplicationBuilder AddAuth0Services(this WebApplicationBuilder builder)
    {
        var authOptions = builder.Configuration.GetSection(Auth0Options.ConfigurationSection).Get<Auth0Options>();
        string appBaseUrl = builder.Configuration.GetValue<string>("AppBaseAddress") ?? "";

        builder.Services.AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = authOptions!.Domain;
            options.ClientId = authOptions!.ClientId;
            options.ClientSecret = authOptions!.ClientSecret; // Para posible acceso a API externa
        });

        builder.Services
            .AddHttpContextAccessor()
            .AddTransient<LocalApiCallsHttpHandler>();

        builder.Services.AddHttpClient(
            Auth0Options.HttpClientName,
            client =>
            {
                client.BaseAddress = new Uri(appBaseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler<LocalApiCallsHttpHandler>();


        return builder;
    }


    public static void MapAuth0Endpoints(this WebApplication app)
    {
        app.MapGet("account/login", async (HttpContext context, string returnUrl = "/") =>
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                 .WithRedirectUri(returnUrl)
                 .Build();
            await context.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        });


        app.MapGet("account/logout", async (context) =>
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                 .WithRedirectUri("/")
                 .Build();
            await context.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        });
    }

}
