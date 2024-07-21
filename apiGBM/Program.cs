

using apiGBM.Infra;
using apiGBM.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(a =>
{
    a.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "apiGBM",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Murilo Amaral",
            Email = "muriloalessioamaral@gmail.com",
            Url = new Uri("https://github.com/Murilo013")   
        }
    });

    var xmlFile = "apiGBM.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    a.IncludeXmlComments(xmlPath);
});

builder.Services.AddTransient<IMotoristaRepository, MotoristaRepository>();  // INJEÇÃO DE DEPENDENCIA, (PARA QUE A INTERFACE IMPLEMENTE A CLASSE)
builder.Services.AddTransient<ICaminhaoRepository, CaminhaoRepository>();
builder.Services.AddTransient<IEntregaRepository, EntregaRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
