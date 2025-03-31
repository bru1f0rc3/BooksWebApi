using WebApplication2.Connection;
using WebApplication2.DTO.Category;

namespace WebApplication2.Services.Category
{
    public class CategoryService
    {
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            const string sql = "SELECT * FROM \"Categories\" ORDER BY name";
            var categories = await DbConnect.QueryAsync<CategoryDTO>(sql);
            return categories.ToList();
        }

        public async Task<CategoryDTO?> GetCategoryById(int id)
        {
            const string sql = "SELECT * FROM \"Categories\" WHERE id = @id";
            return await DbConnect.QueryFirstOrDefaultAsync<CategoryDTO>(sql, new { id = id });
        }

        public async Task<CategoryDTO> CreateCategory(CreateCategoryDTO category)
        {
            const string sql = @"
                INSERT INTO ""Categories"" (name)
                VALUES (@name)
                RETURNING *";
            
            return await DbConnect.QueryFirstOrDefaultAsync<CategoryDTO>(sql, new { Name = category.name });
        }

        public async Task<CategoryDTO?> UpdateCategory(int id, UpdateCategoryDTO category)
        {
            const string sql = @"
                UPDATE ""Categories""
                SET name = @name
                WHERE id = @id
                RETURNING *";
            
            return await DbConnect.QueryFirstOrDefaultAsync<CategoryDTO>(sql, new { id = id, Name = category.name });
        }

        public async Task<bool> DeleteCategory(int id)
        {
            const string sql = "DELETE FROM \"Categories\" WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql, new { id = id });
            return rowsAffected > 0;
        }
    }
} 