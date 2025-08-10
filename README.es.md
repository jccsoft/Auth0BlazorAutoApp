# Auth0BlazorAutoApp

Demostración de cómo asegurar con Auth0 una aplicación Blazor WebApp (.NET 9).

## 1. Crear aplicación Blazor WebApp Auto
- Authentication type: none.
- Interactive render mode: Auto.
- Interactivity location: per page/component.
- En Properties/launchSettings.json, copia la url en la sección https:applicationUrl (`https://localhost:<PORT>`) para usarla luego en Auth0.
 
## 2. Registro de la aplicación en Auth0

En el panel de Auth0.
- Menú: **Applications** > **Create Application**
- Nombre de la aplicación: **<TU_NOMBRE_DE_APLICACIÓN>**
- Tipo de aplicación: **Regular Web Applications**
- Clic en **Create**.
- En la sección de **Settings**
  - Guarda el **Domain**, **Client ID** y **Client Secret** que necesitarás más adelante.
  - Configura los siguientes campos:
    - **Allowed Callback URLs**: `https://localhost:<PORT>/callback`
    - **Allowed Logout URLs**: `https://localhost:<PORT>/`

## 3. Configuración de proyecto Servidor
- Nuget Packages: 
  - Auth0.AspNetCore.Authentication
- Appsettings.json:
```json
"Auth0": {
  "Domain": "<TU_DOMINIO_AUTH0>",
  "ClientId": "<TU_CLIENT_ID>",
  "ClientSecret": "<TU_CLIENT_SECRET>"
}
```
- Program.cs:
  - `builder.Services.AddAuthenticationStateSerialization();` para serializar el estado de autenticación.
  - `builder.AddAuth0Services();` para configurar los servicios de Auth0 (clase Auth0Extensions).
  - `app.UseAuthentication();` para habilitar la autenticación.
  - `app.UseAuthorization();` para habilitar la autorización.
  - `app.MapAuth0Endpoints();` para definir los endpoints de autenticación (clase Auth0Extensions).

## 4. Configuración de proyecto Cliente
- Nuget Packages: 
  - Microsoft.AspNetCore.Components.WebAssembly.Authentication
- Program.cs:
  - `builder.Services.AddAuthenticationStateDeserialization();` para deserializar el estado de autenticación.
  - `builder.Services.AddHttpClient(
    Auth0Options.HttpClientName,
    client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    });` para configurar el cliente HTTP.
  
## 5. Código relevante
- Auth0Extensions.cs
  - Contiene métodos de extensión para configurar los servicios de Auth0 y definir los endpoints de autenticación.
  - Función **AddAuth0Services:**
    - Configura los servicios necesarios para la autenticación con Auth0.
    - Lee los datos de configuración desde `appsettings.json`.
    - Registra el esquema de autenticación de Auth0 y configura un `HttpClient` para llamadas autenticadas.
      - Muy importante añadir `.ConfigurePrimaryHttpMessageHandler<LocalApiCallsHttpHandler>()` para reenviar las cookies de autenticación al servidor.
  - Función **MapAuth0Endpoints:**
    - Define los endpoints `/account/login` y `/account/logout` para gestionar el inicio y cierre de sesión usando Auth0.
    - Utiliza los esquemas de autenticación y cookies para manejar el flujo de autenticación.

