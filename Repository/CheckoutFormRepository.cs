// Repositories/CheckoutFormRepository.cs
using Microsoft.EntityFrameworkCore;
using ProductTask.Data;
using ProductTask.Model;
using System;

namespace ProductTask.Repository
{
    public class CheckoutFormRepository : ICheckoutFormRepository
    {
        private readonly DataEF _context;

        public CheckoutFormRepository(DataEF context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CheckoutForm>> GetAllAsync() =>
            await _context.checkoutForms.Include(f => f.CardInfo).ToListAsync();

        public async Task<CheckoutForm?> GetByIdAsync(int id) =>
            await _context.checkoutForms.Include(f => f.CardInfo).FirstOrDefaultAsync(f => f.Id == id);

        public async Task<CheckoutForm> AddAsync(CheckoutForm form)
        {
            _context.checkoutForms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<CheckoutForm?> UpdateAsync(int id, CheckoutForm updatedForm)
        {
            var existing = await _context.checkoutForms.Include(f => f.CardInfo).FirstOrDefaultAsync(f => f.Id == id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(updatedForm);
            _context.Entry(existing.CardInfo).CurrentValues.SetValues(updatedForm.CardInfo);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var form = await _context.checkoutForms.FindAsync(id);
            if (form == null) return false;

            _context.checkoutForms.Remove(form);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
