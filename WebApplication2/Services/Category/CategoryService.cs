using WebApplication2.Connection;
using WebApplication2.DTO.Category;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.Category
{
    /// <summary>
    /// Сервис для работы с категориями книг
    /// </summary>
    public class CategoryService : ICategoryService
    {
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            const string sql = "SELECT * FROM categories ORDER BY name";
            var categories = await DbConnect.QueryAsync<CategoryDTO>(sql);
            return categories.ToList();
        }

        public async Task<CategoryDTO?> GetCategoryById(int id)
        {
            const string sql = "SELECT * FROM categories WHERE id = @id";
            return await DbConnect.QueryFirstOrDefaultAsync<CategoryDTO>(sql, new { id = id });
        }

        public async Task<CategoryDTO> CreateCategory(CreateCategoryDTO category)
        {
            const string sql = @"
                INSERT INTO categories (name)
                VALUES (@name)
                RETURNING *";
            
            return await DbConnect.QueryFirstOrDefaultAsync<CategoryDTO>(sql, new { Name = category.name });
        }

        public async Task<CategoryDTO?> UpdateCategory(int id, UpdateCategoryDTO category)
        {
            const string sql = @"
                UPDATE categories
                SET name = @name
                WHERE id = @id
                RETURNING *";
            
            return await DbConnect.QueryFirstOrDefaultAsync<CategoryDTO>(sql, new { id = id, Name = category.name });
        }

        public async Task<bool> DeleteCategory(int id)
        {
            const string sql = "DELETE FROM categories WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql, new { id = id });
            return rowsAffected > 0;
        }
    }
} 