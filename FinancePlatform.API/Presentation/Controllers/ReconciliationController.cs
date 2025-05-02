using FinancePlatform.API.Application.Services;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlatform.API.Presentation.Controllers
{
    [ApiController]
    [Route("api/reconciliations")]
    public class ReconciliationController : ControllerBase
    {
        private readonly ReconciliationService _reconciliationService;

        public ReconciliationController(ReconciliationService reconciliationService)
        {
            _reconciliationService = reconciliationService;
        }

        /// <summary>
        /// Cria uma nova reconciliação financeira.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Reconciliation>> Create([FromForm] ReconciliationInputModel model)
        {
            if (model == null)
                return BadRequest("Os dados da reconciliação são obrigatórios.");

            var reconciliation = await _reconciliationService.AddAsync(model);
            if (reconciliation == null)
                return BadRequest("Não foi possível criar a reconciliação.");

            return CreatedAtAction(nameof(GetById), new { id = reconciliation.Id }, reconciliation);
        }

        /// <summary>
        /// Obtém uma reconciliação pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReconciliationViewModel>> GetById(Guid id)
        {
            var reconciliation = await _reconciliationService.FindByIdAsync(id);
            if (reconciliation == null)
                return NotFound();

            return Ok(reconciliation);
        }

        /// <summary>
        /// Obtém todas as reconciliações registradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ReconciliationViewModel>>> GetAll()
        {
            var reconciliations = await _reconciliationService.FindAllAsync();
            return Ok(reconciliations);
        }

        /// <summary>
        /// Atualiza uma reconciliação pelo ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] Dictionary<string, object> updateRequest)
        {
            if (updateRequest == null || updateRequest.Count == 0)
                return BadRequest("Nenhum dado fornecido para atualização.");

            var updatedReconciliation = await _reconciliationService.UpdateAsync(id, updateRequest);
            if (updatedReconciliation == null)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Exclui uma reconciliação pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _reconciliationService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}