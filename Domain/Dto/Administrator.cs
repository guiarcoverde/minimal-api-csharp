using MinimalApi.Domain.Enum;

namespace MinimalApi.Domain.Dto;

public class Administrator
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public EProfile Profile { get; set; } = default!;
}