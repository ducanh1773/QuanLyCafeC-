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
    [Route("api/order")]
    [EnableCors("AllowAngularClient")]
    [Authorize]
    public class OrderContrller : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderContrller(AppDbContext context)
        {
            _context = context;

        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreatOrder request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1️⃣ Kiểm tra Fund trước khi tiếp tục
                var paymentMethod = request.paymentForms.FirstOrDefault()?.PaymentMethod ?? "Unknown";
                var fund = await _context.funds.FirstOrDefaultAsync(f => f.FundName == paymentMethod);
                if (fund == null)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { message = $"Phương thức thanh toán '{paymentMethod}' không hợp lệ!" });
                }

                // 2️⃣ Tạo đơn hàng (OrderCoffe)
                var order = new OrderCoffe
                {
                    Id_Account = request.Id_Account,
                    TimeOrder = request.TimeOrder,
                    Status = false, // Đơn hàng chưa hoàn thành
                    Deleted = false
                };
                _context.orderCoffes.Add(order);
                await _context.SaveChangesAsync();

                decimal totalPrice = 0; // Tổng tiền đơn hàng

                // 3️⃣ Kiểm tra kho trước khi trừ
                foreach (var item in request.orderProductDtos)
                {
                    foreach (var stock in item.stockProductDtos)
                    {
                        var stockDetail = await _context.deatailStockProducts
                            .FirstOrDefaultAsync(dsp => dsp.Id_StockProduct == stock.ID_Stock && dsp.Id_Product == item.ProductCoffeeId);

                        if (stockDetail == null)
                        {
                            var stockItem = await _context.Stocks.FindAsync(stock.ID_Stock);
                            if (stockItem == null)
                            {
                                await transaction.RollbackAsync();
                                return BadRequest(new { message = $"Kho {stock.ID_Stock} không tồn tại!" });
                            }

                            stockDetail = new DeatailStockProduct
                            {
                                Id_StockProduct = stock.ID_Stock,
                                Id_Product = item.ProductCoffeeId,
                                Quantity = stockItem.Quantity
                            };

                            _context.deatailStockProducts.Add(stockDetail);
                            await _context.SaveChangesAsync();
                        }

                        if (stockDetail.Quantity < stock.QuantityStock)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(new { message = $"Không đủ nguyên liệu trong kho {stock.ID_Stock} cho sản phẩm {item.ProductCoffeeId}!" });
                        }
                    }
                }

                // 4️⃣ Nếu kho đủ hàng, trừ kho và thêm vào đơn hàng
                foreach (var item in request.orderProductDtos)
                {
                    var product = await _context.ProductCoffee.FindAsync(item.ProductCoffeeId);
                    if (product == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { message = $"Sản phẩm {item.ProductCoffeeId} không tồn tại!" });
                    }

                    decimal productPrice = product.price * item.QuantityProduct; // Tính tổng giá sản phẩm
                    totalPrice += productPrice; // Cộng vào tổng tiền đơn hàng

                    foreach (var stock in item.stockProductDtos)
                    {
                        var stockDetail = await _context.deatailStockProducts
                            .FirstOrDefaultAsync(dsp => dsp.Id_StockProduct == stock.ID_Stock && dsp.Id_Product == item.ProductCoffeeId);

                        if (stockDetail != null)
                        {
                            stockDetail.Quantity -= stock.QuantityStock;

                            var stockItem = await _context.Stocks.FindAsync(stock.ID_Stock);
                            if (stockItem != null)
                            {
                                stockItem.Quantity -= stock.QuantityStock;
                            }
                        }
                    }

                    // 5️⃣ Thêm vào OrderDetailProduct
                    var orderDetail = new OrderDetailProduct
                    {
                        Id_Order = order.Id,
                        Id_Product = item.ProductCoffeeId,
                        Quantity = item.QuantityProduct,
                        Price = product.price // Lưu giá sản phẩm tại thời điểm đặt hàng
                    };
                    _context.orderDetailProducts.Add(orderDetail);
                }

                // 6️⃣ Cập nhật Fund.SumPrice thay vì chỉ cập nhật PaymentForm
                fund.SumPrice += totalPrice;
                _context.funds.Update(fund);

                // 7️⃣ Tạo PaymentForm với tổng tiền đơn hàng
                var paymentForm = new PaymentForm
                {
                    Id_Order = order.Id,
                    Sum_Price = totalPrice, // Tổng tiền đơn hàng đã tính
                    Payment_Method = paymentMethod
                };
                _context.paymentForms.Add(paymentForm);

                // 8️⃣ Cập nhật trạng thái đơn hàng thành hoàn thành
                order.Status = true;
                await _context.SaveChangesAsync();

                // 9️⃣ Xác nhận transaction
                await transaction.CommitAsync();
                return Ok(new { message = "Đặt hàng thành công!", totalPrice, fundSumPrice = fund.SumPrice });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = "Lỗi khi tạo đơn hàng: " + ex.Message });
            }
        }



        [HttpDelete("delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.orderCoffes.FindAsync(orderId);
                if (order == null)
                {
                    return NotFound(new { message = "Đơn hàng không tồn tại!" });
                }

                // Xóa chi tiết đơn hàng
                var orderDetails = _context.orderDetailProducts.Where(od => od.Id_Order == orderId);
                _context.orderDetailProducts.RemoveRange(orderDetails);

                // Xóa phương thức thanh toán
                var payments = _context.paymentForms.Where(p => p.Id_Order == orderId);
                _context.paymentForms.RemoveRange(payments);

                // Xóa đơn hàng
                _context.orderCoffes.Remove(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Ok(new { message = "Đã xóa đơn hàng thành công!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = "Lỗi khi xóa đơn hàng: " + ex.Message });
            }
        }


        [HttpPut("update/{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] CreatOrder request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.orderCoffes.FindAsync(orderId);
                if (order == null)
                {
                    return NotFound(new { message = "Đơn hàng không tồn tại!" });
                }

                // Cập nhật thông tin đơn hàng
                order.TimeOrder = request.TimeOrder;
                order.Status = request.Status;
                order.Deleted = request.Deleted;

                // Cập nhật sản phẩm trong đơn hàng
                var existingOrderDetails = _context.orderDetailProducts.Where(od => od.Id_Order == orderId);
                _context.orderDetailProducts.RemoveRange(existingOrderDetails);

                decimal totalPrice = 0;

                foreach (var item in request.orderProductDtos)
                {
                    var product = await _context.ProductCoffee.FindAsync(item.ProductCoffeeId);
                    if (product == null)
                    {
                        return BadRequest(new { message = $"Sản phẩm {item.ProductCoffeeId} không tồn tại!" });
                    }

                    decimal productPrice = product.price * item.QuantityProduct;
                    totalPrice += productPrice;

                    var orderDetail = new OrderDetailProduct
                    {
                        Id_Order = orderId,
                        Id_Product = item.ProductCoffeeId,
                        Quantity = item.QuantityProduct,
                        Price = product.price
                    };
                    _context.orderDetailProducts.Add(orderDetail);
                }

                // Cập nhật tổng tiền trong PaymentForm
                var paymentForm = await _context.paymentForms.FirstOrDefaultAsync(p => p.Id_Order == orderId);
                if (paymentForm != null)
                {
                    paymentForm.Sum_Price = totalPrice;
                    paymentForm.Payment_Method = request.paymentForms.FirstOrDefault()?.PaymentMethod ?? "Unknown";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Cập nhật đơn hàng thành công!", totalPrice });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = "Lỗi khi cập nhật đơn hàng: " + ex.Message });
            }
        }


        [HttpGet("get/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _context.orderCoffes
                .Where(o => o.Id == orderId)
                .Select(o => new
                {
                    o.Id,
                    o.Id_Account,
                    o.TimeOrder,
                    o.Status,
                    o.Deleted,
                    OrderDetails = _context.orderDetailProducts
                        .Where(od => od.Id_Order == orderId)
                        .Select(od => new
                        {
                            od.Id_Product,
                            ProductName = _context.ProductCoffee
                                .Where(p => p.Id == od.Id_Product)
                                .Select(p => p.Name)
                                .FirstOrDefault(),
                            od.Quantity,
                            od.Price
                        }).ToList(),
                    PaymentForms = _context.paymentForms
                        .Where(p => p.Id_Order == orderId)
                        .Select(p => new
                        {
                            p.Sum_Price,
                            p.Payment_Method
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });
            }

            return Ok(order);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.orderCoffes
                .Select(o => new
                {
                    o.Id,
                    o.Id_Account,
                    o.TimeOrder,
                    o.Status,
                    o.Deleted,
                    OrderDetails = _context.orderDetailProducts
                        .Where(od => od.Id_Order == o.Id)
                        .Select(od => new
                        {
                            od.Id_Product,
                            ProductName = _context.ProductCoffee
                                .Where(p => p.Id == od.Id_Product)
                                .Select(p => p.Name)
                                .FirstOrDefault(),
                            od.Quantity,
                            od.Price
                        }).ToList(),
                    PaymentForms = _context.paymentForms
                        .Where(p => p.Id_Order == o.Id)
                        .Select(p => new
                        {
                            p.Sum_Price,
                            p.Payment_Method
                        }).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }



    }
}