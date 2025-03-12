using QuanLyCafe.Models;

public class CreatOrder
{
    public int Id_Account { get; set; }
    public DateTime TimeOrder { get; set; }

    public bool Status { get; set; }

    public bool Deleted { get; set; }

    public List<OrderProductDto> orderProductDtos { get; set; }

    public List<PaymentFormOrderDto> paymentForms { get; set; }

}

public class OrderProductDto
{
    public int ProductCoffeeId { get; set; }

    public int QuantityProduct { get; set; }
    public List<StockProductDto> stockProductDtos { get; set; }



}

public class StockProductDto
{
    public int ID_Stock { get; set; }
    public int QuantityStock { get; set; }
}


public class PaymentFormOrderDto
{
    public int SumPrice { get; set; }

    public string PaymentMethod { get; set; }

}

