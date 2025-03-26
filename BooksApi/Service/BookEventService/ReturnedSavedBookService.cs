using BooksApi.Models.EventBook;
using Supabase;
using BooksApi.DTO.EventBook;
using BooksApi.Models;
using Supabase.Postgrest.Models;
using System.Linq;
using BooksApi.Models.Book;

namespace BooksApi.Service.BookEventService
{
    public class ReturnedSavedBookService
    {
        private readonly Client _supabaseClient;

        public ReturnedSavedBookService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
        }

        public async Task ReturnedBook(Returned_Books requests)
        {
            try
            {
                if (requests == null)
                {
                    throw new ArgumentNullException(nameof(requests));
                }

                var returnedBook = new Returned_Books
                {
                    BookId = requests.BookId,
                    AccountId = requests.AccountId,
                    CreatedAt = DateTime.UtcNow
                };

                await _supabaseClient.From<Returned_Books>().Insert(returnedBook);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при возврате книги: {ex.Message}", ex);
            }
        }

        public async Task SavedBook(Saved_Books requests)
        {
            try
            {
                if (requests == null)
                {
                    throw new ArgumentNullException(nameof(requests));
                }

                // Проверяем, не сохранена ли уже эта книга
                var existingBook = await _supabaseClient
                    .From<Saved_Books>()
                    .Select("*")
                    .Where(x => x.BookId == requests.BookId && x.AccountId == requests.AccountId)
                    .Get();

                if (existingBook.Models.Any())
                {
                    return;
                }

                var savedBook = new Saved_Books
                {
                    BookId = requests.BookId,
                    AccountId = requests.AccountId,
                    CreatedAt = DateTime.UtcNow
                };

                await _supabaseClient.From<Saved_Books>().Insert(savedBook);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении книги: {ex.Message}", ex);
            }
        }

        public async Task<List<SavedBookDetailsDTO>> GetUserSavedBooks(int accountId)
        {
            try
            {
                if (accountId <= 0)
                {
                    throw new ArgumentException("Некорректный ID пользователя", nameof(accountId));
                }

                // Получаем сохраненные книги
                var savedBooks = await _supabaseClient
                    .From<Saved_Books>()
                    .Select("*")
                    .Where(x => x.AccountId == accountId)
                    .Get();

                if (!savedBooks.Models.Any())
                {
                    return new List<SavedBookDetailsDTO>();
                }

                var result = new List<SavedBookDetailsDTO>();

                foreach (var savedBook in savedBooks.Models)
                {
                    // Получаем информацию о книге
                    var book = await _supabaseClient
                        .From<Bookss>()
                        .Select("id, title, cover_link")
                        .Match(new Dictionary<string, string> { { "id", savedBook.BookId.ToString() } })
                        .Get();

                    if (book.Models.Count > 0)
                    {
                        result.Add(new SavedBookDetailsDTO
                        {
                            Id = savedBook.Id,
                            BookTitle = book.Models[0].Title,
                            CoverLink = book.Models[0].Cover_Link,
                            CreatedAt = savedBook.CreatedAt
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении сохраненных книг: {ex.Message}", ex);
            }
        }

        public async Task<List<ReturnedBookDetailsDTO>> GetUserReturnedBooks(int accountId)
        {
            try
            {
                if (accountId <= 0)
                {
                    throw new ArgumentException("Некорректный ID пользователя", nameof(accountId));
                }

                // Получаем возвращенные книги
                var returnedBooks = await _supabaseClient
                    .From<Returned_Books>()
                    .Select("*")
                    .Where(x => x.AccountId == accountId)
                    .Get();

                if (!returnedBooks.Models.Any())
                {
                    return new List<ReturnedBookDetailsDTO>();
                }

                var result = new List<ReturnedBookDetailsDTO>();

                foreach (var returnedBook in returnedBooks.Models)
                {
                    // Получаем информацию о книге
                    var book = await _supabaseClient
                        .From<Bookss>()
                        .Select("id, title, cover_link")
                        .Match(new Dictionary<string, string> { { "id", returnedBook.BookId.ToString() } })
                        .Get();

                    if (book.Models.Count > 0)
                    {
                        result.Add(new ReturnedBookDetailsDTO
                        {
                            Id = returnedBook.Id,
                            AccountId = accountId,
                            BookTitle = book.Models[0].Title,
                            CoverLink = book.Models[0].Cover_Link,
                            CreatedAt = returnedBook.CreatedAt
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении возвращенных книг: {ex.Message}", ex);
            }
        }
    }
}
