using BlazorCRUDApp.Server.Models;
using BlazorCRUDApp.Server.Repository;

namespace BlazorCRUDApp.Server.Services
{
    public class PersonService : IPersonService
    {
        private readonly Scoring _person;
        public PersonService(Scoring person)
        {
            _person = person;
        }
        public async Task<Scoring> AddPerson(Scoring person)
        {
            return new Scoring();
            //return await _person.CreateAsync(person);
        }

        public async Task<bool> UpdatePerson(int id, Scoring person) 
        {
            //var data = await _person.GetByIdAsync(id);

            //if (data != null)
            //{  
            //    data.FirstName = person.FirstName;
            //    data.LastName = person.LastName;
            //    data.Email = person.Email;
            //    data.MobileNo = person.MobileNo;

            //    await _person.UpdateAsync(data);
            //    return true;
            //}
            //else
            //    return false;
            return false;
        }

        public async Task<bool> DeletePerson(int id)
        {
            //await _person.DeleteAsync(id);
            return true;
        }

        public async Task<List<Scoring>> GetAllPersons()
        {
            List<Scoring> list = new List<Scoring>();
            list.Add(new Scoring() { Id =1, FirstName = "h" });
            return list;
            //return await _person.GetAllAsync();
        }

        public async Task<Scoring> GetPerson(int id)
        {
            return new Scoring();
            //return await _person.GetByIdAsync(id);
        }
    }
}
