public class CreateAccountDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    
    public DateTime Creat_At{ get; set; }
    
    public bool Status { get; set; }
    
    public bool Deleted { get; set; }   
    
    
    
}
