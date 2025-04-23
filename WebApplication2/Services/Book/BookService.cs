using WebApplication2.Connection;
using WebApplication2.DTO.Book;
using Dapper;
using WebApplication2.Services.File;

namespace WebApplication2.Services.Book
{
    public class BookService
    {
        private readonly FileService _fileService;

        public BookService(FileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<List<BookListDTO>> BookListedGet()
        {
            const string listBook = @"
                SELECT 
                    ""Books"".id, 
                    ""Books"".title, 
                    ""Books"".description, 
                    ""Books"".fragment, 
                    ""Books"".cover_link,
                    ""Books"".branch_id, 
                    ""Books"".author_id,
                    ""Books"".category_id,
                    ""Authors"".full_name as author_name,
                    ""Branches"".name as branch_name, 
                    ""Categories"".name as category_name 
                FROM ""Books"" 
                JOIN ""Authors"" ON ""Authors"".id = ""Books"".author_id
                JOIN ""Categories"" ON ""Categories"".id = ""Books"".category_id
                JOIN ""Branches"" ON ""Branches"".id = ""Books"".branch_id";
            
            var books = await DbConnect.QueryAsync<BookListDTO>(listBook);

            foreach (var book in books)
            {
                if (!string.IsNullOrEmpty(book.cover_link))
                {
                    if (!book.cover_link.StartsWith("http://") && !book.cover_link.StartsWith("https://"))
                    {
                        book.cover_link = $"/coverlink/{book.cover_link}";
                    }
                }
            }

            return books.ToList();
        }

        public async Task AddBook(AddBook book, IFormFile? coverImage)
        {
            string coverLink = book.CoverLink;
            if (coverImage != null)
            {
                coverLink = await _fileService.SaveImage(coverImage);
            }

            const string addBook = @"
                INSERT INTO ""Books"" (title, description, fragment, cover_link, author_id, branch_id, category_id) 
                VALUES (@Title, @Description, @Fragment, @CoverLink, @AuthorId, @BranchId, @CategoryId)";
            
            await DbConnect.ExecuteAsync(addBook, new
            {
                book.Title,
                book.Description,
                book.Fragment,
                CoverLink = coverLink,
                book.AuthorId,
                book.BranchId,
                book.CategoryId
            });
        }

        public async Task EditBook(EditBook book, IFormFile? coverImage)
        {
            string coverLink = book.CoverLink;
            if (coverImage != null)
            {
                if (!string.IsNullOrEmpty(book.CoverLink))
                {
                    _fileService.DeleteImage(book.CoverLink);
                }
                coverLink = await _fileService.SaveImage(coverImage);
            }

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
            
            await DbConnect.ExecuteAsync(editBook, new
            {
                book.Id,
                book.Title,
                book.Description,
                book.Fragment,
                CoverLink = coverLink,
                book.AuthorId,
                book.BranchId,
                book.CategoryId
            });
        }

        public async Task RemoveBook(int id)
        {
            const string getBookSql = "SELECT cover_link FROM \"Books\" WHERE id = @Id";
            var book = await DbConnect.QueryFirstOrDefaultAsync<dynamic>(getBookSql, new { Id = id });
            
            if (book != null && !string.IsNullOrEmpty(book.cover_link))
            {
                _fileService.DeleteImage(book.cover_link);
            }

            const string removeBook = "DELETE FROM \"Books\" WHERE id = @Id";
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

        public async Task<BookDetailDTO> GetBookDetail(int bookId)
        {
            const string sql = @"
                SELECT 
                    b.id,
                    b.title,
                    b.description,
                    b.fragment,
                    b.cover_link,
                    b.author_id,
                    au.full_name as author_name,
                    b.category_id,
                    c.name as category_name,
                    b.branch_id,
                    br.name as branch_name
                FROM ""Books"" b
                JOIN ""Authors"" au ON au.id = b.author_id
                JOIN ""Categories"" c ON c.id = b.category_id
                JOIN ""Branches"" br ON br.id = b.branch_id
                WHERE b.id = @BookId";

            var book = await DbConnect.QueryFirstOrDefaultAsync<BookDetailDTO>(sql, new { BookId = bookId });
            if (book == null)
            {
                throw new Exception("Книга не найдена");
            }

            if (!string.IsNullOrEmpty(book.cover_link))
            {
                if (!book.cover_link.StartsWith("http://") && !book.cover_link.StartsWith("https://"))
                {
                    book.cover_link = $"/coverlink/{book.cover_link}";
                }
            }

            return book;
        }
    }
}
