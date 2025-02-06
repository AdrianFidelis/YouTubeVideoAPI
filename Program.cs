using Microsoft.EntityFrameworkCore;
using YouTubeVideoAPI.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")));

// Configuração do
// gger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "YouTube Video API", Version = "v1" });
});

// Configuração de controllers
builder.Services.AddControllers();

var app = builder.Build();

// Habilita o Swagger apenas no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "YouTube Video API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapeia os controllers
app.MapControllers();

// Inicia a aplicação
app.Run();
