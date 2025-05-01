using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.UseCases;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlatform.API.Presentation.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentUseCase _paymentUseCase;

        public PaymentController(IPaymentService paymentService, IPaymentUseCase paymentUseCase)
        {
            _paymentService = paymentService;
            _paymentUseCase = paymentUseCase;
        }

        /// <summary>
        /// Obtém um pagamento pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentViewModel>> GetById(Guid id)
        {
            var payment = await _paymentService.FindByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        /// <summary>
        /// Obtém todos os pagamentos registrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PaymentViewModel>>> GetAll()
        {
            var payments = await _paymentService.FindAllAsync();
            if (payments == null)
                return NotFound("Nenhum pagamento encontrada.");

            return Ok(payments);
        }

        /// <summary>
        /// Gera um novo pagamento com base nos dados fornecidos.
        /// </summary>
        [HttpPost("generate")]
        public async Task<ActionResult<Payment>> GeneratePayment([FromForm] PaymentInputModel model)
        {
            if (model == null)
                return BadRequest("Dados do pagamento são obrigatórios.");

            var payment = await _paymentUseCase.generatePayment(model);
            if (payment == null)
                return BadRequest("Não foi possível processar o pagamento.");

            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
        }

        /// <summary>
        /// Atualiza um pagamento pelo ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] Dictionary<string, object> updateRequest)
        {
            if (updateRequest == null || updateRequest.Count == 0)
                return BadRequest("Nenhum dado fornecido para atualização.");

            var updatedPayment = await _paymentService.UpdateAsync(id, updateRequest);
            if (updatedPayment == null)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Exclui um pagamento pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _paymentService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}