namespace Cwiczenie6.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

[ApiController]
[Route("[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly string _connectionString;

    public WarehouseController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    [HttpPost]
    [Route("add-product")]
    public IActionResult AddProductToWarehouse([FromBody] ProductWarehouseRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest("Amount must be greater than 0.");
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Check if Product exists
                    var checkProductCommand = new SqlCommand("SELECT COUNT(1) FROM Product WHERE Id = @ProductId", connection, transaction);
                    checkProductCommand.Parameters.AddWithValue("@ProductId", request.ProductId);
                    var productExists = (int)checkProductCommand.ExecuteScalar() > 0;

                    if (!productExists)
                    {
                        return NotFound("Product not found.");
                    }

                    // Check if Warehouse exists
                    var checkWarehouseCommand = new SqlCommand("SELECT COUNT(1) FROM Warehouse WHERE Id = @WarehouseId", connection, transaction);
                    checkWarehouseCommand.Parameters.AddWithValue("@WarehouseId", request.WarehouseId);
                    var warehouseExists = (int)checkWarehouseCommand.ExecuteScalar() > 0;

                    if (!warehouseExists)
                    {
                        return NotFound("Warehouse not found.");
                    }

                    // Check if Order exists and is valid
                    var checkOrderCommand = new SqlCommand("SELECT Id FROM [Order] WHERE ProductId = @ProductId AND Amount = @Amount AND CreatedAt < @CreatedAt AND FullfilledAt IS NULL", connection, transaction);
                    checkOrderCommand.Parameters.AddWithValue("@ProductId", request.ProductId);
                    checkOrderCommand.Parameters.AddWithValue("@Amount", request.Amount);
                    checkOrderCommand.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);
                    var orderId = (int?)checkOrderCommand.ExecuteScalar();

                    if (orderId == null)
                    {
                        return BadRequest("No valid order found.");
                    }

                    // Update Order FullfilledAt
                    var updateOrderCommand = new SqlCommand("UPDATE [Order] SET FullfilledAt = @FullfilledAt WHERE Id = @OrderId", connection, transaction);
                    updateOrderCommand.Parameters.AddWithValue("@FullfilledAt", DateTime.UtcNow);
                    updateOrderCommand.Parameters.AddWithValue("@OrderId", orderId);
                    updateOrderCommand.ExecuteNonQuery();

                    // Insert into Product_Warehouse
                    var insertProductWarehouseCommand = new SqlCommand("INSERT INTO Product_Warehouse (ProductId, WarehouseId, Amount, Price, CreatedAt) VALUES (@ProductId, @WarehouseId, @Amount, @Price, @CreatedAt); SELECT SCOPE_IDENTITY();", connection, transaction);
                    insertProductWarehouseCommand.Parameters.AddWithValue("@ProductId", request.ProductId);
                    insertProductWarehouseCommand.Parameters.AddWithValue("@WarehouseId", request.WarehouseId);
                    insertProductWarehouseCommand.Parameters.AddWithValue("@Amount", request.Amount);
                    insertProductWarehouseCommand.Parameters.AddWithValue("@Price", GetProductPrice(request.ProductId) * request.Amount);
                    insertProductWarehouseCommand.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                    var productWarehouseId = (int)(decimal)insertProductWarehouseCommand.ExecuteScalar();

                    transaction.Commit();
                    return Ok(new { Id = productWarehouseId });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
        }
    }
    [HttpPost]
    [Route("add-product-stored-procedure")]
    public IActionResult AddProductToWarehouseWithSP([FromBody] ProductWarehouseRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest("Amount must be greater than 0.");
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("sp_AddProductToWarehouse", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductId", request.ProductId);
                command.Parameters.AddWithValue("@WarehouseId", request.WarehouseId);
                command.Parameters.AddWithValue("@Amount", request.Amount);
                command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);
            
                try
                {
                    var result = command.ExecuteScalar();
                    return Ok(new { Id = (int)(decimal)result });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
        }
    }


    private decimal GetProductPrice(int productId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var getPriceCommand = new SqlCommand("SELECT Price FROM Product WHERE Id = @ProductId", connection);
            getPriceCommand.Parameters.AddWithValue("@ProductId", productId);
            return (decimal)getPriceCommand.ExecuteScalar();
        }
    }
}
