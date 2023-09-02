using Sample.DigitalNotice.Common.Infrastructure;
using Sample.DigitalNotice.Di.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Configuring logging.

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Information)
    .WriteTo.Console(outputTemplate:
        "{Timestamp:yyyy-MM-dd HH:mm:ss} {EnvironmentName} [{Level:u3}] (Process: {ProcessId}, Thread: {ThreadId}) {RequestPath} {StatusCode} {StatusCodeDescription} {Elapsed:0.000}ms {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("C:/Logs/Samples/DigitalNoticeMongo/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {EnvironmentName} [{Level:u3}] {RequestPath} {StatusCode} {StatusCodeDescription} {Elapsed:0.000}ms {Message:lj}{NewLine}{Exception}")
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["AnalyticsServiceOptions:Default:ConnectionString"]))
    {
        AutoRegisterTemplate = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        IndexFormat = $"sample-digital-notice-{{0:yyyy.MM.dd}}",
    })
    .CreateLogger());

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps(httpsOptions =>
        {
            var localhostCert = new X509Certificate2("/root/.aspnet/https/sample-digitalnotice-https-local.pfx", "JF(E@&$g78367GF7dtt23^@7eGydet^Ey7etd75eTQ5t");
            //var remoteCert = new X509Certificate2("/root/.aspnet/https/sample-digitalnotice-https-remote.pfx", "JF(E@&$g78367GF7dtt23^@7eGydet^Ey7etd75eTQ5t");

            var certs = new Dictionary<string, X509Certificate2>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["localhost"] = localhostCert,
                //["sample-digitalnotice"] = remoteCert,
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

// Configure response caching
builder.Services.AddResponseCaching();

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration["DatabaseConnectionOptions:Default:ConnectionString"], timeout: TimeSpan.FromSeconds(5))
    .AddCheck("example", () => HealthCheckResult.Healthy("Example check is healthy"), new[] { "example" });

// Add services to the container.

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("DatabaseConnectionOptions:Default"));
builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Digital notice",
        Description = "Description about digital notice",
        Version = "1.0",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    setup.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital notice V1");
    });
}

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseSerilogRequestLogging();

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
