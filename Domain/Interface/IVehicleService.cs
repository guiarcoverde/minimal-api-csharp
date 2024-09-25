using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interface;

public interface IVehicleService
{
    List<Vehicle> GetAll(int? page = 1, string? nome = null, string? marca = null);
    Vehicle? GetById(long id);
    Vehicle Add(Vehicle vehicle);
    Vehicle Update(long id, Vehicle vehicle);
    Vehicle Delete(long id);

}