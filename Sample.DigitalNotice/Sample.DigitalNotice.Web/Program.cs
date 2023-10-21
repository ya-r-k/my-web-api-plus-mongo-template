using Sample.DigitalNotice.Common.Infrastructure;
using Sample.DigitalNotice.Di.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var isRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var result) && result;
var configuration = builder.Configuration;

// Configuring logging.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger());

if (isRunningInContainer)
{
    builder.WebHost.ConfigureKestrel((context, serverOptions) =>
    {
        serverOptions.ListenAnyIP(443, listenOptions =>
        {
            listenOptions.UseHttps(httpsOptions =>
            {
                var localhostCert = new X509Certificate2(configuration["Certificates:Localhost:Path"], configuration["Certificates:Localhost:Password"]);
                var remoteCert = new X509Certificate2(configuration["Certificates:Remote:Path"], configuration["Certificates:Remote:Password"]);

                var certs = new Dictionary<string, X509Certificate2>(StringComparer.OrdinalIgnoreCase)
                {
                    ["localhost"] = localhostCert,
                    ["sample.digitalnotice"] = remoteCert,
                };

                httpsOptions.ServerCertificateSelector = (connectionContext, name) =>
                {
                    if (name is not null && certs.TryGetValue(name, out var cert))
                    {
                        return cert;
                    }

                    return localhostCert;
                };
            });
        });
    });
}

// Configure response caching
builder.Services.AddResponseCaching();

// Configure connection to database
var connectionString = isRunningInContainer
    ? configuration.GetConnectionString("Docker")
    : configuration.GetConnectionString("Default");

builder.Services.AddSingleton(Options.Create(new MongoDbSettings
{
    ConnectionString = connectionString,
    DatabaseName = configuration["DatabaseCredentials:DatabaseName"],
}));
builder.Services.AddHealthChecks()
    .AddMongoDb(connectionString, timeout: TimeSpan.FromSeconds(5))
    .AddCheck("example", () => HealthCheckResult.Healthy("Example check is healthy"), new[] { "example" });

// Add services to the container.
builder.Services.AddServices();
builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sample digital notice",
        Description = "Description about sample digital notice",
        Version = "1.0",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    setup.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample digital notice V1");
    });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Configure Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

app.UseStaticFiles();
app.UseResponseCaching();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");

    endpoints.MapMetrics();
    endpoints.MapHealthChecks("/health");
});

app.MapFallbackToFile("index.html");

app.Run();
