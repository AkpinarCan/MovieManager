using movieManager.Data;
using movieManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieManager.Services
{
    public class MovieService
    {
        private readonly DatabaseContext _dbContext;
        private readonly SQLiteCommand _command;
        public MovieService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _dbContext = new DatabaseContext(connectionString);
            _command = new SQLiteCommand(_dbContext.GetConnection());
            _command.Parameters.Clear();
        }

        public string CreateTable()
        {
            _command.Parameters.Clear();

            // Tablo oluşturma komutu
            _command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Movies (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Type TEXT,
                    Director TEXT,
                    ReleaseYear INTEGER,
                    IMDb TEXT,
                    Duration INTEGER,
                    Producer TEXT
                )";

            // Tablo oluşturma işlemi
            _command.ExecuteNonQuery();

            // Tabloyu kontrol eden sorgu
            _command.CommandText = @"
                SELECT name 
                FROM sqlite_master 
                WHERE type='table' AND name='Movies'";

            using (var reader = _command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return "Tablo zaten mevcut.";
                }
            }

            return "Tablo başarıyla oluşturuldu.";
        }
        public async Task<Movie> DataChoiceMovieAsync(int Id)
        {
            _command.Parameters.Clear();
            _command.CommandText = @"SELECT * FROM Movies WHERE Id = @Id";
            _command.Parameters.AddWithValue("@Id", Id);

            using (var reader = await Task.Run(() => _command.ExecuteReaderAsync()))
            {
                if (await reader.ReadAsync())
                {
                    var movie = new Movie(
                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                        title: reader.GetString(reader.GetOrdinal("Title")),
                        type: reader.GetString(reader.GetOrdinal("Type")),
                        director: reader.GetString(reader.GetOrdinal("Director")),
                        releaseYear: reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                        imdb: reader.GetString(reader.GetOrdinal("IMDb")),
                        duration: reader.GetInt32(reader.GetOrdinal("Duration")),
                        producer: reader.GetString(reader.GetOrdinal("Producer"))
                    );
                    return movie;
                }
            }
            return null;
        }
        public async Task<string> MovieAddAsync(Movie movie)
        {
            // SQL sorgusu tanımlanıyor
            _command.CommandText = @"
                INSERT INTO Movies (
                    Title, Type, Director, ReleaseYear, IMDb, Duration, Producer
                ) VALUES (
                    @Title, @Type, @Director, @ReleaseYear, @IMDb, @Duration, @Producer
            )";

            // Parametreler ekleniyor
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Title", movie.Title);
            _command.Parameters.AddWithValue("@Type", movie.Type);
            _command.Parameters.AddWithValue("@Director", movie.Director);
            _command.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
            _command.Parameters.AddWithValue("@IMDb", movie.IMDb);
            _command.Parameters.AddWithValue("@Duration", movie.Duration);
            _command.Parameters.AddWithValue("@Producer", movie.Producer);

            try
            {
                await Task.Run(() => _command.ExecuteNonQuery());
                return "Film ekleme başarıyla gerçekleşti.";
            }
            catch (Exception ex)
            {
                return $"Film eklenirken bir hata oluştu: {ex.Message}";
            }
        }
        public async Task<string> MovieDeleteAsync(int id)
        {
            try
            {
                _command.Parameters.Clear();
                _command.CommandText = "DELETE FROM Movies WHERE Id = @Id";
                _command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = await Task.Run(() => _command.ExecuteNonQuery()) ;

                if (rowsAffected > 0)
                {
                    return "Silme işlemi başarıyla gerçekleşit";
                }
                else
                    return "Silinecek id değeri geçersiz!";
            }
            catch (Exception ex)
            {
                return $"Film silinirken hata oluştu: {ex.Message}";
            }
        }
        public async Task<string> MovieUpdateAsync(Movie movie,int id)
        {
            try
            {
                _command.Parameters.Clear();
                _command.CommandText = @"UPDATE Movies
                SET Title = @Title,
                    Type = @Type,
                    Director = @Director,
                    ReleaseYear = @ReleaseYear,
                    IMDb = @IMDb,
                    Duration = @Duration,
                    Producer = @Producer
                WHERE Id = @Id";

                _command.Parameters.AddWithValue("@Id", id);
                _command.Parameters.AddWithValue("@Title", movie.Title);
                _command.Parameters.AddWithValue("@Type", movie.Type);
                _command.Parameters.AddWithValue("@Director", movie.Director);
                _command.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
                _command.Parameters.AddWithValue("@IMDb", movie.IMDb);
                _command.Parameters.AddWithValue("@Duration", movie.Duration);
                _command.Parameters.AddWithValue("@Producer", movie.Producer);

                int rowsAffected = await Task.Run(() => _command.ExecuteNonQuery());

                if (rowsAffected > 0)
                {
                    return "Güncelleme işlemi başarıyla gerçekleşti";
                }
                else
                {
                    return "Güncellenecek id değeri geçersiz!";
                }
            }
            catch (Exception ex)
            {
                return $"Film güncellenirken hata oluştu: {ex.Message}";
            }
        }
        public string ConnectionClose()
        {
            try
            {
                if (_dbContext.GetConnection().State == System.Data.ConnectionState.Open)
                {
                    _dbContext.CloseConnection();
                    return "Bağlantı başarıyla kapatıldı.";
                }
                else
                {
                    return "Bağlantı zaten kapalı.";
                }
            }
            catch (Exception ex)
            {
                return $"Bağlantı kapatılırken hata oluştu: {ex.Message}";
            }
        }
    }
}
