using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.Text = "Snake Game - Login";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Title Label
            lblTitle.Text = "SNAKE GAME";
            lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 174, 219);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point((this.Width - lblTitle.Width) / 2, 40);
            
            // Panel
            panelLogin.BackColor = Color.FromArgb(50, 50, 60);
            panelLogin.Location = new Point(50, 100);
            panelLogin.Size = new Size(300, 250);
            panelLogin.BorderStyle = BorderStyle.None;
            
            // Username
            lblUsername.Text = "Username:";
            lblUsername.ForeColor = Color.White;
            txtUsername.BackColor = Color.FromArgb(70, 70, 80);
            txtUsername.ForeColor = Color.White;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            
            // Password
            lblPassword.Text = "Password:";
            lblPassword.ForeColor = Color.White;
            txtPassword.BackColor = Color.FromArgb(70, 70, 80);
            txtPassword.ForeColor = Color.White;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.PasswordChar = '●';
            
            // Buttons
            btnLogin.Text = "LOGIN";
            btnLogin.BackColor = Color.FromArgb(0, 120, 215);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            
            btnSignup.Text = "CREATE ACCOUNT";
            btnSignup.BackColor = Color.FromArgb(60, 60, 70);
            btnSignup.ForeColor = Color.LightGray;
            btnSignup.FlatStyle = FlatStyle.Flat;
            btnSignup.FlatAppearance.BorderSize = 0;
            btnSignup.Font = new Font("Segoe UI", 9);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["SnakeDB"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string hashedPassword = HashPassword(txtPassword.Text);
                    
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@user AND Password=@pass", conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass", hashedPassword);
                    
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        new GameForm().Show();
                        this.Hide();
                    }
                    else MessageBox.Show("Invalid credentials", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
            this.Hide();
        }
    }
}
