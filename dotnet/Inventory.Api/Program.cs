using Inventory.Api.Authentication;
using Inventory.Api.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Cosmos");
builder.Services.UseCosmos(connectionString, "Inventory", new()
{
    SerializerOptions = new() { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase }
});
builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddAuthentication("BasicAuth")
    .AddScheme<AuthenticationSchemeOptions, AuthHandler>("BasicAuth", null)
    .AddCertificate();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
