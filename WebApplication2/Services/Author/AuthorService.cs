using WebApplication2.Connection;
using WebApplication2.DTO.Author;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.Author
{
    /// <summary>
    /// Сервис для работы с авторами книг
    /// </summary>
    public class AuthorService : IAuthorService
    {
        public async Task<List<AuthorDTO>> GetAllAuthors()
        {
            const string sql = @"SELECT id, full_name FROM authors ORDER BY full_name";
            var authors = await DbConnect.QueryAsync<AuthorDTO>(sql);
            return authors.ToList();
        }

        public async Task<AuthorDTO?> GetAuthorById(int id)
        {
            const string sql = @"SELECT id, full_name FROM authors WHERE id = @id";
            return await DbConnect.QueryFirstOrDefaultAsync<AuthorDTO>(sql, new { id });
        }

        public async Task<AuthorDTO> CreateAuthor(CreateAuthorDTO author)
        {
            const string sql = @"
                INSERT INTO authors (full_name)
                VALUES (@full_name)
                RETURNING id, full_name";
            
            return await DbConnect.QueryFirstOrDefaultAsync<AuthorDTO>(sql, new { full_name = author.full_name });
        }

        public async Task<AuthorDTO?> UpdateAuthor(int id, UpdateAuthorDTO author)
        {
            const string sql = @"
                UPDATE authors
                SET full_name = @full_name
                WHERE id = @id
                RETURNING id, full_name";
            
            return await DbConnect.QueryFirstOrDefaultAsync<AuthorDTO>(sql, new { id, full_name = author.full_name });
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            const string sql = @"DELETE FROM authors WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql, new { id });
            return rowsAffected > 0;
        }
    }
} 