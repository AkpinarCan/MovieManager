using movieManager.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace movieManager.Services
{
    internal class AuthenticationService
    {
        private readonly DatabaseContext _dbContext;
        private readonly SQLiteCommand _command;
        public AuthenticationService() 
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _dbContext = new DatabaseContext(connectionString);
            _command = new SQLiteCommand(_dbContext.GetConnection());
            _command.Parameters.Clear();

            EnsureUserTableExists();
        }

        public void AddUser(string username, string password, string tcNumber, string email, string dateOfBirth)
        {
            _command.Parameters.Clear();
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);

            _command.CommandText = @"
            INSERT INTO Users (Username, PasswordHash, Salt, TCNumber, Email, DateOfBirth)
            VALUES (@username, @passwordHash, @salt, @tcNumber, @email, @dateOfBirth);
            ";

            // Parametreleri ekleyin
            _command.Parameters.AddWithValue("@username", username);
            _command.Parameters.AddWithValue("@passwordHash", hashedPassword);
            _command.Parameters.AddWithValue("@salt", salt);
            _command.Parameters.AddWithValue("@tcNumber", tcNumber);
            _command.Parameters.AddWithValue("@email", email);
            _command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);

            try
            {
                _command.ExecuteNonQuery();
                MessageBox.Show("Kullanıcı başarıyla eklendi.");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}");
            }
        }


        public bool ValidateUser(string username, string password)
        {
            _command.CommandText = @"SELECT PasswordHash, Salt FROM Users WHERE Username = @username";
            _command.Parameters.AddWithValue("@username", username);

            using (SQLiteDataReader reader = _command.ExecuteReader())
            {
                if (reader.Read()) 
                {
                    string storedHash = reader.GetString(0);
                    string storedSalt = reader.GetString(1);

                    string hashToCompare = HashPassword(password, storedSalt);
                    return hashToCompare == storedHash;
                }
            }

            return false;
        }
        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedPasswordSalt = password + salt;
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPasswordSalt));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public void EnsureUserTableExists()
        {
            _command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                Salt TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                TCNumber TEXT NOT NULL,  -- TC Kimlik Numarasının son 3 hanesi
                Email TEXT NOT NULL UNIQUE,  -- E-mail adresi
                DateOfBirth TEXT  -- Doğum tarihi
            );";
            _command.ExecuteNonQuery();
        }

        private string GenerateSalt()
        {
            var randomBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        public bool UserUpdatePassword(string username, string newPassword)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(newPassword, salt);

            _command.Parameters.Clear();
            _command.CommandText = @"
                UPDATE Users 
                SET PasswordHash = @passwordHash, Salt = @salt
                WHERE Username = @username
            ";

            _command.Parameters.AddWithValue("@username", username);
            _command.Parameters.AddWithValue("@passwordHash", hashedPassword);
            _command.Parameters.AddWithValue("@salt", salt);

            try
            {
                int rowsAffected = _command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Şifre başarıyla güncellendi.");
                    return true;
                }
                else
                {
                    MessageBox.Show("Şifre güncellenemedi!");
                    return false;
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}");
                return false;
            }
        }


        public bool PasswordRenewal(string username, string tc, string email, string birthDate)
        {
            _command.Parameters.Clear();
            _command.CommandText = @"
                SELECT COUNT(*) 
                FROM Users 
                WHERE Username = @username 
                  AND TCNumber = @tc 
                  AND Email = @email 
                  AND DateOfBirth = @birthDate
            ";

            _command.Parameters.AddWithValue("@username", username);
            _command.Parameters.AddWithValue("@tc", tc);
            _command.Parameters.AddWithValue("@email", email);
            _command.Parameters.AddWithValue("@birthDate", birthDate);
            try
            {
                using (SQLiteDataReader reader = _command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0) > 0;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}");
            }

            return false;
        }
    }
}
