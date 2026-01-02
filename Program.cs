using ProjetoApi.Models;
using ProjetoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVIÇOS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurações do MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Registro do nosso serviço de lógica
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

// 2. PIPELINE (A ordem aqui evita o 404)

// Forçamos o Swagger a aparecer sempre, ignorando o "if Development"
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Usamos o caminho relativo para evitar erros no Docker
    c.SwaggerEndpoint("v1/swagger.json", "Minha API v1");
    c.RoutePrefix = "swagger"; 
});

// Comente esta linha para evitar erros de SSL/HTTPS no Docker por enquanto
// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Rota de teste que já sabemos que funciona
app.MapGet("/ping", () => "A API está viva!");

app.MapControllers();

app.Run();