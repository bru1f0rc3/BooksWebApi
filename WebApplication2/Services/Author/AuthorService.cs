using WebApplication2.Connection;
using WebApplication2.DTO.Author;

namespace WebApplication2.Services.Author
{
    public class AuthorService
    {
        public async Task<List<AuthorDTO>> GetAllAuthors()
        {
            const string sql = @"SELECT id, full_name FROM ""Authors"" ORDER BY full_name";
            var authors = await DbConnect.QueryAsync<AuthorDTO>(sql);
            return authors.ToList();
        }

        public async Task<AuthorDTO?> GetAuthorById(int id)
        {
            const string sql = @"SELECT id, full_name FROM ""Authors"" WHERE id = @id";
            return await DbConnect.QueryFirstOrDefaultAsync<AuthorDTO>(sql, new { id });
        }

        public async Task<AuthorDTO> CreateAuthor(CreateAuthorDTO author)
        {
            const string sql = @"
                INSERT INTO ""Authors"" (full_name)
                VALUES (@full_name)
                RETURNING id, full_name";
            
            return await DbConnect.QueryFirstOrDefaultAsync<AuthorDTO>(sql, new { full_name = author.full_name });
        }

        public async Task<AuthorDTO?> UpdateAuthor(int id, UpdateAuthorDTO author)
        {
            const string sql = @"
                UPDATE ""Authors""
                SET full_name = @full_name
                WHERE id = @id
                RETURNING id, full_name";
            
            return await DbConnect.QueryFirstOrDefaultAsync<AuthorDTO>(sql, new { id, full_name = author.full_name });
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            const string sql = @"DELETE FROM ""Authors"" WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql, new { id });
            return rowsAffected > 0;
        }
    }
} 