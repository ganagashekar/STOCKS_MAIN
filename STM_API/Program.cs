using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Options;
using STM_API.Hubs;
using STM_API.Services;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

//var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
//store.Open(OpenFlags.ReadOnly);
//var certificate = store.Certificates.OfType<X509Certificate2>()
//    .First(c => c.FriendlyName == "STOCK");

//var file = "localhost.pfx";
//var certificate = new X509Certificate2(File.ReadAllBytes("localhost.pfx"), "YourSecurePassword");

var builder = WebApplication.CreateBuilder(args);

//var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions
//{
//    SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
//    ClientCertificateMode = ClientCertificateMode.AllowCertificate,
//    ServerCertificate = new X509Certificate2("./certificate.pfx", "password")

//};


//builder.WebHost.UseKestrel(options =>
//{
//    options.Listen(System.Net.IPAddress.Loopback, 5001, listenOptions =>
//    {
//        var connectionOptions = new HttpsConnectionAdapterOptions();
//        connectionOptions.ServerCertificate = certificate;

//        listenOptions.UseHttps(connectionOptions);
//    });
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<StockTicker>();
builder.Services.AddScoped<BreezapiServices>();
builder.Services.Configure<JsonOptions>(options =>
{
    // Set this to true to ignore null or default values
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});
//builder.Services.ConfigureHttpJsonOptions(options => {
//    options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
//});
//builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
//{
//    builder
//        .AllowAnyMethod()
//        .AllowAnyHeader()
//        .WithOrigins("http://localhost:4200", "http://localhost:4200", "http://localhost", "https://localhost", "https://localhost/StockSignalRServer", "https://localhost/StockSignalRServer");
//}));
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.MaximumParallelInvocationsPerClient = 1000000;
    hubOptions.StreamBufferCapacity = 1024000000;
    hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(180);
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(180);
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(180);
    hubOptions.MaximumReceiveMessageSize = 1024000000;




});



builder.Host.UseWindowsService();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseCors(x => x.SetIsOriginAllowed(x => x.StartsWith("http://localhost:4200/")));
//app.UseCors(x => x.AllowAnyHeader().WithOrigins("http://localhost:4200", "http://localhost:4200", "http://localhost", "https://localhost", "https://localhost/StockSignalRServer", "https://localhost/StockSignalRServer").AllowAnyMethod());
//app.UseCors(x => x.AllowAnyOrigin());
//app.UseHttpsRedirection();



//app.UseAuthorization();
//app.UseEndpoints(endpoints => endpoints.MapHub<ICICIDirectHUB>("/livefeedhub")); // Restore this);
app.MapHub<ICICIDirectHUB>("/livefeedhub");
app.MapHub<BreezeOperationHUB>("/breezeoperation");
//    , options =>
//{
//    options.Transports =
//        HttpTransportType.WebSockets;

//});
app.MapControllers();

app.Run();
