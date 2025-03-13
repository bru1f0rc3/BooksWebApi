using Supabase;

namespace BooksApi.SupabaseBD
{
    public class Connection
    {
        private static Supabase.Client _supabaseClient;

        static Connection()
        {
            var url = "https://uvobqbanbbtrbnmsghxb.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InV2b2JxYmFuYmJ0cmJubXNnaHhiIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjkxNzM4MzUsImV4cCI6MjA0NDc0OTgzNX0.ng171iYkBN7SnV4pSyIquzJkwNvK_6NMqEFcREY0Ctc";
            var options = new SupabaseOptions { AutoConnectRealtime = true };
            _supabaseClient = new Supabase.Client(url, key, options);

            // Инициализация клиента
            _supabaseClient.InitializeAsync().Wait();
        }

        public static Supabase.Client GetSupabaseClient()
        {
            return _supabaseClient;
        }
    }
}