using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using TodoApi.Helpers;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System;


namespace TodoApi.Services
{
    /*
     public interface IUserService
     {
         AuthenticateResponse Authenticate(Customer customer);

         Customer Login(LoginModel model);

         AuthenticateResponse Register(Customer customer);
         IEnumerable<Customer> GetAll();
         Customer GetById(int id);
     }
    */
    public class CustomerService
    {
        private readonly IMongoCollection<Customer> _customers;

        private readonly AppSettings _appSettings;

        public CustomerService(IBookstoreDatabaseSettings settings, IOptions<AppSettings> appSettings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _customers = database.GetCollection<Customer>(settings.CustomersCollectionName);
            _appSettings = appSettings.Value;
        }

        public Customer Login(LoginModel model)
        {
            return _customers.Find(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefault();
        }

        public AuthenticateResponse Authenticate(Customer customer)
        {
            //var customer = (Customer)_customers.Find(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            // if (customer == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(customer);

            return new AuthenticateResponse(customer, token);
        }

        public Customer Register(Customer customer)
        {
            var findCustomer = _customers.Find(x => x.Username == customer.Username).FirstOrDefault();

            // return null if user not found
             if (findCustomer != null) return null;
            _customers.InsertOne(customer);

            return customer;
        }

        public IEnumerable<Customer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Customer GetById(string id)
        {
            return _customers.Find(user => user.Id == id).FirstOrDefault();
        }

        // helper methods

        private string GenerateJwtToken(Customer customer)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", customer.Id) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
