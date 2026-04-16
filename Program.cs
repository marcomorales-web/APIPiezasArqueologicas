using ApiPiezasArqueologicas.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var rutaJson = Path.Combine(AppContext.BaseDirectory, "Data", "piezas.json");
var json = File.ReadAllText(rutaJson);

var opciones = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

var piezas = JsonSerializer.Deserialize<List<Pieza>>(json, opciones) ?? new List<Pieza>();

app.MapGet("/", () => "API de piezas arqueológicas");

app.MapGet("/piezas", () =>
{
    return Results.Ok(piezas);
});

app.MapGet("/piezas/{id:int}", (int id) =>
{
    var pieza = piezas.FirstOrDefault(p => p.Id == id);

    return pieza is not null
        ? Results.Ok(pieza)
        : Results.NotFound(new { mensaje = "Pieza no encontrada" });
});


app.MapGet("/piezas/buscar", (string texto) =>
{
    var resultado = piezas
        .Where(p =>
            p.Titulo.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            p.Descripcion.Contains(texto, StringComparison.OrdinalIgnoreCase))
        .ToList();

    return Results.Ok(resultado);
});

app.MapGet("/api/abecedario", (IWebHostEnvironment env) =>
{
    string rutaArchivo = Path.Combine(env.ContentRootPath, "Data", "abecedario.json");

    if (!File.Exists(rutaArchivo))
    {
        return Results.NotFound(new { mensaje = "No se encontró el archivo abecedario.json" });
    }

    string json = File.ReadAllText(rutaArchivo);

    var opciones = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    var palabras = JsonSerializer.Deserialize<List<PalabraAbecedario>>(json, opciones);

    return Results.Ok(palabras);
});

app.MapGet("/api/abecedario/{id:int}", (int id, IWebHostEnvironment env) =>
{
    string rutaArchivo = Path.Combine(env.ContentRootPath, "Data", "abecedario.json");

    if (!File.Exists(rutaArchivo))
    {
        return Results.NotFound(new { mensaje = "No se encontró el archivo abecedario.json" });
    }

    string json = File.ReadAllText(rutaArchivo);

    var opciones = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    var palabras = JsonSerializer.Deserialize<List<PalabraAbecedario>>(json, opciones) ?? new List<PalabraAbecedario>();

    var palabra = palabras.FirstOrDefault(p => p.Id == id);

    if (palabra == null)
    {
        return Results.NotFound(new { mensaje = "Palabra no encontrada" });
    }

    return Results.Ok(palabra);
});

app.Run();