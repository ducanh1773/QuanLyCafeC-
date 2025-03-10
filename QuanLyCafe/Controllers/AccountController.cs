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

        public ActionResult<Account> AddAccount([FromBody] CreateAccountDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            dto.Creat_At = DateTime.UtcNow;
            if (string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest("Vui lòng điền đầy đủ thông tin.");
            }

            var newAccount = new Account
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Status = dto.Status,
                Deleted = dto.Deleted,
                Create_At = DateTime.UtcNow,

            };

            _context.Accounts.Add(newAccount);

            _context.SaveChanges();



            return CreatedAtAction(nameof(GetById), new { id = newAccount.ID }, newAccount);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateAccount(int id, [FromBody] UpdateAccountDTO dto)
        {
            var existingAccount = _context.Accounts.FirstOrDefault(a => a.ID == id);
            if (existingAccount == null)
            {
                return NotFound($"Không tìm thấy tài khoản có ID {id}.");
            }

            // Cập nhật chỉ các thông tin liên quan đến tài khoản
            existingAccount.UserName = dto.UserName;
            existingAccount.Email = dto.Email;
            existingAccount.PhoneNumber = dto.PhoneNumber;
            existingAccount.Address = dto.Address;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                existingAccount.Password = dto.Password; // Mã hóa mật khẩu
            }

            existingAccount.Status = dto.Status;
            existingAccount.Deleted = dto.Deleted;

            _context.SaveChanges();
            return Ok($"Tài khoản {id} đã được cập nhật thành công.");
        }


        [HttpDelete("{id}")]
        public ActionResult DeleteAccount(int id)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.ID == id);
            if (account == null)
            {
                return NotFound($"Không tìm thấy tài khoản có ID {id}.");
            }

            // Không xóa vĩnh viễn, chỉ đánh dấu Deleted = true
            account.Deleted = true;
            _context.SaveChanges();

            return Ok($"Tài khoản {id} đã bị vô hiệu hóa (soft delete).");
        }




    }
}




