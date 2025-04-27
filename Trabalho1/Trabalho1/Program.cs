using Microsoft.EntityFrameworkCore;
using Trabalho1.Data; // <-- importa seu AppDbContext

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços à aplicação
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar o banco de dados (exemplo com SQLite, mas ajusta se quiser outro)
// Você precisa adicionar o pacote Microsoft.EntityFrameworkCore.Sqlite se for usar SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configurar o pipeline de requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Ativa seus Controllers

app.Run();
