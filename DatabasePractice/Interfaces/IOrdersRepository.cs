using DatabasePractice.Models;

namespace DatabasePractice.Interfaces
{
    public interface IOrdersRepository
    {
        ICollection<Order> GetOrdersFiltered(string searchFilter);
        IEnumerable<GetAvgCheck> GetAvgChecks();
        IEnumerable<GetPurchasesOfBirthday> GetPurchasesOfBirthday();
        bool CreateOrder(Order order);
        bool UpdateOrder(Order order);
        bool DeleteOrder(int id);
        bool isExist(int id);
        bool Save();

    }
}
