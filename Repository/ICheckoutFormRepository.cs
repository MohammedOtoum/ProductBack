using ProductTask.Dto;
using ProductTask.Model;

namespace ProductTask.Repository
{
    public interface ICheckoutFormRepository
    {
        Task<IEnumerable<CheckoutForm>> GetAllAsync();
        Task<CheckoutForm?> GetByIdAsync(int id);
        Task<CheckoutForm> AddAsync(CheckoutForm form);
        Task<CheckoutForm?> UpdateAsync(int id, CheckoutForm updatedForm);
        Task<bool> DeleteAsync(int id);
        public Task<List<ViewOrder>> ManageOrderAsync();

    }
}
