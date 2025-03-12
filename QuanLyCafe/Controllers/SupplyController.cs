using Microsoft.AspNetCore.Mvc;
using QuanLyCafe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
namespace QuanLyCafe.Controllers
{
    [ApiController]
    [Route("api/supply")]
    [EnableCors("AllowAngularClient")]

    public class SupplyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SupplyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public ActionResult<List<Supply>> Get()
        {
            var Supply = _context.Supplies.ToList();
            return Supply;
        }

        [HttpGet("{id}")]

        public ActionResult<List<Supply>> GetById(int id)
        {
            var Supply = _context.Supplies.FirstOrDefault(x => x.id == id);
            return Ok(id);
        }



        // Thêm supply và upoate fund và stock

        [HttpPost]
        public ActionResult<Supply> AddSupply(SupplyRequestDto input)
        {
            if (input == null || input.Stocks == null || !input.Stocks.Any())
            {
                return BadRequest("Cannot create supply and related records");
            }

            // Tìm tài khoản theo username
            var account = _context.Accounts.FirstOrDefault(a => a.UserName == input.UserName);

            if (account == null)
            {
                return NotFound($"Không tìm thấy tài khoản với username: {input.UserName}");
            }

            var supply = new Supply
            {
                Id_Account = account.ID, // Lấy Id từ tài khoản tìm được
                Time_In = DateTime.UtcNow,
                Status = true,
                Deleted = false,
            };

            _context.Supplies.Add(supply);
            _context.SaveChanges();
            Console.WriteLine(supply);

            foreach (var stockRequest in input.Stocks)
            {
                var stock = _context.Stocks.FirstOrDefault(x => x.Name == stockRequest.NameStock);

                if (stock == null)
                {
                    return NotFound($"Không tìm thấy mặt hàng: {stockRequest.NameStock}");
                }

                stock.Quantity += stockRequest.Quantity;

                var detailSupplyStocks = new DetailSupplyStock
                {
                    ID_Supply = supply.id,
                    Id_Stock = stock.Id,
                    Quantity = stockRequest.Quantity,
                };

                _context.detailSupplyStocks.Add(detailSupplyStocks);
                Console.WriteLine(detailSupplyStocks);

                var fund = _context.funds.FirstOrDefault(f => f.FundName == stockRequest.PaymentMethod);
                if (fund != null)
                {
                    fund.SumPrice -= stockRequest.Price;
                }

                var paymentForms = new PaymentForm
                {
                    Id_Order = 0,
                    ID_Supply = supply.id,
                    Id_Fund = 0,
                    Payment_Method = stockRequest.PaymentMethod,
                    CreatedAt = DateTime.UtcNow,
                    Status = false,
                    Deleted = false,
                    Sum_Price = stockRequest.Price,
                };

                Console.WriteLine(paymentForms);
                _context.paymentForms.Add(paymentForms);
            }

            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = supply.id }, supply);
        }

        [HttpPut("{id}")]
        public ActionResult<Supply> UpdateSupply(int id, SupplyRequestDto input)
        {
            var supply = _context.Supplies.FirstOrDefault(s => s.id == id);
            if (supply == null)
            {
                return NotFound($"Không tìm thấy Supply với ID: {id}");
            }

            var account = _context.Accounts.FirstOrDefault(a => a.UserName == input.UserName);
            if (account == null)
            {
                return NotFound($"Không tìm thấy tài khoản với username: {input.UserName}");
            }

            supply.Id_Account = account.ID;
            supply.Time_In = DateTime.UtcNow;
            _context.SaveChanges();

            foreach (var stockRequest in input.Stocks)
            {
                var stock = _context.Stocks.FirstOrDefault(s => s.Name == stockRequest.NameStock);
                if (stock == null)
                {
                    return NotFound($"Không tìm thấy mặt hàng: {stockRequest.NameStock}");
                }

                var detailSupplyStock = _context.detailSupplyStocks
                    .FirstOrDefault(ds => ds.ID_Supply == supply.id && ds.Id_Stock == stock.Id);

                if (detailSupplyStock != null)
                {
                    stock.Quantity -= detailSupplyStock.Quantity; // Trừ số lượng cũ
                    detailSupplyStock.Quantity = stockRequest.Quantity;
                }
                else
                {
                    detailSupplyStock = new DetailSupplyStock
                    {
                        ID_Supply = supply.id,
                        Id_Stock = stock.Id,
                        Quantity = stockRequest.Quantity,
                    };
                    _context.detailSupplyStocks.Add(detailSupplyStock);
                }

                stock.Quantity += stockRequest.Quantity; // Cập nhật số lượng mới

                var paymentForm = _context.paymentForms.FirstOrDefault(pf => pf.ID_Supply == supply.id);
                if (paymentForm != null)
                {
                    paymentForm.Sum_Price = stockRequest.Price;
                    paymentForm.Payment_Method = stockRequest.PaymentMethod;
                }
            }
    
            _context.SaveChanges();
            return Ok(id);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteSupply(int id)
        {
            var supply = _context.Supplies.FirstOrDefault(s => s.id == id);
            if (supply == null)
            {
                return NotFound($"Không tìm thấy Supply với ID: {id}");
            }

            var detailSupplyStocks = _context.detailSupplyStocks.Where(ds => ds.ID_Supply == id).ToList();
            foreach (var detail in detailSupplyStocks)
            {
                var stock = _context.Stocks.FirstOrDefault(s => s.Id == detail.Id_Stock);
                if (stock != null)
                {
                    stock.Quantity -= detail.Quantity;
                }
                _context.detailSupplyStocks.Remove(detail);
            }

            var paymentForms = _context.paymentForms.Where(pf => pf.ID_Supply == id).ToList();
            _context.paymentForms.RemoveRange(paymentForms);

            _context.Supplies.Remove(supply);
            _context.SaveChanges();

            return NoContent();
        }





    }

}