using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace movieManager.Services
{
    public class UserValidator
    {
        
        public bool ValidateUserInputs(string username, string password, string tc, string mail , string birthDate, out string errorMessage)
        {
            errorMessage = string.Empty;
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                errorMessage = "Kullanıcı adı en az 3 karakter olmalıdır.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                errorMessage = "Şifre en az 6 karakter olmalıdır.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(tc) || tc.Length != 3)
            {
                errorMessage = "T.C. kimlik numarasının son 3 hanesi olmalıdır.";
                return false;
            }
            if (!int.TryParse(tc, out _))
            {
                errorMessage = "T.C. kimlik numarasının son 3 hanesi sayı olmalıdır.";
                return false;
            }
            MessageBox.Show($"{tc.GetType()}");
            //string convertTC = tc.ToString();
            if (string.IsNullOrEmpty(mail) || !mail.Contains("@") || !mail.Contains(".com"))
            {
                errorMessage = "Lütfen geçerli bir mail adresi giriniz. Örnek: example@mail.com";
                return false;

            }
            if (!Regex.IsMatch(mail, emailPattern))
            {
                errorMessage = "Lütfen geçerli bir mail adresi giriniz. Örnek: example@mail.com";
                return false;
            }
            if (!DateTime.TryParse(birthDate, out _))
            {
                errorMessage = "Geçerli bir doğum tarihi giriniz. Örnek tarih : 2004-01-01";
                return false;
            }
            return true;
        }
    }
}
