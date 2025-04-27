using Microsoft.EntityFrameworkCore;
using Trabalho1.Data;
using Trabalho1.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço do Entity Framework com banco SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona os serviços dos Controllers
builder.Services.AddControllers();

// Adiciona o serviço do Swagger (opcional, para testar APIs)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Cria o banco de dados automaticamente na primeira execução
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configura o Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de roteamento
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
