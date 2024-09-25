using MinimalApi.Domain.Dto;
using MinimalApi.Domain.Interface;
using MinimalApi.Infrastructure.Db;
using Administrator = MinimalApi.Domain.Entities.Administrator;

namespace MinimalApi.Domain.Services;

public class AdministratorService(AppDbContext context) : IAdministratorService
{
    private readonly AppDbContext _context = context;
    
    public Administrator? Login(Login login)
    {
        var admin = _context.Administrators
            .FirstOrDefault(a => a.Email == login.Email && a.Password == login.Password);

        return admin;
    }

    public List<Administrator> GetAll(int? page)
    {
        var query = _context.Administrators.AsQueryable();
        int itemsPerPage = 10;

        if (page is not null)
        {
            query = query.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage);
        }
        
        return query.ToList();
    }

    public Administrator? GetById(long id) => _context.Administrators.FirstOrDefault(a => a.Id == id);

    public Administrator Add(Administrator request)
    {
        var result = _context.Administrators.Add(request);
        _context.SaveChanges();

        return result.Entity;
    }

    public Administrator Update(long id, Administrator request)
    {
        throw new NotImplementedException();
    }

    public Administrator Delete(long id)
    {
        throw new NotImplementedException();
    }
}