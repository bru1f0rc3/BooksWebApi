using WebApplication2.Connection;
using WebApplication2.DTO.BookEvent;

namespace WebApplication2.Services.BookEvent
{
    public class BookEventService
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
                FROM ""BookEvents"" be
                JOIN ""Books"" b ON b.id = be.book_id
                JOIN ""Accounts"" a ON a.id = be.account_id
                JOIN ""Event_Type"" et ON et.id = be.event_type_id
                ORDER BY be.event_date DESC";
            
            var events = await DbConnect.QueryAsync<BookEventListDTO>(sql);
            return events.ToList();
        }

        public async Task<List<BookEventListDTO>> GetPendingRequests()
        {
            const string sql = @"
                SELECT 
                    be.id as bookevent_id,
                    b.title as book_title,
                    a.full_name as user_name,
                    a.id as account_id,
                    et.name as event_type_name
                FROM ""BookEvents"" be
                JOIN ""Books"" b ON b.id = be.book_id
                JOIN ""Accounts"" a ON a.id = be.account_id
                JOIN ""Event_Type"" et ON et.id = be.event_type_id
                WHERE be.event_type_id = 1
                ORDER BY be.event_date DESC";
            
            var events = await DbConnect.QueryAsync<BookEventListDTO>(sql);
            return events.ToList();
        }

        public async Task<List<UserBookHistoryDTO>> GetUserBookHistory(int accountId)
        {
            const string sql = @"
                WITH LastEvents AS (
                    SELECT 
                        book_id,
                        MAX(event_date) as last_event_date,
                        MAX(event_type_id) as last_event_type_id
                    FROM ""BookEvents""
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
                JOIN ""Books"" b ON b.id = le.book_id
                JOIN ""Authors"" a ON a.id = b.author_id
                JOIN ""Categories"" c ON c.id = b.category_id
                JOIN ""Branches"" br ON br.id = b.branch_id
                JOIN ""Event_Type"" et ON et.id = le.last_event_type_id
                ORDER BY le.last_event_date DESC";
            
            var history = await DbConnect.QueryAsync<UserBookHistoryDTO>(sql, new { AccountId = accountId });
            return history.ToList();
        }

        public async Task<List<UserBookHistoryDTO>> GetUserCurrentBooks(int accountId)
        {
            const string sql = @"
                WITH LastEvents AS (
                    SELECT 
                        book_id,
                        MAX(event_date) as last_event_date,
                        MAX(event_type_id) as last_event_type_id
                    FROM ""BookEvents""
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
                JOIN ""Books"" b ON b.id = le.book_id
                JOIN ""Authors"" a ON a.id = b.author_id
                JOIN ""Categories"" c ON c.id = b.category_id
                JOIN ""Branches"" br ON br.id = b.branch_id
                JOIN ""Event_Type"" et ON et.id = le.last_event_type_id
                WHERE le.last_event_type_id IN (3, 4) -- Taked или Saved
                ORDER BY le.last_event_date DESC";
            
            var currentBooks = await DbConnect.QueryAsync<UserBookHistoryDTO>(sql, new { AccountId = accountId });
            return currentBooks.ToList();
        }

        public async Task RequestBook(int accountId, int bookId)
        {
            const string sql = @"
                INSERT INTO ""BookEvents"" (account_id, book_id, event_type_id)
                VALUES (@AccountId, @BookId, 1)"; // 1 = Requested
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task<bool> AcceptRequest(int eventId, int librarianId)
        {
            // Проверяем существование запроса
            const string checkSql = @"
                SELECT COUNT(1) 
                FROM ""BookEvents"" 
                WHERE id = @EventId AND event_type_id = 1";

            var exists = await DbConnect.QueryFirstOrDefaultAsync<int>(checkSql, new { EventId = eventId });
            if (exists == 0)
                return false;

            // Обновляем существующий запрос
            const string sql = @"
                UPDATE ""BookEvents""
                SET event_type_id = 3,
                    account_id = @LibrarianId,
                    event_date = CURRENT_TIMESTAMP
                WHERE id = @EventId AND event_type_id = 1";
            
            var rowsAffected = await DbConnect.ExecuteAsync(sql, new { EventId = eventId, LibrarianId = librarianId });
            return rowsAffected > 0;
        }

        public async Task RejectRequest(int eventId, int librarianId)
        {
            const string sql = @"
                UPDATE ""BookEvents""
                SET event_type_id = 5 -- Rejected
                WHERE id = @EventId AND event_type_id = 1";
            
            await DbConnect.ExecuteAsync(sql, new { EventId = eventId });
        }

        public async Task SaveBook(int accountId, int bookId)
        {
            const string sql = @"
                INSERT INTO ""BookEvents"" (account_id, book_id, event_type_id)
                VALUES (@AccountId, @BookId, 4)"; // 4 = Saved
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task TakeBook(int accountId, int bookId)
        {
            const string sql = @"
                INSERT INTO ""BookEvents"" (account_id, book_id, event_type_id)
                VALUES (@AccountId, @BookId, 3)"; // 3 = Taked
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task ReturnBook(int accountId, int bookId)
        {
            const string sql = @"
                INSERT INTO ""BookEvents"" (account_id, book_id, event_type_id)
                VALUES (@AccountId, @BookId, 2)"; // 2 = Returned
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task RemoveSavedBook(int accountId, int bookId)
        {
            const string sql = @"
                DELETE FROM ""BookEvents""
                WHERE account_id = @AccountId AND book_id = @BookId AND event_type_id = 4"; // 4 = Saved
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
        }

        public async Task CancelRequest(int accountId, int bookId)
        {
            const string sql = @"
                DELETE FROM ""BookEvents""
                WHERE account_id = @AccountId AND book_id = @BookId AND event_type_id = 1"; // 1 = Requested
            
            await DbConnect.ExecuteAsync(sql, new { AccountId = accountId, BookId = bookId });
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
                FROM ""BookEvents"" be
                JOIN ""Books"" b ON b.id = be.book_id
                JOIN ""Accounts"" a ON a.id = be.account_id
                JOIN ""Event_Type"" et ON et.id = be.event_type_id
                WHERE be.event_type_id = 1
                ORDER BY be.event_date DESC";
            
            var requests = await DbConnect.QueryAsync<BookEventListDTO>(sql);
            return requests.ToList();
        }
    }
}
