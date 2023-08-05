using Microsoft.AspNetCore.Authentication.Negotiate;
using Turmerik.Infrastucture;
using Turmerik.LocalDevice.Core.Dependencies;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Dependencies;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

var configBuilder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
var configuration = configBuilder.Build();

AppServiceCollectionBuilder.RegisterAll(
    builder.Services,
    configuration.GetRequiredSection(TurmerikPrefixes.TURMERIK),
    out IClientAppSettingsService clientAppSettingsService,
    out IAppSettingsService appSettingsService);

var clientAppSettings = clientAppSettingsService.ClientAppSettings;
var appSettings = appSettingsService.AppSettings;

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                appSettings.ClientAppHosts.ToArray());
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
