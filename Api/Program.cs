using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Domain.Dto;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;
using MinimalApi.Validators;
using Administrator = MinimalApi.Domain.Dto.Administrator;


#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key))
{
    key = "123456";
}

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "The token needs to be passed this way: Bearer {your token}"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")));
});


var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Admins

string GenerateJwtToken(MinimalApi.Domain.Entities.Administrator administrator)
{
    if(string.IsNullOrEmpty(key)) return string.Empty;
    
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", administrator.Email),
        new Claim(ClaimTypes.Role, administrator.Profile),
        new Claim("Profile", administrator.Profile),
    };

    var token = new JwtSecurityToken(claims: claims,expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}


app.MapPost("/admins/login", ([FromBody]Login loginDto, [FromServices]IAdministratorService administratorService) =>
{
    var admin = administratorService.Login(loginDto);
    
    if (admin is null) return Results.Unauthorized();
    
    string token = GenerateJwtToken(admin);
    return Results.Ok(new LoggedAdministrator
    {
        Email = admin.Email,
        Profile = admin.Profile,
        Token = token
    });

}).AllowAnonymous().WithTags("Administrators");

app.MapPost("/admin", ([FromBody] Administrator adminDto, [FromServices]IAdministratorService administratorService) =>
{
    var validate = AdminRequestValidation.ValidateCreateAdminRequest(adminDto);

    if (validate.Messages.Count > 0)
        return Results.BadRequest(validate);

    var admin = new MinimalApi.Domain.Entities.Administrator
    {
        Email = adminDto.Email,
        Password = adminDto.Password,
        Profile = adminDto.Profile.ToString()
    };

    var result = administratorService.Add(admin);
    return Results.Created($"/admin/{result.Id}", new MinimalApi.Domain.ModelViews.Administrator
    {
        Id = result.Id,
        Email = result.Email,
        Profile = result.Profile
    });

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute {Roles = "Admin"}).WithTags("Administrators");

app.MapGet("/admin", ([FromQuery] int? page, [FromServices] IAdministratorService administratorService) =>
{
    var admModelView = new List<MinimalApi.Domain.ModelViews.Administrator>();
    var admins = administratorService.GetAll(page);
    foreach (var adm in admins)
    {
        admModelView.Add(new MinimalApi.Domain.ModelViews.Administrator
        {
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile
        });
    }

    return admins.Count > 1 ? Results.Ok(admModelView) : Results.NoContent();
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute {Roles = "Admin"}).WithTags("Administrators");

app.MapGet("/admin/{id:long}", ([FromRoute] long id, [FromServices] IAdministratorService administratorService) =>
{
    var admin = administratorService.GetById(id);
    return admin != null ? Results.Ok(new MinimalApi.Domain.ModelViews.Administrator
    {
        Id = admin.Id,
        Email = admin.Email,
        Profile = admin.Profile
    }) : Results.NoContent();
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute {Roles = "Admin"}).WithTags("Administrators");


#endregion

#region Vehicles

app.MapPost("/vehicles", ([FromBody]Vehicle vehicleDto, [FromServices]IVehicleService vehicleService) =>
{
    var validate = VehicleRequestValidation.ValidateDtoAddVehicle(vehicleDto);

    if (validate.Messages.Count > 0)
    {
        return Results.BadRequest(validate);
    }
    
    var vehicle = new MinimalApi.Domain.Entities.Vehicle
    {
        Name = vehicleDto.Name,
        Year = vehicleDto.Year,
        CarBrand = vehicleDto.CarBrand
    };
    
    vehicleService.Add(vehicle);

    return Results.Created($"/veiculo/{vehicle.Id}", vehicle);
}).RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute {Roles = "Admin, Editor"})
    .WithTags("Vehicles");

app.MapGet("/vehicles", ([FromQuery]int? page, IVehicleService vehicleService) =>
{
    var vehicles = vehicleService.GetAll(page);
    return Results.Ok(vehicles);
}).RequireAuthorization().WithTags("Vehicles");

app.MapGet("/vehicles/{id:long}", ([FromRoute]long id, [FromServices]IVehicleService vehicleService) =>
{
    var vehicles = vehicleService.GetById(id);

    if (vehicles is null)
    {
        return Results.NoContent();
    }
    
    return Results.Ok(vehicles);
}).RequireAuthorization().WithTags("Vehicles");

app.MapPut("/vehicles/{id:long}", ([FromRoute]long id, [FromBody] Vehicle request,[FromServices]IVehicleService vehicleService) =>
{
    var validate = VehicleRequestValidation.ValidateDtoUpdateVehicle(request);

    if (validate.Messages.Count > 0)
    {
        return Results.BadRequest(validate);
    }
    
    var vehicle = vehicleService.GetById(id);

    if (vehicle is null)
        return Results.NotFound();

    vehicle.Name = request.Name;
    vehicle.Year = request.Year;
    vehicle.CarBrand = request.CarBrand;
    
    var result = vehicleService.Update(id, vehicle);
    return Results.Ok(result);

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute {Roles = "Admin"}).WithTags("Vehicles");

app.MapDelete("/vehicles/{id:long}", ([FromRoute]long id,[FromServices]IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetById(id);

    if (vehicle is null)
        return Results.NotFound();
    
    var result = vehicleService.Delete(id);
    return Results.Ok(result);

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute {Roles = "Admin"}).WithTags("Vehicles");

#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion


