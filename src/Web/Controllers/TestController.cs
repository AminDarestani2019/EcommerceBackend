using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Web.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("testdbconnection")]
        public IActionResult TestDbConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT 1", connection))
                    {
                        var result = command.ExecuteScalar();
                        return Ok("Connection successful!");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Connection failed: {ex.Message}");
            }
        }
    }
}
