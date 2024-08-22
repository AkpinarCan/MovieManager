using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieManager.Data
{
    public class DatabaseContext : IDisposable
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SQLiteConnection(_connectionString);
        }

        // Bağlantıyı döndürür
        public SQLiteConnection GetConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
                //Console.WriteLine("- Bağlantı başarılı -");
            }
            return _connection;
        }

        // Bağlantıyı kapatır
        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                //Console.WriteLine("- Bağlantı Kapatılılıyor -");
            }
        }

        // IDisposable implementasyonu ile kaynakları serbest bırakır
        public void Dispose()
        {
            CloseConnection();
            _connection.Dispose();
        }
    }
}
