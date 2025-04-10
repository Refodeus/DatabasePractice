using DatabasePractice.Models;

namespace DatabasePractice.Interfaces
{
    public interface IClientsRepository
    {
        ICollection<Client> GetClientsFiltered(string searchFilter);
        bool CreateClient(Client client);
        bool UpdateClient(Client client);
        bool DeleteClient(int id);
        bool isExist(int id);
        bool Save();

    }
}
