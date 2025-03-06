// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using QuanLyCafe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace QuanLyCafe.Controllers
{
    [ApiController]
    [Route("api/account")]
    [EnableCors("AllowAngularClient")] 
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // [HttpGet]
        // public ActionResult<List<Account>> Get()
        // {
        //     return _context.Accounts.ToList();
        // }

        [HttpGet]
        public ActionResult<List<Account>> Get()
        {
            var accounts = _context.Accounts.ToList();
            Console.WriteLine($"Retrieved {accounts.Count} accounts."); // Ghi log số lượng tài khoản
            return accounts;
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetById(int id)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.ID == id);

            if (account == null)
            {
                return NotFound($"Account with ID {id} not found.");
            }

            return Ok(account);
        }


        [HttpPost]

        public ActionResult<Account> AddAccount(Account account)
        {
            if (account == null)
            {
                return BadRequest("Account cannot be null");
            }

            account.Create_At = DateTime.UtcNow;
            _context.Accounts.Add(account);
            _context.SaveChanges();


            return CreatedAtAction(nameof(GetById), new { id = account.ID }, account);
        }

        [HttpPut("{id}")]
        public ActionResult<Account> UpdateAccount(int id, [FromBody] Account updatedAccount)
        {
            var existingAccount = _context.Accounts.FirstOrDefault(a => a.ID == id);
            if (existingAccount == null)
            {
                return NotFound($"Account with ID {id} not found.");
            }

            existingAccount.UserName = updatedAccount.UserName;
            existingAccount.Email = updatedAccount.Email;
            existingAccount.PhoneNumber = updatedAccount.PhoneNumber;
            existingAccount.Address = updatedAccount.Address;
            existingAccount.Password = updatedAccount.Password;
            existingAccount.Status = updatedAccount.Status;
            existingAccount.Deleted = updatedAccount.Deleted;

            _context.SaveChanges();
            return Ok(id);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAccount(int id)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.ID == id);
            if (account == null)
            {
                return NotFound($"Account with ID {id} not found.");
            }

            _context.Accounts.Remove(account);
            _context.SaveChanges();

            return Ok(id);
        }




    }
}




