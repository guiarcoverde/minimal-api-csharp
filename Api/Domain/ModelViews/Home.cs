namespace MinimalApi.Domain.ModelViews;

public struct Home
{
    public string Message
    {
        get => "Welcome to the Vehicle API - Minimal API Example";
    }
    public string Doc { get => "/swagger"; }
}