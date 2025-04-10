using DatabasePractice.DbContexts;
using DatabasePractice.Interfaces;
using DatabasePractice.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DatabasePractice.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly AppDbContext _appDbContext;
        public ClientsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public bool CreateClient(Client client)
        {
            _appDbContext.Clients.Add(client);
            return Save();
        }

        public bool DeleteClient(int id)
        {
            var client = _appDbContext.Clients.FirstOrDefault(p => p.Id == id);
            if (client != null) 
                _appDbContext.Clients.Remove(client);
            return Save();
        }

        public ICollection<Client> GetClientsFiltered(string searchFilter)
        {
            var clients = _appDbContext.Clients.ToList();
            if (string.IsNullOrWhiteSpace(searchFilter))
                return clients;
            if (DateOnly.TryParseExact(searchFilter, new[] { "dd.MM.yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "yyyy-MM-dd", "yyyy.MM.dd" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dateFilter))
                clients = clients.Where(c => c.ДатаРождения == dateFilter).ToList();
            else
                clients = clients.Where(c => c.Имя.Contains(searchFilter, StringComparison.OrdinalIgnoreCase) ||
                    c.Фамилия.Contains(searchFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            return clients;
        }

        public bool isExist(int id)
        {
            return _appDbContext.Clients.Where(p => p.Id == id).Any();
        }

        public bool Save()
        {
            var saved = _appDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateClient(Client client)
        {
            _appDbContext.Clients.Update(client);
            return Save();
        }
    }
}
