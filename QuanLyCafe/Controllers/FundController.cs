using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyCafe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace QuanLyCafe.Controllers
{
    [Route("api/fund")]
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FundController(AppDbContext context)
        {
            _context = context;
        }

        // API để lấy danh sách tất cả các quỹ
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fund>>> GetFunds()
        {
            var funds = await _context.funds.ToListAsync();
            return Ok(funds);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Fund>>> GetFundById(int id)
        {
            var fundsById = _context.funds.FirstOrDefault(x => x.Id == id);
            if (fundsById == null)
            {
                return BadRequest("Không tìm thấy sản phẩm");
            }
            return Ok(fundsById);
        }


        [HttpPost]
        public async Task<ActionResult<IEnumerable<Fund>>> addFund([FromBody] FundCreateDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            dto.create_at = DateTime.Now;

            if (string.IsNullOrEmpty(dto.FundName))
            {
                return BadRequest("Vui lòng điền tên quỹ");
            }

            var newFund = new Fund
            {
                Id = dto.Id,
                SumPrice = dto.SumPrice,
                detail_status = dto.detail_status,
                FundName = dto.FundName,
            };
            _context.funds.Add(newFund);
            _context.SaveChanges();

            return Ok(newFund);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFundById(int id)
        {
            var deleteFund = _context.funds.FirstOrDefault(x => x.Id == id);
            if(deleteFund == null)
            {
                return NotFound("");
            }
            _context.funds.Remove(deleteFund);
            _context.SaveChanges();
            return Ok(id);
        }
        
         [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] FundUpdateDTO fundUpdateDTO)
        {
            var fund = _context.funds.FirstOrDefault(a => a.Id == id);
            if (fund == null)
            {
                return NotFound("Cannot find product");
            }
            fund.SumPrice = fundUpdateDTO.SumPrice;
            fund.detail_status = fundUpdateDTO.detail_status;
            fund.FundName = fundUpdateDTO.FundName;
            fund.creat_at = DateTime.UtcNow;
            _context.SaveChanges();
            return Ok(id);

        }

        



    }

}