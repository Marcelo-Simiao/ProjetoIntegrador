using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os necess�rios para APIs
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configurar para usar controladores
app.MapControllers();

app.Run();
