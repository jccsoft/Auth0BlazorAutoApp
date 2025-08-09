using BlazorAuto.Auth;
using BlazorAuto.Components;
using BlazorAuto.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuth0Services();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization(options => options.SerializeAllClaims = true);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapAuth0Endpoints();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorAuto.Client.Components._Imports).Assembly);

app.MapServerEndpoints();

await app.RunAsync();
