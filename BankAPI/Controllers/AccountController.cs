using BankAPI.BankModels;
using BankAPI.DTOs;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService accountService;
        private readonly AccountTypeService accountTypeService;
        private readonly ClientService clientService;

        public AccountController(
            AccountService accountService,
            AccountTypeService accountTypeService,
            ClientService clientService
        )
        {
            this.accountService = accountService;
            this.accountTypeService = accountTypeService;
            this.clientService = clientService;
        }
        private async Task<string> ValidateAccount(AccountDTOIn account)
        {
            string result = "Valid";

            var accountType = await accountTypeService.GetById(account.Id);

            if (accountType == null)
            {
                result = $"El tipo de cuenta {account.AccountType} no existe";
            }

            var clientID = account.ClientId;

            var client = await clientService.GetClientById(clientID);

            if (client == null)
            {
                result = $"El cliente {clientID} no existe";
            }
            return result;
        }

        private NotFoundObjectResult AccountNotFound(int id)
        {
            return NotFound(new { message = $"La cuenta con ID = {id} no existe." });
        }

        [HttpGet("getall")]
        public async Task<IEnumerable<AccountDtoOut>> GetAll()
        {
            return await accountService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDtoOut>> GetById(int id)
        {
            var account = await accountService.GetDtoById(id);

            if (account is null)
            {
                return AccountNotFound(id);
            }

            return account;
        }
        [Authorize(Policy = "SuperAdmin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(AccountDTOIn account)
        {
            string validationResult = await ValidateAccount(account);

            if (!validationResult.Equals("Valid"))
            {
                return BadRequest(new {message = validationResult});
            }

            var newAccount = await accountService.Create(account);

            return CreatedAtAction(nameof(GetById), new { id = newAccount.Id }, newAccount);

        }

        [Authorize(Policy = "SuperAdmin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, AccountDTOIn account)
        {
            if (id !=  account.Id)
            {
                return BadRequest(new {message = $"El ID({id}) de la URL no coincide con el ID ({account.Id}) de la cuenta"});
            }

            var accountToUpdate = await accountService.GetById(id);

            if (accountToUpdate is not null)
            {
                string validationResult = await ValidateAccount(account);

                if (!validationResult.Equals("Valid"))
                {
                    return BadRequest(new { message = validationResult});
                }

                await accountService.Update(account);
                return NoContent();
            } else
            {
                return AccountNotFound(id);
            }
        }
    }
}
