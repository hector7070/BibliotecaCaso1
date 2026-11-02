using System.Text.Json.Serialization;
using BibliotecaCaso1.Errores;
using DA;
using Flujo;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorExcepcionesGlobal>();

builder.Services.AddOpenApi();

var cadenaConexion = builder.Configuration.GetConnectionString("Biblioteca")
    ?? throw new InvalidOperationException("Falta ConnectionStrings:Biblioteca en appsettings.json.");

builder.Services.AddDatos(cadenaConexion);
builder.Services.AddFlujo();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
