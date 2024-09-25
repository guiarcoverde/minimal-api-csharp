using MinimalApi.Domain.Dto;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interface;

public interface IAdministratorService
{
    Entities.Administrator? Login(Login login);

    List<Entities.Administrator> GetAll(int? page = 1);
    Entities.Administrator? GetById(long id);
    Entities.Administrator Add(Entities.Administrator request);
    Entities.Administrator Update(long id, Entities.Administrator request);
    Entities.Administrator Delete(long id);
}