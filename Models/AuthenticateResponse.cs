
namespace TodoApi.Models
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(Customer customer, string token)
        {
            Id = customer.Id;
            Email = customer.Email;
            Username = customer.Username;
            Token = token;
        }
    }
}
