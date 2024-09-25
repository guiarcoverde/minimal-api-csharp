namespace MinimalApi.Domain.Dto;

public record Vehicle
{
    public string Name { get; set; } = default!;
    public string CarBrand { get; set; } = default!;
    public int Year { get; set; } = default!;
}