using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Abstractions;


// Full directory URL, in the form of https://login.microsoftonline.com/<tenant_id>
const string authority = " https://login.microsoftonline.com/c512e30b-c327-43a9-9340-0d49119c380a";

// 'Enter the client ID obtained from the Microsoft Entra Admin Center
const string clientId = "6215d839-188c-48a4-91d5-418ac654f9df";

// Client secret 'Value' (not its ID) from 'Client secrets' in the Microsoft Entra Admin Center
const string clientSecret = "xxx";

// Client 'Object ID' of app registration in Microsoft Entra Admin Center - this value is a GUID
const string clientObjectId = "84c96022-3bbd-4e10-bafb-aeb7a539f2e5";

// This app instance should be a long-lived instance because
// it maintains the in-memory token cache.
var msalClient = ConfidentialClientApplicationBuilder.Create(clientId)
    .WithClientSecret(clientSecret)
    .WithAuthority(new Uri(authority))
    .Build();

msalClient.AddInMemoryTokenCache();

var authenticationResult =
    await msalClient.AcquireTokenForClient(["https://graph.microsoft.com/.default"]).ExecuteAsync();

var jsonSerializerOptions = new JsonSerializerOptions
    { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

// Get *this* application's application object from Microsoft Graph
// await GetProfile();






// Get tasks from the tasks API
await GetTasks();

async Task GetProfile()
{
    var httpClient = new HttpClient();

    using var graphRequest =
        new HttpRequestMessage(method: HttpMethod.Get,
            requestUri: $"https://graph.microsoft.com/v1.0/applications/{clientObjectId}");

    graphRequest.Headers.Authorization =
        new AuthenticationHeaderValue(scheme: "Bearer", parameter: authenticationResult.AccessToken);

    var graphResponseMessage = await httpClient.SendAsync(graphRequest);
    graphResponseMessage.EnsureSuccessStatusCode();

    using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
    Console.WriteLine(JsonSerializer.Serialize(value: graphResponseJson, options: jsonSerializerOptions));
}

async Task GetTasks()
{
    var httpClient = new HttpClient();
    using var request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: "http://localhost:5250/tasks");
    request.Headers.Authorization =
        new AuthenticationHeaderValue(scheme: "Bearer", parameter: authenticationResult.AccessToken);
    
    Console.WriteLine(authenticationResult.AccessToken);

    var response = await httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();

    using var graphResponseJson = JsonDocument.Parse(await response.Content.ReadAsStreamAsync());
    Console.WriteLine(JsonSerializer.Serialize(value: graphResponseJson, options: jsonSerializerOptions));
}