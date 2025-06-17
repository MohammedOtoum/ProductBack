using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductTask.Dto;
using ProductTask.Model;
using ProductTask.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutFormController : ControllerBase
    {
        private readonly ICheckoutFormRepository _repository;

        public CheckoutFormController(ICheckoutFormRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckoutForm>>> GetAll() =>
            Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<CheckoutForm>> Get(int id)
        {
            var form = await _repository.GetByIdAsync(id);
            return form == null ? NotFound() : Ok(form);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CheckoutForm form)
        {
            var validationResult = ValidateCheckoutForm(form);
            if (validationResult != null)
                return validationResult;

            var created = await _repository.AddAsync(form);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CheckoutForm form)
        {
            var validationResult = ValidateCheckoutForm(form);
            if (validationResult != null)
                return validationResult;

            var updated = await _repository.UpdateAsync(id, form);
            return updated == null ? NotFound() : NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // Validation method to check CardInfo only if payment method is Credit / Debit Card
        private IActionResult ValidateCheckoutForm(CheckoutForm form)
        {
            if (form.PaymentMethod == "Credit / Debit Card")
            {
                if (form.CardInfo == null)
                    return BadRequest("Card information is required for Credit / Debit Card payments.");

                if (string.IsNullOrWhiteSpace(form.CardInfo.CardHolderName))
                    return BadRequest("Cardholder name is required.");

                if (string.IsNullOrWhiteSpace(form.CardInfo.CardNumber))
                    return BadRequest("Card number is required.");

                if (string.IsNullOrWhiteSpace(form.CardInfo.ExpirationDate))
                    return BadRequest("Expiration date is required.");

                if (string.IsNullOrWhiteSpace(form.CardInfo.CVV))
                    return BadRequest("CVV is required.");

                // Optional: Add Luhn check or date validation here
            }

            // No card info required for other payment methods
            return null;
        }
        [HttpGet("managed")]
        public async Task<ActionResult<List<ViewOrder>>> GetManagedOrders()
        {
            var result = await _repository.ManageOrderAsync();

            if (result == null || result.Count == 0)
            {
                return NotFound("No managed orders found.");
            }

            return Ok(result);
        }
    }
}
