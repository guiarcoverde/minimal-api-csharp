namespace MinimalApi.Domain.Dto;

public record Login
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
