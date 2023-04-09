using BlazorCRUDApp.Server.Models;

namespace BlazorCRUDApp.Server.Services
{
    public interface IPersonService
    {
        Task<Scoring> AddPerson(Scoring person);

        Task<bool> UpdatePerson(int id, Scoring person);

        Task<bool> DeletePerson(int id);

        Task<List<Scoring>> GetAllPersons();

        Task<Scoring> GetPerson(int id);

    }
}
