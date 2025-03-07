using Microsoft.AspNetCore.Mvc;
using QuanLyCafe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
namespace QuanLyCafe.Controllers
{
    [ApiController]
    [Route("api/table")]
    [EnableCors("AllowAngularClient")]

    public class TableController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TableController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<TableCoffe>> GetTable()
        {
            var table = _context.Tables.ToList();
            return table;
        }

        [HttpGet("{id}")]
        public ActionResult<TableCoffe> GetTableById(int id)
        {
            var table = _context.Tables.FirstOrDefault(a => a.Id == id);
            if (table == null)
            {
                return NotFound("Cannot find table");
            }
            return Ok(table);
        }

        [HttpPost]
        public ActionResult<TableCoffe> AddTable(TableCoffe table)
        {
            if (table == null)
            {
                return BadRequest("Cannot create product");
            }
            _context.Tables.Add(table);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTableById), new { id = table.Id }, table);
        }

        [HttpDelete("{id}")]
        public ActionResult DeteleProduct(int id)
        {
            var Table = _context.Tables.FirstOrDefault(a => a.Id == id);
            if (Table == null)
            {
                return NotFound("Cannot find id table");
            }
            _context.Tables.Remove(Table);
            _context.SaveChanges();
            return Ok(id);
        }

        [HttpPut("{id}")]
        public ActionResult<TableCoffe> UpdateTable(int id, TableCoffe updatedTable)
        {
            // Kiểm tra xem bảng có tồn tại không
            var table = _context.Tables.FirstOrDefault(a => a.Id == id);
            if (table == null)
            {
                return NotFound("Cannot find table");
            }

            // Cập nhật các thuộc tính của bảng
            table.TableName = updatedTable.TableName; // Tên bảng
            table.ChairNumber = updatedTable.ChairNumber; // Số ghế
            table.Status = updatedTable.Status; // Trạng thái
            table.Deleted = updatedTable.Deleted; // Trạng thái đã xóa

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            return Ok(table); // Trả về bảng đã được cập nhật
        }


    }
}