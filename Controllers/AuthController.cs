using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Helpers;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private CustomerService _customerService;

        public AuthController(CustomerService userService)
        {
            _customerService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            var UserExist = _customerService.Login(model);

            if (UserExist == null)
                return BadRequest(new { message = "Username or password is incorrect." });

            var response = _customerService.Authenticate(UserExist);

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(Customer customer)
        {
            var customerExist = _customerService.Register(customer);

            if (customerExist == null)
                return BadRequest(new { message = "Username is exist, please try again." });

           // var response = _customerService.Authenticate(customerExist);

            return Ok(customerExist);
        }

        [Authorize]
        [HttpGet("users")]
        public IActionResult GetAll()
        {
            var users = _customerService.GetAll();
            return Ok(users);
        }
    }
}
