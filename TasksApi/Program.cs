using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Adds Microsoft Identity platform (Azure AD B2C) support to protect this Api
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind(key: "AzureAdB2C", instance: options);
        options.TokenValidationParameters.NameClaimType = "name";
    }, options => { builder.Configuration.Bind(key: "AzureAdB2C", instance: options); });

builder.Services.Configure<OpenIdConnectOptions>(
    name: OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        // The claim in the Jwt token where App roles are available.
        options.TokenValidationParameters.RoleClaimType = "roles";
        options.TokenValidationParameters.NameClaimType = "name";
    });


builder.Services.AddAuthorization(policies =>
{
    policies.AddPolicy(name: "read", policy => { policy.RequireRole("Task.Read"); });
    policies.AddPolicy(name: "write", p => { p.RequireRole("Task.Write"); });
});

// End of the Microsoft Identity platform block 


var app = builder.Build();

app.UseCors()
    .UseAuthentication()
    .UseAuthorization();

app.UseHttpsRedirection();

app.MapGet(pattern: "/tasks", () => { return Store.Tasks.Select(task => new Task(task)); })
    .RequireAuthorization(policyNames: ["read"]);

app.MapPost(pattern: "/tasks", ([FromBody] Task task) =>
{
    Console.WriteLine($"Adding task: {task.Description}");
    Store.Tasks.Add(task.Description);
    return Results.Created(uri: $"/tasks/{task.Description}", value: task);
}).RequireAuthorization(policyNames: ["read", "write"]);

app.Run();

internal record Task(string Description);

internal static class Store
{
    internal static readonly List<string> Tasks =
    [
        "Do the laundry",
        "Clean the house",
        "Buy groceries",
        "Walk the dog",
        "Cook dinner"
    ];
}