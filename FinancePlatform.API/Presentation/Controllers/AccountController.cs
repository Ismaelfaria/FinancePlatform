using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.UseCases;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlatform.API.Presentation.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountUseCase _accountUseCase;

        public AccountController(IAccountService accountService, 
                                 IAccountUseCase accountUseCase)
        {
            _accountService = accountService;
            _accountUseCase = accountUseCase;
        }

        /// <summary>
        /// Obtém todas as contas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<AccountViewModel>>> GetAllAccounts()
        {
            var accounts = await _accountService.FindAllAccountsAsync();
            if (accounts == null) 
                return NotFound("Nenhuma conta encontrada.");

            return Ok(accounts);
        }

        /// <summary>
        /// Obtém uma conta pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountViewModel>> GetAccountById([FromForm] Guid id)
        {
            var account = await _accountService.FindByIdAsync(id);
            if (account == null) 
                return NotFound("Conta não encontrada.");

            return Ok(account);
        }

        /// <summary>
        /// Cria uma nova conta
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AccountViewModel>> CreateAccount([FromForm] AccountInputModel model)
        {
            var createdAccount = await _accountService.CreateAccountAsync(model);
            if (createdAccount == null) return BadRequest("Falha na validação dos dados.");

            return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.Id }, createdAccount);
        }

        /// <summary>
        /// Atualiza parcialmente uma conta
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AccountViewModel>> UpdateAccount([FromForm] Guid id, Dictionary<string, object> updateRequest)
        {
            if (updateRequest == null || updateRequest.Count == 0)
                return BadRequest("Nenhum dado fornecido para atualização.");

            var updatedAccount = await _accountService.UpdateAccountAsync(id, updateRequest);
            if (updatedAccount == null) return NotFound("Conta não encontrada.");

            return Ok(updatedAccount);
        }

        /// <summary>
        /// Exclui uma conta pelo ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount([FromForm] Guid id)
        {
            var deleted = await _accountService.DeleteAccountAsync(id);
            if (!deleted) return NotFound("Conta não encontrada.");

            return NoContent();
        }

        /// <summary>
        /// Deposite em uma conta pelo ID e valor a ser depositado
        /// </summary>
        [HttpPost("deposit/{accountId}")]
        public async Task<IActionResult> Deposit([FromForm] Guid accountId, decimal amount)
        {
            var result = await _accountUseCase.Deposit(accountId, amount);
            if (!result) return BadRequest("Depósito não realizado. Verifique o valor ou a conta.");
            
            return Ok("Depósito realizado com sucesso.");
        }

        /// <summary>
        /// Buscar saldo da conta pelo ID
        /// </summary>
        [HttpGet("balance/{accountId}")]
        public async Task<IActionResult> GetBalance(Guid accountId)
        {
            var balance = await _accountUseCase.FindBalance(accountId);
            return Ok(new { balance });
        }

        /// <summary>
        /// Sacar em uma conta pelo ID
        /// </summary>
        [HttpPost("withdraw/{accountId}")]
        public async Task<IActionResult> Withdraw(Guid accountId, [FromBody] decimal amount)
        {
            var result = await _accountUseCase.Withdraw(accountId, amount);
            if (!result) return BadRequest("Saque não realizado. Verifique o valor ou a conta.");
            
            return Ok("Saque realizado com sucesso.");
        }
    }
}
