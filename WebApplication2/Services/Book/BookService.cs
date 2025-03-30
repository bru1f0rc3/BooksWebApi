using WebApplication2.Models;
using WebApplication2.Connection;
using WebApplication2.DTO.Book;
using Dapper;

namespace WebApplication2.Services.Book
{
    public class BookService
    {
        public async Task<List<BookListDTO>> BookListedGet()
        {
            const string listBook = @"
                SELECT 
                    ""Books"".id, 
                    ""Books"".title, 
                    ""Books"".description, 
                    ""Books"".fragment, 
                    ""Books"".cover_link, 
                    ""Authors"".full_name as author_name,
                    ""Branches"".name as branch_name, 
                    ""Categories"".name as category_name 
                FROM ""Books"" 
                JOIN ""Authors"" ON ""Authors"".id = ""Books"".author_id
                JOIN ""Categories"" ON ""Categories"".id = ""Books"".category_id
                JOIN ""Branches"" ON ""Branches"".id = ""Books"".branch_id";
            
            var books = await DbConnect.QueryAsync<BookListDTO>(listBook);
            return books.ToList();
        }

        public async Task AddBook(AddBook book)
        {
            const string addBook = @"
                INSERT INTO ""Books"" (title, description, fragment, cover_link, author_id, branch_id, category_id) 
                VALUES (@Title, @Description, @Fragment, @CoverLink, @AuthorId, @BranchId, @CategoryId)";
            
            await DbConnect.ExecuteAsync(addBook, book);
        }

        public async Task EditBook(EditBook book)
        {
            const string editBook = @"
                UPDATE ""Books"" 
                SET title = @Title,
                    description = @Description,
                    fragment = @Fragment,
                    cover_link = @CoverLink,
                    author_id = @AuthorId,
                    branch_id = @BranchId,
                    category_id = @CategoryId
                WHERE id = @Id";
            
            await DbConnect.ExecuteAsync(editBook, book);
        }

        public async Task RemoveBook(int id)
        {
            const string removeBook = @"
                DELETE FROM ""Books"" 
                WHERE id = @Id";
            
            await DbConnect.ExecuteAsync(removeBook, new { Id = id });
        }

        public async Task<List<BookListDTO>> SearchBooks(string? searchTerm, int? categoryId = null)
        {
            var sql = @"
                SELECT 
                    ""Books"".id, 
                    ""Books"".title, 
                    ""Books"".description, 
                    ""Books"".fragment, 
                    ""Books"".cover_link, 
                    ""Authors"".full_name as author_name,
                    ""Branches"".name as branch_name, 
                    ""Categories"".name as category_name 
                FROM ""Books"" 
                JOIN ""Authors"" ON ""Authors"".id = ""Books"".author_id
                JOIN ""Categories"" ON ""Categories"".id = ""Books"".category_id
                JOIN ""Branches"" ON ""Branches"".id = ""Books"".branch_id
                WHERE 1=1";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += @" AND (""Books"".title ILIKE @SearchTerm OR ""Authors"".full_name ILIKE @SearchTerm)";
                parameters.Add("SearchTerm", $"%{searchTerm}%");
            }

            if (categoryId.HasValue)
            {
                sql += @" AND ""Books"".category_id = @CategoryId";
                parameters.Add("CategoryId", categoryId.Value);
            }

            var books = await DbConnect.QueryAsync<BookListDTO>(sql, parameters);
            return books.ToList();
        }
    }
}
