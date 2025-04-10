using DatabasePractice.DbContexts;
using DatabasePractice.Interfaces;
using DatabasePractice.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DatabasePractice.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrdersRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public bool CreateOrder(Order order)
        {
            _appDbContext.Orders.Add(order);
            return Save();
        }

        public bool DeleteOrder(int id)
        {
            var order = _appDbContext.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
                _appDbContext.Orders.Remove(order);
            return Save();
        }
        public ICollection<Order> GetOrdersFiltered(string searchFilter)
        {
            var query = _appDbContext.Orders.Include(o => o.КлиентNavigation).AsNoTracking();
            if (string.IsNullOrWhiteSpace(searchFilter))
                return query.ToList();
            if (DateOnly.TryParseExact(searchFilter, new[] { "dd.MM.yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "dd/MM/yyyy" },
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dateFilter))
            {
                DateTime startDate = dateFilter.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
                DateTime endDate = dateFilter.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
                query = query.Where(o => o.ДатаИВремя >= startDate && o.ДатаИВремя <= endDate);
            }
            else if (decimal.TryParse(searchFilter, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal sum))
                query = query.Where(o => o.Сумма == sum);
            else
                query = query.Where(o => o.КлиентNavigation != null &&
                    (o.КлиентNavigation.Имя.Contains(searchFilter) ||
                     o.КлиентNavigation.Фамилия.Contains(searchFilter)) ||
                     o.Статус.Contains(searchFilter));
            return query.ToList();
        }
        public IEnumerable<GetAvgCheck> GetAvgChecks()
        {
            return _appDbContext.Set<GetAvgCheck>().FromSqlRaw("SELECT * FROM get_avg_check()").ToList();
        }
        public IEnumerable<GetPurchasesOfBirthday> GetPurchasesOfBirthday()
        {
            return _appDbContext.Set<GetPurchasesOfBirthday>().FromSqlRaw("SELECT * FROM get_purchases_of_birthday()").ToList();
        }
        public bool isExist(int id)
        {
            return _appDbContext.Orders.Where(p => p.Id == id).Any();
        }

        public bool Save()
        {
            var saved = _appDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOrder(Order order)
        {
            _appDbContext.Orders.Update(order);
            return Save();
        }
    }
}
