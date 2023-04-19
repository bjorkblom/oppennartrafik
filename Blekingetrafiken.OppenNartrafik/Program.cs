using Blekingetrafiken.OppenNartrafik.Services;
using System.Reflection;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var kmlDocument = new XmlDocument();
if (builder.Configuration["PolygonFile"] != null)
{
    var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    var filePath = Path.Combine(assemblyPath, builder.Configuration["PolygonFile"]);
    kmlDocument.Load(filePath);
}

builder.Services.AddSingleton<IPolygonService>(provider => new PolygonService(provider.GetService<ILogger>(), kmlDocument));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Öppen Närtrafik");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
