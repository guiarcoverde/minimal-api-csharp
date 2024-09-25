using MinimalApi.Domain.Dto;
using MinimalApi.Domain.ModelViews;

namespace MinimalApi.Validators;

public static class VehicleRequestValidation
{
    public static ValidationErrors ValidateDtoAddVehicle(Vehicle vehicle)
    {
        var validator = new ValidationErrors()
        {
            Messages = []
        };

        if (string.IsNullOrEmpty(vehicle.Name))
        {
            validator.Messages.Add("Name can not be empty.");
        }
    
        if (string.IsNullOrEmpty(vehicle.CarBrand))
        {
            validator.Messages.Add("Car brand can not be empty.");
        }
    
        if (vehicle.Year <= 1950)
        {
            validator.Messages.Add("Only vehicles produced after 1950 can be added.");
        }

        return validator;
    }
    
    public static ValidationErrors ValidateDtoUpdateVehicle(Vehicle vehicle)
    {
        var validator = new ValidationErrors()
        {
            Messages = []
        };

        if (string.IsNullOrEmpty(vehicle.Name))
        {
            validator.Messages.Add("Name can not be empty.");
        }
    
        if (string.IsNullOrEmpty(vehicle.CarBrand))
        {
            validator.Messages.Add("Car brand can not be empty.");
        }
    
        if (vehicle.Year <= 1950)
        {
            validator.Messages.Add("You can not change the vehicle year to 1950 or before.");
        }

        return validator;
    }
}