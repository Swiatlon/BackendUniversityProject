using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountOnlyDto>>> GetAccounts()
        {
            try
            {
                var accounts = await _accountService.GetAllAccountsAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting accounts");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("by-id/{id:guid}")]
        public async Task<ActionResult<AccountFullDto>> GetAccountById(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account is null)
                return NotFound("Account not found");
            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<AccountFullDto>> AddAccount(AccountFullDto accountDto)
        {
            var account = await _accountService.AddAccountAsync(accountDto);
            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, AccountFullDto accountDto)
        {
            if (id != accountDto.Id)
                return BadRequest("Account ID mismatch");

            var account = await _accountService.GetAccountByIdAsync(id);
            if (account is null)
                return NotFound("Account not found");

            var updatedAccount = await _accountService.UpdateAccountAsync(accountDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account is null)
                return NotFound("Account not found");

            await _accountService.DeleteAccountAsync(id);
            return NoContent();
        }

        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<AccountFullDto>> GetAccountByUsername(string username)
        {
            var account = await _accountService.GetByUsernameAsync(username);
            if (account is null)
                return NotFound("Account not found");
            return Ok(account);
        }
    }
}
