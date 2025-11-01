using WebApplication2.Connection;
using WebApplication2.DTO.Book;
using Dapper;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.Book
{
    /// <summary>
    /// Сервис для работы с книгами в базе данных
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IFileService _fileService;

        /// <summary>
        /// Конструктор сервиса книг
        /// </summary>
        /// <param name="fileService">Сервис для работы с файлами</param>
        public BookService(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Получить полный список всех книг с информацией об авторах, категориях и филиалах
        /// </summary>
        /// <returns>Список всех книг в библиотеке</returns>
        public async Task<List<BookListDTO>> BookListedGet()
        {
            const string listBook = @"
                SELECT 
                    books.id, 
                    books.title, 
                    books.description, 
                    books.fragment, 
                    books.cover_link,
                    books.branch_id, 
                    books.author_id,
                    books.category_id,
                    authors.full_name as author_name,
                    branches.name as branch_name, 
                    categories.name as category_name 
                FROM books 
                JOIN authors ON authors.id = books.author_id
                JOIN categories ON categories.id = books.category_id
                JOIN branches ON branches.id = books.branch_id";
            
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

        /// <summary>
        /// Добавить новую книгу в базу данных
        /// </summary>
        /// <param name="book">Данные новой книги</param>
        /// <param name="coverImage">Файл изображения обложки книги (опционально)</param>
        public async Task AddBook(AddBook book, IFormFile? coverImage)
        {
            string coverLink = book.CoverLink;
            if (coverImage != null)
            {
                coverLink = await _fileService.SaveImage(coverImage);
            }

            const string addBook = @"
                INSERT INTO books (title, description, fragment, cover_link, author_id, branch_id, category_id) 
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

        /// <summary>
        /// Редактировать существующую книгу в базе данных
        /// </summary>
        /// <param name="book">Обновленные данные книги</param>
        /// <param name="coverImage">Новый файл изображения обложки (опционально)</param>
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
                UPDATE books 
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

        /// <summary>
        /// Удалить книгу из базы данных по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор книги для удаления</param>
        public async Task RemoveBook(int id)
        {
            const string getBookSql = "SELECT cover_link FROM books WHERE id = @Id";
            var book = await DbConnect.QueryFirstOrDefaultAsync<dynamic>(getBookSql, new { Id = id });
            
            if (book != null && !string.IsNullOrEmpty(book.cover_link))
            {
                _fileService.DeleteImage(book.cover_link);
            }

            const string removeBook = "DELETE FROM books WHERE id = @Id";
            await DbConnect.ExecuteAsync(removeBook, new { Id = id });
        }

        /// <summary>
        /// Поиск книг по названию, автору или категории
        /// </summary>
        /// <param name="searchTerm">Поисковый запрос для поиска по названию или автору</param>
        /// <param name="categoryId">Идентификатор категории для фильтрации (опционально)</param>
        /// <returns>Список книг, соответствующих критериям поиска</returns>
        public async Task<List<BookListDTO>> SearchBooks(string? searchTerm, int? categoryId = null)
        {
            var sql = @"
                SELECT 
                    books.id, 
                    books.title, 
                    books.description, 
                    books.fragment, 
                    books.cover_link, 
                    authors.full_name as author_name,
                    branches.name as branch_name, 
                    categories.name as category_name 
                FROM books 
                JOIN authors ON authors.id = books.author_id
                JOIN categories ON categories.id = books.category_id
                JOIN branches ON branches.id = books.branch_id
                WHERE 1=1";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += @" AND (books.title ILIKE @SearchTerm OR authors.full_name ILIKE @SearchTerm)";
                parameters.Add("SearchTerm", $"%{searchTerm}%");
            }

            if (categoryId.HasValue)
            {
                sql += @" AND books.category_id = @CategoryId";
                parameters.Add("CategoryId", categoryId.Value);
            }

            var books = await DbConnect.QueryAsync<BookListDTO>(sql, parameters);
            return books.ToList();
        }

        /// <summary>
        /// Получить детальную информацию о конкретной книге
        /// </summary>
        /// <param name="bookId">Идентификатор книги</param>
        /// <returns>Детальная информация о книге</returns>
        /// <exception cref="Exception">Выбрасывается, если книга не найдена</exception>
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
                FROM books b
                JOIN authors au ON au.id = b.author_id
                JOIN categories c ON c.id = b.category_id
                JOIN branches br ON br.id = b.branch_id
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
