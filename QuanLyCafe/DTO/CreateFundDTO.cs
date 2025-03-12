public class FundCreateDTO
{
    public int Id { get; set; }

    public decimal SumPrice { get; set; }

    public string detail_status { get; set; }

    public DateTime create_at { get; set; }

    public string FundName { get; set; }


}

public class FundUpdateDTO
{
    public decimal SumPrice { get; set; }

    public string detail_status { get; set; }

    public DateTime create_at { get; set; }

    public string FundName { get; set; }
}