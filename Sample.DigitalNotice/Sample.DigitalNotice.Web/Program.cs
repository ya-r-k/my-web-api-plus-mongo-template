using Sample.DigitalNotice.Common.Infrastructure;
using Sample.DigitalNotice.Di.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Configuring logging.

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Information)
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {ThreadId}] ({Level:u3}) {RequestPath} {StatusCode} {StatusCodeDescription} {Elapsed:0.000}ms {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("C:/Logs/Samples/DigitalNoticeMongo/log-.txt", 
        rollingInterval: RollingInterval.Day, 
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] ({Level:u3}) {RequestPath} {StatusCode} {StatusCodeDescription} {Elapsed:0.000}ms {Message:lj}{NewLine}{Exception}")
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
    {
        AutoRegisterTemplate = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        IndexFormat = $"sample-digital-notice-{{0:yyyy.MM.dd}}",
    })
    .CreateLogger());

// Configure response caching
builder.Services.AddResponseCaching();

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration["MongoDbSettings:ConnectionString"], timeout: TimeSpan.FromSeconds(5))
    .AddCheck("example", () => HealthCheckResult.Healthy("Example check is healthy"), new[] { "example" });

// Add services to the container.

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
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

app.UseSerilogRequestLogging();

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
