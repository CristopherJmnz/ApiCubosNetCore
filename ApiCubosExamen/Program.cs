using ApiCubosExamen.Data;
using ApiCubosExamen.Helpers;
using ApiCubosExamen.Respositories;
using ApiCubosExamen.Services;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//Configuracion de keyvaults
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient =
builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret secretConnectionString =
    await secretClient.GetSecretAsync("SqlAzure");

//config DB connection
string connectionString = secretConnectionString.Value;
string azurekeys = builder.Configuration.GetValue<string>("AzureKeys:StorageAccount");
BlobServiceClient blobServiceClient = new BlobServiceClient(azurekeys);
builder.Services.AddTransient<StorageBlobsService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<CubosRepository>();
//string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddDbContext<CubosContext>(options => options.UseSqlServer(connectionString));

//Inyeccion del helper
HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);


//Configuracion del JWT
builder.Services.AddAuthentication
    (helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());


//CONFIG SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "Api cubos cris",
        Description = "Api"
    });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        url: "/swagger/v1/swagger.json",
        name: "Api v1"
        );
    options.RoutePrefix = "";
});
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();