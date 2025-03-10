using Microsoft.AspNetCore.Mvc;
using QuanLyCafe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace QuanLyCafe.Controllers
{
    [ApiController]
    [Route("api/stock")]
    [EnableCors("AllowAngularClient")]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách stock
        [HttpGet]
        public ActionResult<List<Stock>> Get()
        {
            return _context.Stocks.Where(s => !s.Deleted).ToList();
        }

        // Lấy stock theo ID
        [HttpGet("{id}")]
        public ActionResult<Stock> GetById(int id)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id && !s.Deleted);
            if (stock == null)
            {
                return NotFound("Không tìm thấy Stock.");
            }
            return Ok(stock);
        }

        // Thêm mới stock
        [HttpPost]
        public ActionResult<Stock> AddStock([FromBody] UpdateStockDTO stockDto)
        {
            if (stockDto == null)
            {
                return BadRequest("Dữ liệu Stock không hợp lệ.");
            }

            var stock = new Stock
            {
                Name = stockDto.Name,
                Quantity = stockDto.Quantity,
                Status = stockDto.Status,
                Deleted = stockDto.Deleted,
                UnitOfMeasure = stockDto.UnitOfMeasure
            };

            _context.Stocks.Add(stock);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock);
        }

        // Cập nhật stock
        [HttpPut("{id}")]
        public ActionResult UpdateStock(int id, [FromBody] UpdateStockDTO updateDto)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id && !s.Deleted);
            if (stock == null)
            {
                return NotFound("Không thể cập nhật Stock.");
            }

            stock.Name = updateDto.Name;
            stock.Quantity = updateDto.Quantity;
            stock.Status = updateDto.Status;
            stock.Deleted = updateDto.Deleted;
            stock.UnitOfMeasure = updateDto.UnitOfMeasure;

            _context.SaveChanges();
            return Ok($"Stock {id} đã được cập nhật.");
        }

        // Xóa (Soft Delete) stock
        [HttpDelete("{id}")]
        public ActionResult DeleteById(int id)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id && !s.Deleted);
            if (stock == null)
            {
                return NotFound("Không thể xóa Stock.");
            }

            stock.Deleted = true; // Chỉ đánh dấu là đã bị xóa
            _context.SaveChanges();

            return Ok($"Stock {id} đã bị vô hiệu hóa (soft delete).");
        }
    }
}
