using Microsoft.AspNetCore.Mvc;
using QuanLyCafe.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace QuanLyCafe.Controllers
{
    [ApiController]
    [Route("api/product")]
    [EnableCors("AllowAngularClient")]
    [Authorize]
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

        public ActionResult<ProductCoffee> AddProduct([FromBody] ProductCreateDto productCreateDto)
        {
            if (productCreateDto == null)
            {
                return BadRequest("Product cannot be null");
            }
            productCreateDto.CreatAt = DateTime.Now;
            // productCoffee.OrderDetailProducts = null;
            // productCoffee.deatailStockProducts = null;
            var newProduct = new ProductCoffee
            {
                Id = productCreateDto.Id,
                Name = productCreateDto.Name,
                Detail = productCreateDto.Detail,
                price = productCreateDto.Price,
                category_Name = productCreateDto.Category_Name,
                Status = true,
                Deleted = false,
                ImageProduct = productCreateDto.ImageProduct,
            };
            _context.ProductCoffee.Add(newProduct);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = productCreateDto.Id }, productCreateDto);

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
        public ActionResult UpdateProduct(int id, [FromBody] UpdateProductDto updateProductCoffee)
        {
            var productCoffee = _context.ProductCoffee.FirstOrDefault(a => a.Id == id);
            if (productCoffee == null)
            {
                return NotFound("Cannot find product");
            }

            productCoffee.Name = updateProductCoffee.Name;
            productCoffee.Detail = updateProductCoffee.Detail;
            productCoffee.price = updateProductCoffee.Price;
            productCoffee.category_Name = updateProductCoffee.Category_Name;
            productCoffee.CreatedAt = DateTime.Now;
            productCoffee.Status = updateProductCoffee.Status;
            productCoffee.Deleted = updateProductCoffee.Deleted;
            productCoffee.ImageProduct = updateProductCoffee.ImageProduct;
            _context.SaveChanges();
            return Ok(id);


        }


        // [HttpDelete("{id}")]
        // public ActionResult DeletedById(int id)
        // {
        //     var productDelete = _context.ProductCoffee.FirstOrDefault(x => x.Id == id);
        //     if (productDelete == null)
        //     {
        //         return BadRequest("Sản phẩm không tồn tại");
        //     }
        //     productDelete.Deleted = true;
        //     _context.SaveChanges();
        //     return Ok(id);
        // }



    }
}