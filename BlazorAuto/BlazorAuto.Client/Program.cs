using BlazorAuto.Client.Auth;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddAuthorizationCore()
    .AddCascadingAuthenticationState()
    .AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient(
    Auth0Options.HttpClientName,
    client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    });

await builder.Build().RunAsync();
