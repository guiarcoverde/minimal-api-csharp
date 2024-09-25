using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interface;
using MinimalApi.Infrastructure.Db;

namespace MinimalApi.Domain.Services;

public class VehicleService(AppDbContext context) : IVehicleService
{
    private readonly AppDbContext _context = context;
    
    
    public List<Vehicle> GetAll(int? page = 1, string? Name = null, string? CarBrand = null)
    {
        var query = _context.Vehicles.AsQueryable();
        
        if (!string.IsNullOrEmpty(Name))
        {
            query = query.Where(v => v.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase));
            
        } 
        else if (!string.IsNullOrEmpty(CarBrand))
        {
            query = query.Where(v => v.CarBrand.Equals(CarBrand, StringComparison.CurrentCultureIgnoreCase));
        }

        int itemsPerPage = 10;

        if (page is not null)
        {
            query = query.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage);
        }
        
        return query.ToList();
    }

    public Vehicle? GetById(long id) =>
        _context.Vehicles.FirstOrDefault(v => v.Id == id);

    public Vehicle Add(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        _context.SaveChanges();

        return vehicle;
    }

    public Vehicle Update(long id, Vehicle vehicle)
    {
        var vehicleSearch = _context.Vehicles.FirstOrDefault(v => v.Id == id);

        if (vehicleSearch is null)
            return new Vehicle();

        vehicleSearch.Name = vehicle.Name;
        vehicleSearch.Year = vehicle.Year;
        vehicleSearch.CarBrand = vehicle.CarBrand;

        _context.Vehicles.Update(vehicleSearch);
        _context.SaveChanges();

        return vehicleSearch;
    }

    public Vehicle Delete(long id)
    {
        var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id);

        if (vehicle is null)
            return new Vehicle();

        _context.Vehicles.Remove(vehicle);
        _context.SaveChanges();

        return vehicle;

    }
}