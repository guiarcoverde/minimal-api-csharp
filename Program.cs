using MinimalApi.Domain.Dto;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDTO loginDto) =>
{
    return loginDto is { Email: "adm@teste.com", Senha: "123456" } ? Results.Ok("Login Com sucesso") : Results.Unauthorized();
});


app.Run();


