using Microsoft.AspNetCore.Mvc;
using QuanLyCafe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
namespace QuanLyCafe.Controllers
{
    [ApiController]
    [Route("api/product")]
    [EnableCors("AllowAngularClient")]

    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<ProductCoffee>> Get()
        {
            var productCoffees = _context.ProductCoffee.ToList();

            return productCoffees;
        }


        [HttpGet("{id}")]
        public ActionResult<ProductCoffee> GetById(int id)
        {
            var productCoffee = _context.ProductCoffee.FirstOrDefault(a => a.Id == id);
            if (productCoffee == null)
            {
                return NotFound($"Product with ID {id} not found");
            }
            return Ok(productCoffee);
        }

        [HttpPost]
        public ActionResult<ProductCoffee> AddProduct(ProductCoffee productCoffee)
        {
            if (productCoffee == null)
            {
                return BadRequest("Product cannot be null");
            }
            productCoffee.CreatedAt = DateTime.Now;
            _context.ProductCoffee.Add(productCoffee);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = productCoffee.Id }, productCoffee);

        }


        [HttpDelete("{id}")]
        public ActionResult DeteleProduct(int id)
        {
            var productCoffee = _context.ProductCoffee.FirstOrDefault(a => a.Id == id);
            if (productCoffee == null)
            {
                return NotFound("Cannot find id product");
            }
            _context.ProductCoffee.Remove(productCoffee);
            _context.SaveChanges();
            return Ok(id);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] ProductCoffee updateProductCoffee)
        {
            var productCoffee = _context.ProductCoffee.FirstOrDefault(a => a.Id == id);
            if (productCoffee == null)
            {
                return NotFound("Cannot find product");
            }

            productCoffee.Name = updateProductCoffee.Name;
            productCoffee.Detail = updateProductCoffee.Detail;
            productCoffee.price = updateProductCoffee.price;
            productCoffee.Category_Name = updateProductCoffee.Category_Name;
            productCoffee.CreatedAt = DateTime.Now;
            productCoffee.Status = updateProductCoffee.Status;
            productCoffee.Deleted = updateProductCoffee.Deleted;
            productCoffee.ImageProduct = updateProductCoffee.ImageProduct;
            _context.SaveChanges();
            return Ok(id);


        }


    }
}