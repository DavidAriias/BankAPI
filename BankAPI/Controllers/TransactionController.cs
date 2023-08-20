using BankAPI.DTOs;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {

        private readonly TransactionService _service;

        public TransactionController(TransactionService service)
        {
            _service = service;
        }

        [Authorize("Client")]
        [HttpPost("retiro")]
        public async Task<IActionResult> MakingWithdrawal(WithdrawalDTO withdrawal)
        {
            var accountFounded = await _service.GetAccountById(withdrawal.AccountId);

            if (accountFounded is not null)
            {
                if (withdrawal.TransactionType != 2 || withdrawal.TransactionType != 4)
                {
                    return BadRequest("Metodo no valido para retiros");
                }

                if (withdrawal.TransactionType == 4 && withdrawal.ExternalAccount.HasValue) 
                {
                    return BadRequest(new { message = "No se pueede realizar la transaccion" });
                }

               await _service.MakeWithdrawal(withdrawal);

                return Ok();
            } else
            {
                return NotFound(new { message = "Cuenta ingresada no valida" });
            }
        }

        [Authorize("Client")]
        [HttpPost("deposito")]
        public async Task<IActionResult> MakeDeposit(DepositDTO deposit)
        {
            var accountFounded = _service.GetAccountById(deposit.AccountId);

            if (accountFounded is not null)
            {
                await _service.MakingDeposits(deposit);
                return Ok();
            } else
            {
                return NotFound( new {message = "No se ha encontrado la cuenta ingresada"});
            }
            
        }

        [Authorize("Client")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAccount(int id) 
        {
            var accountToDelete = await _service.GetAccountById(id);

            if (accountToDelete is not null && accountToDelete.Balance == 0)
            {
                await _service.DeleteAccount(accountToDelete);
                return Ok();
            } else
            {
                return BadRequest(new {message = "La cuenta no puede ser borrada"});
            }
        }
    }
}
