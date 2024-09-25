using MinimalApi.Domain.Enum;

namespace MinimalApi.Domain.ModelViews;

public record Administrator
{
    public long Id { get; set; }
    public string Email { get; set; } = default!;
    public string Profile { get; set; } = default!;
}