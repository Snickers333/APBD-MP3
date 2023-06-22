using Exercise3.Models;
using Exercise3.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Exercise3.Repositories
{
    public interface IAnimalsRepository
    {
        Task<Animal> PutAnimalAsync(int id, AnimalPUT animalPUT);
        Task<ICollection<Animal>> GetAnimalsAsync(string orderBy);
        void AddAnimal(AnimalPOST animalPOST);
        Task RemAnimal(int id);
        Task<bool> DoesAnimalExist(int id);
    }
    public class AnimalsRepository : IAnimalsRepository
    {
        private readonly string _connectionString;
        public AnimalsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
            ?? throw new ArgumentException(nameof(configuration));
        }

        public async void AddAnimal(AnimalPOST animalPOST)
        {
            string query = "INSERT INTO animal (id, name, description, category, area) " +
                   "VALUES (@Id, @Name, @Description, @Category, @Area)";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@Id", animalPOST.ID);
                sqlCommand.Parameters.AddWithValue("@Name", animalPOST.Name);
                sqlCommand.Parameters.AddWithValue("@Description", animalPOST.Description);
                sqlCommand.Parameters.AddWithValue("@Category", animalPOST.Category);
                sqlCommand.Parameters.AddWithValue("@Area", animalPOST.Area);

                await connection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> DoesAnimalExist(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = $"SELECT COUNT(*) FROM animal WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task<ICollection<Animal>> GetAnimalsAsync(string orderBy)
        {
            if (orderBy != "name" && orderBy != "description" && orderBy != "category" && orderBy != "area")
            {
                orderBy = "name";
            }

            var list = new List<Animal>();
            string query = $"SELECT * FROM animal ORDER BY {orderBy} ASC";
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));
                        string description = reader.GetString(reader.GetOrdinal("Description"));
                        string category = reader.GetString(reader.GetOrdinal("Category"));
                        string area = reader.GetString(reader.GetOrdinal("Area"));

                        list.Add(new Animal
                        {
                            ID = id,
                            Name = name,
                            Description = description,
                            Category = category,
                            Area = area
                        });

                    }
                }
            }
            return list;
        }

        public async Task<Animal> PutAnimalAsync(int animalID, AnimalPUT animalPUT)
        {
            string query = "UPDATE animal SET name = @name, description = @description, category = @category, area = @area WHERE id = @id";
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@id", animalID);
                sqlCommand.Parameters.AddWithValue("@name", animalPUT.Name);
                sqlCommand.Parameters.AddWithValue("@description", animalPUT.Description ?? "");
                sqlCommand.Parameters.AddWithValue("@category", animalPUT.Category);
                sqlCommand.Parameters.AddWithValue("@area", animalPUT.Area);

                await connection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
                return new Animal
                {
                    ID = animalID,
                    Name = animalPUT.Name,
                    Description = animalPUT.Description ?? "",
                    Category = animalPUT.Category,
                    Area = animalPUT.Area
                };
            }
            
        }

        public async Task RemAnimal(int id)
        {
            string query = "DELETE FROM animal WHERE id = @id";
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@id", id);

                await connection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
