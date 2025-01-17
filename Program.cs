using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Data;
using ProjetoIntegrador.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços necessários para APIs
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o HttpClient e o serviço FactorialService
builder.Services.AddHttpClient(); // Registra o HttpClient
builder.Services.AddScoped<FactorialService>(); // Registra o serviço

var app = builder.Build();

// Teste de conexão com a API Factorial
using (var scope = app.Services.CreateScope())
{
    var factorialService = scope.ServiceProvider.GetRequiredService<FactorialService>();
    await factorialService.TestarConexaoAsync(); // Chama o teste de conexão
}

// Configurar para usar controladores
app.MapControllers();

// Configurar a rota padrão para redirecionar para o HomeController
app.MapGet("/", () => Results.Redirect("/Home"));

app.Run();
