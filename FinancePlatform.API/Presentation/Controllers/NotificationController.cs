using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlatform.API.Presentation.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Obtém uma notificação pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationViewModel>> GetById(Guid id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) 
                return NotFound();

            return Ok(notification);
        }

        /// <summary>
        /// Obtém todas as notificações
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<NotificationViewModel>>> GetAll()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            if (notifications == null) 
                return NotFound("Nenhuma notificação encontrada.");

            return Ok(notifications);
        }

        /// <summary>
        /// Cria uma nova notificação
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<NotificationViewModel>> Create([FromBody] NotificationInputModel model)
        {
            if (model == null)
                return BadRequest("Dados inválidos.");

            var createdNotification = await _notificationService.CreateNotificationAsync(model);
            if (createdNotification == null)
                return BadRequest("Erro ao validar a notificação.");

            return CreatedAtAction(nameof(GetById), new { id = createdNotification.Id }, createdNotification);
        }
        
        /// <summary>
        /// Atualiza uma notificação pelo ID, e um JSON com 'chave - valor'
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Dictionary<string, object> updateRequest)
        {
            if (updateRequest == null || updateRequest.Count == 0)
                return BadRequest("Nenhum dado para atualização.");

            var updatedNotification = await _notificationService.UpdateAsync(id, updateRequest);
            if (updatedNotification == null)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deleta uma notificação por ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _notificationService.DeleteNotificationAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}