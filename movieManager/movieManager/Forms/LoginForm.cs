using System;
using movieManager.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace movieManager.Forms
{
    public partial class LoginForm : Form
    {
        private AuthenticationService _authenticationService;
        public LoginForm()
        {
            InitializeComponent();
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
            _authenticationService = new AuthenticationService();
            tabPageTexts();
        }

        private void tabPageTexts()
        {
            tabPage1.Text = "Login";
            tabPage2.Text = "Sign up";
            tabPage3.Text = "Forgot Password";
            tabPage4.Text = "Visiter";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userN = username.Text.Trim();
            string passW = password.Text.Trim();
            DateTime da = new DateTime(1990, 1, 1);

            if(userN.Length <= 1 || passW.Length <= 1)
            {
                error1.Visible = true;
                error2.Visible = true;
                MessageBox.Show("Kullanıcı bilgilerini doldurun", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (_authenticationService.ValidateUser(userN,passW))
            {
                error1.Visible = false;
                error2.Visible = false;
                // Giriş başarılı, ana formu aç
                MessageBox.Show("BAŞARILI");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Giriş başarısız, kullanıcıyı bilgilendir
                error1.Visible = true;
                error2.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string userD = textBox7.Text;
            string mailD = textBox8.Text;
            string tcD = textBox6.Text;
            string formattedDate = textBox9.Text;

            if (_authenticationService.PasswordRenewal(userD, tcD, mailD, formattedDate))
            {
                textBox9.Enabled = false;
                textBox8.Enabled = false;
                textBox7.Enabled = false;
                textBox6.Enabled = false;
                panel13.Visible = true;
                button3.Visible = false;
                button4.Visible = true;
                label27.Text = $"İşlem yapılan kullanıcı : {userD}";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string passw1 = textBox10.Text.Trim();
            string passw2 = textBox11.Text.Trim();

            if (passw1 == passw2)
            {
                if (_authenticationService.UserUpdatePassword(textBox7.Text, passw1))
                {
                    MessageBox.Show("İşlem başarıyla gerçekleşmiştir");
                    textBox10.Text = "";
                    textBox11.Text = "";
                    textBox9.Text = "";
                    textBox8.Text = "";
                    textBox7.Text = "";
                    textBox6.Text = "";
                    textBox9.Enabled = true;
                    textBox8.Enabled = true;
                    textBox7.Enabled = true;
                    textBox6.Enabled = true;
                    panel13.Visible = false;
                    button3.Visible = true;
                    button4.Visible = false;
                    tabControl1.SelectTab(0);
                }
                else
                    MessageBox.Show("İşlem başarısız!!!");
            }
            else
            {
                MessageBox.Show("Yeni girilen şifre ile şifre tekrarı uyuşmalıdır");
                textBox10.Text = "";
                textBox11.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void TakeString()
        {
            string username = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            string tc = textBox5.Text.Trim();
        }
    }
}
