using Microsoft.EntityFrameworkCore;
using Trabalho1.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar serviços dos controllers
builder.Services.AddControllers();

// Adicionar serviços do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    // Swagger no ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI();

    // No ambiente de desenvolvimento, não precisa redirecionar para HTTPS
}
else
{
    // Em produção, redirecionar para HTTPS
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

