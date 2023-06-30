using ProjetoIntegrador.Modulo1.BackEnd.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppSettings();
builder.AddConnectionString();
builder.AddServices();
builder.Services.Configure<RouteOptions>(option => option.LowercaseUrls = true);
builder.Services
    .AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCookieAuthentication();

var policyName = builder.AllowCors();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(policyName);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
