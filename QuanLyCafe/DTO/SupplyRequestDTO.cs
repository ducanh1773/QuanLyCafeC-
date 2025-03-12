

public class SupplyRequestDto
{
    public string UserName { get; set; }
    public List<StockRequestDto> Stocks { get; set; } // Danh sách sản phẩm
}


public class StockRequestDto
{
    public string NameStock { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string PaymentMethod { get; set; }
}
