public class UpdateAccountDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Password { get; set; } // Có thể để trống nếu không đổi mật khẩu
    public bool Status { get; set; }
    public bool Deleted { get; set; }
}
