using WebApplication2.Connection;
using WebApplication2.DTO.BookEvent;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.BookEvent
{
    /// <summary>
    /// Сервис для работы с событиями книг (выдача, возврат, бронирование)
    /// </summary>
    public class BookEventService : IBookEventService
    {
        public async Task<List<BookEventListDTO>> GetBookEvents()
        {
            const string sql = @"
                SELECT 
                    be.id as bookevent_id,
                    b.title as book_title,
                    a.full_name as user_name,
                    a.id as account_id,
                    et.name as event_type_name
                FROM book_events be
                JOIN books b ON b.id = be.book_id
                JOIN accounts a ON a.id = be.account_id
                JOIN event_types et ON et.id = be.event_type_id
                ORDER BY be.event_date DESC";
            
            var events = await DbConnect.QueryAsync<BookEventListDTO>(sql);
            return events.ToList();
        }

        public async Task<List<BookEventListDTO>> GetActiveRequests()
        {
                const string sql = @"
                SELECT 
                    be.id as bookevent_id,
                    b.title as book_title,
                    a.full_name as user_name,
                    a.id as account_id,
                    et.name as event_type_name
                FROM book_events be
                JOIN books b ON b.id = be.book_id
                JOIN accounts a ON a.id = be.account_id
                JOIN event_types et ON et.id = be.event_type_id
                WHERE be.event_type_id = 1
                ORDER BY be.event_date DESC";
            
            var requests = await DbConnect.QueryAsync<BookEventListDTO>(sql);
            return requests.ToList();
        }

        public async Task<List<UserBookHistoryDTO>> GetUserBookHistory(int accountId)
        {
            const string sql = @"
                WITH LastEvents AS (
                    SELECT 
                        book_id,
                        MAX(event_date) as last_event_date,
                        MAX(event_type_id) as last_event_type_id
                    FROM book_events
                    WHERE account_id = @AccountId
                    GROUP BY book_id
                )
                SELECT 
                    b.id as book_id,
                    b.title as book_title,
                    a.full_name as author_name,
                    c.name as category_name,
                    br.name as branch_name,
                    le.last_event_date,
                    et.name as last_event_type
                FROM LastEvents le
                JOIN books b ON b.id = le.book_id
                JOIN authors a ON a.id = b.author_id
                JOIN categories c ON c.id = b.category_id
                JOIN branches br ON br.id = b.branch_id
                JOIN event_types et ON et.id = le.last_event_type_id
                ORDER BY le.last_event_date DESC";
            
            var history = await DbConnect.QueryAsync<UserBookHistoryDTO>(sql, new { AccountId = accountId });
            return history.ToList();
        }

        public async Task<List<UserBookEventDTO>> GetUserBooksByEventType(int accountId, int eventTypeId)
        {
            const string sql = @"
                SELECT 
                    be.id as book_event_id,
                    b.id as book_id,
                    b.title as book_title,
                    au.full_name as author_name,
                    c.name as category_name,
                    br.name as branch_name,
                    be.event_date,
                    et.name as event_type_name
                FROM book_events be
                JOIN books b ON b.id = be.book_id
                JOIN authors au ON au.id = b.author_id
                JOIN categories c ON c.id = b.category_id
                JOIN branches br ON br.id = b.branch_id
                JOIN event_types et ON et.id = be.event_type_id
                WHERE be.account_id = @AccountId 
                AND be.event_type_id = @EventTypeId
                ORDER BY be.event_date DESC";
            
            var books = await DbConnect.QueryAsync<UserBookEventDTO>(sql, new { AccountId = accountId, EventTypeId = eventTypeId });
            return books.ToList();
        }

        public async Task RequestBook(int accountId, int bookId)
        {
            const string sql = @"
                INSERT INTO book_events (account_id, book_id, event_type_id)
                VALUES (@AccountId, @BookId, 1)"; // 1 = Request
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task<bool> AcceptRequest(int eventId, int librarianId)
        {
            const string checkSql = @"
        SELECT COUNT(1) 
        FROM book_events 
        WHERE id = @EventId AND event_type_id = 1";

            var exists = await DbConnect.QueryFirstOrDefaultAsync<int>(checkSql, new { EventId = eventId });
            if (exists == 0)
                return false;

            const string sql = @"
        UPDATE book_events
        SET event_type_id = 3,
            event_date = CURRENT_TIMESTAMP
        WHERE id = @EventId AND event_type_id = 1";

            var rowsAffected = await DbConnect.ExecuteAsync(sql, new { EventId = eventId });
            return rowsAffected > 0;
        }

        public async Task RejectRequest(int eventId, int librarianId)
        {
            const string sql = @"
        UPDATE book_events
        SET event_type_id = 5,
            event_date = CURRENT_TIMESTAMP
        WHERE id = @EventId AND event_type_id = 1";

            await DbConnect.ExecuteAsync(sql, new { EventId = eventId });
        }

        public async Task SaveBook(int accountId, int bookId)
        {
            const string sql = @"
                INSERT INTO book_events (account_id, book_id, event_type_id)
                VALUES (@AccountId, @BookId, 4)"; // 4 = Saved
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task TakeBook(int accountId, int bookId)
        {
            const string sql = @"
                INSERT INTO book_events (account_id, book_id, event_type_id)
                VALUES (@AccountId, @BookId, 3)"; // 3 = Taken
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task ReturnBook(int eventId)
        {
            const string sql = @"
                UPDATE book_events
                SET event_type_id = 2,
                    event_date = CURRENT_TIMESTAMP
                WHERE id = @EventId 
                AND event_type_id = 3";
            
            await DbConnect.ExecuteAsync(sql, new { EventId = eventId });
        }

        public async Task RemoveSavedBook(int accountId, int bookId)
        {
            const string sql = @"
                DELETE FROM book_events
                WHERE account_id = @AccountId AND book_id = @BookId AND event_type_id = 4";
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task CancelRequest(int accountId, int bookId)
        {
            const string sql = @"
                DELETE FROM book_events
                WHERE account_id = @AccountId AND book_id = @BookId AND event_type_id = 1";
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task<List<BookEventListDTO>> GetAllTakedBooks()
        {
            const string sql = @"
                SELECT 
                    be.id as bookevent_id,
                    be.book_id,
                    b.title as book_title,
                    be.account_id,
                    a.full_name as user_name,
                    'Taken' as event_type_name,
                    be.event_date,
                    au.full_name as author_name,
                    c.name as category_name,
                    br.name as branch_name
                FROM book_events be
                JOIN books b ON b.id = be.book_id
                JOIN accounts a ON a.id = be.account_id
                JOIN authors au ON au.id = b.author_id
                JOIN categories c ON c.id = b.category_id
                JOIN branches br ON br.id = b.branch_id
                WHERE be.event_type_id = 3
                ORDER BY be.event_date DESC";
            
            var books = await DbConnect.QueryAsync<BookEventListDTO>(sql);
            return books.ToList();
        }

        public async Task<BookRequestDetailDTO> GetRequestDetail(int eventId)
        {
            const string sql = @"
                SELECT 
                    be.id as event_id,
                    be.book_id,
                    b.title as book_title,
                    b.description as book_description,
                    c.name as category_name,
                    br.name as branch_name,
                    be.account_id as user_id,
                    a.full_name as user_fullname,
                    a.phone as user_phone,
                    be.event_date
                FROM book_events be
                JOIN books b ON b.id = be.book_id
                JOIN categories c ON c.id = b.category_id
                JOIN branches br ON br.id = b.branch_id
                JOIN accounts a ON a.id = be.account_id
                WHERE be.id = @EventId
                AND be.event_type_id = 1";

            var request = await DbConnect.QueryFirstOrDefaultAsync<BookRequestDetailDTO>(sql, new { EventId = eventId });
            if (request == null)
            {
                throw new Exception("Запрос не найден");
            }

            return request;
        }
    }
}
