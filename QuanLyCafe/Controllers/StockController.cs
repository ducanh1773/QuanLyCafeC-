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
        
        [HttpGet]
        public ActionResult<List<Stock>> Get()
        {
            var Stock = _context.Stocks.ToList();
            return Stock;
        }
        
        [HttpGet("{id}")]
        public ActionResult<Stock> GetById(int id)
        {
            var Stock = _context.Stocks.FirstOrDefault(a => a.Id == id);    
            if(Stock == null)
            {
                return NotFound("Cannot find stock");
            }
            return Ok(Stock);
        }
        
        
        [HttpPost]
        public ActionResult<Stock> AddStock(Stock stock)
        {
            if(stock == null)
            {
                return BadRequest("Cannot create stock");
            }
            _context.Stocks.Add(stock);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById) , new {id = stock.Id}, stock);
        }
        
        [HttpPut("{id}")]
        public ActionResult<Stock> UpdateStock(Stock UpdateStock , int id)
        {
            var stock = _context.Stocks.FirstOrDefault(a=>a.Id == id);
            if(stock == null)
            {
                return NotFound("Stock cannot update");
            }
            stock.Quantity = UpdateStock.Quantity;
            stock.Status = UpdateStock.Status;
            stock.Deleted = UpdateStock.Deleted;
            stock.UnitOfMeasure = UpdateStock.UnitOfMeasure;
            _context.SaveChanges();
            return Ok(id); 
        }

        
    }

}