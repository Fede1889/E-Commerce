using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;


namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;  //per poter accedere al database 

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        //--- DA QUI INIZIANO I METODI DELL'INTERFACCIA IProductRepository ---
        public async Task<IEnumerable<Product>> GetProducts()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            return await connection.QueryAsync<Product>("SELECT * FROM Catalog");
        }

        public async Task<Product> GetProduct(string id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var product = await connection.QueryFirstOrDefaultAsync<Product>
                ("SELECT * FROM Catalog WHERE Id = @Id", new { Id = id });

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var product = await connection.QueryAsync<Product>
                ("SELECT * FROM Catalog WHERE Name = @Name", new { Name = name });

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            return await connection.QueryAsync<Product>
                ("SELECT * FROM Catalog WHERE Category = @Category", new { Category = categoryName });
        }


        public async Task<bool> CreateProduct(Product product)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =await connection.ExecuteAsync
                    ("INSERT INTO Catalog (Id,Name, Category, Description, Price, Quantity) VALUES (@Id,@Name, @Category, @Description, @Price, @Quantity)",
                            new { Id = product.Id,Name = product.Name, Category = product.Category, Description = product.Description, Price = product.Price, Quantity = product.Quantity });
            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE Catalog SET Name=@Name, Category=@Category, Description = @Description, Price = @Price, Quantity=@Quantity WHERE Id = @Id",
                            new { Name = product.Name, Category = product.Category, Description = product.Description, Price = product.Price, Quantity = product.Quantity, Id = product.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Catalog WHERE Id = @Id",
                new { Id = id });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
