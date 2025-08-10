# Auth0BlazorAutoApp

Demonstration of how to secure a Blazor WebApp (.NET 9) with Auth0.

## 1. Create Blazor WebApp Auto Application
- Authentication type: none.
- Interactive render mode: Auto.
- Interactivity location: per page/component.
- In Properties/launchSettings.json, copy the url in the https:applicationUrl section (`https://localhost:<PORT>`) to use later in Auth0.

## 2. Register the application in Auth0

In the Auth0 dashboard:
- Menu: **Applications** > **Create Application**
- Application name: **<YOUR_APPLICATION_NAME>**
- Application type: **Regular Web Applications**
- Click **Create**.
- In the **Settings** section:
  - Save the **Domain**, **Client ID**, and **Client Secret** for later use.
  - Configure the following fields:
    - **Allowed Callback URLs**: `https://localhost:<PORT>/callback`
    - **Allowed Logout URLs**: `https://localhost:<PORT>/`

## 3. Server Project Configuration
- Nuget Packages:
  - Auth0.AspNetCore.Authentication
- Appsettings.json:
```json
"Auth0": {
  "Domain": "<YOUR_AUTH0_DOMAIN>",
  "ClientId": "<YOUR_CLIENT_ID>",
  "ClientSecret": "<YOUR_CLIENT_SECRET>"
}
```
- Program.cs:
  - `builder.Services.AddAuthenticationStateSerialization();` to serialize authentication state.
  - `builder.AddAuth0Services();` to configure Auth0 services (Auth0Extensions class).
  - `app.UseAuthentication();` to enable authentication.
  - `app.UseAuthorization();` to enable authorization.
  - `app.MapAuth0Endpoints();` to define authentication endpoints (Auth0Extensions class).

## 4. Client Project Configuration
- Nuget Packages:
  - Microsoft.AspNetCore.Components.WebAssembly.Authentication
- Program.cs:
  - `builder.Services.AddAuthenticationStateDeserialization();` to deserialize authentication state.
  - `builder.Services.AddHttpClient(
    Auth0Options.HttpClientName,
    client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    });` to configure the HTTP client.

## 5. Relevant Code
- Auth0Extensions.cs
  - Contains extension methods to configure Auth0 services and define authentication endpoints.
  - **AddAuth0Services function:**
    - Configures the necessary services for Auth0 authentication.
    - Reads configuration data from `appsettings.json`.
    - Registers the Auth0 authentication scheme and configures an `HttpClient` for authenticated calls.
      - It is very important to add `.ConfigurePrimaryHttpMessageHandler<LocalApiCallsHttpHandler>()` to forward authentication cookies to the server.
  - **MapAuth0Endpoints function:**
    - Defines the `/account/login` and `/account/logout` endpoints to handle login and logout with Auth0.
    - Uses authentication and cookie schemes to manage the authentication flow.
