using System.Net;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using System.Net.WebSockets;

using Bytewizer.Huxley.Api.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HuxleyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HuxleyContext")));

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseWebSockets();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/ws", async (HttpContext context, HuxleyContext db) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
        {
            while (true)
            {
                var devices = await db.Devices.ToListAsync();
                if (devices.Any())
                {
                    var feature = new List<Feature>();
                    foreach (var device in devices)
                    {
                        var properties = new Properties(device.DeviceID);
                        properties.Location = device.Location;
                        properties.Timestamp = device.Timestamp;

                        var geometry = new Geometry(device.Latitude, device.Longitude);

                        feature.Add(new Feature(properties, geometry));
                        
                        var geoObject = new GeoModel(feature);

                        var geojson = JsonSerializer.Serialize(geoObject);
                        Debug.WriteLine(geojson);

                        await webSocket.SendAsync(
                                Encoding.ASCII.GetBytes(geojson),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None
                            );
                    }
                }

                await Task.Delay(3000);
            }
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

app.MapPost("/notehub",
[AllowAnonymous] async ([FromBody] DeviceModel body, [FromServices] HuxleyContext db, HttpResponse response) =>
{
    var device = db.Devices.SingleOrDefault(s => s.DeviceID == body.DeviceID);

    if (device == null)
    {
        db.Devices.Add(body);
        response.StatusCode = 200;
        response.Headers.Location = $"/{body.DeviceID}";
    }
    else
    {
        device.Latitude = body.Latitude;
        device.Longitude = body.Longitude;
        device.Location = body.Location;
        device.Timestamp = body.Timestamp;
    }

    await db.SaveChangesAsync();
})
.Accepts<DeviceModel>("application/json")
.Produces<DeviceModel>(StatusCodes.Status200OK)
.WithName("AddOrUpdateDevice").WithTags("Setters");

app.Run();