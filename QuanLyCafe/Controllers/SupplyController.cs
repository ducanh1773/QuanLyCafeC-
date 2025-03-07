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

        [HttpPost]

        public ActionResult<Supply> AddSupply(SupplyRequestDto input)
        {

            if (input == null || input.Stocks == null || !input.Stocks.Any())
            {
                return BadRequest("Cannot create supply and related records");
            }

            var supply = new Supply
            {
                Id_Account = input.Id_Account,
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
                    return NotFound($"Không tìm mặt hàng: {stockRequest.NameStock}");
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

    //     [HttpDelete("{id}")]
    //     public ActionResult<Supply> DeleteSupply(int id)
    //     {
    //         // Find the supply to delete, including related DetailSupplyStocks
    //         var supplyToDelete = _context.Supplies
    //             .Include(s => s.DetailSupplyStocks)
    //             .ThenInclude(d => d.Id_Stock) // Include the related Stock
    //             .FirstOrDefault(x => x.id == id);

    //         if (supplyToDelete == null)
    //         {
    //             return NotFound("Cannot find supply");
    //         }

    //         // Process each detail supply stock
    //         foreach (var detail in supplyToDelete.DetailSupplyStocks)
    //         {
    //             var stock = _context.Stocks.FirstOrDefault(x => x.Id == detail.Id_Stock);

    //             if (stock == null)
    //             {
    //                 return NotFound($"Cannot find item with ID: {detail.Id_Stock}");
    //             }

    //             // Update stock quantity
    //             stock.Quantity += detail.Quantity;

    //             // Optionally, you might want to remove the detail supply stock entry
    //             _context.detailSupplyStocks.Remove(detail);
    //         }

    //         // Remove the supply
    //         _context.Supplies.Remove(supplyToDelete);
    //         _context.SaveChanges();

    //         return Ok(supplyToDelete); // Return the deleted supply
    //     }

     }

}