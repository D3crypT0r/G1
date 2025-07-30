using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.Text = "Snake Game - Register";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Title Label
            lblTitle.Text = "CREATE ACCOUNT";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 174, 219);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point((this.Width - lblTitle.Width) / 2, 30);
            
            // Panel
            panelRegister.BackColor = Color.FromArgb(50, 50, 60);
            panelRegister.Location = new Point(40, 80);
            panelRegister.Size = new Size(320, 300);
            panelRegister.BorderStyle = BorderStyle.None;
            
            // TextBoxes
            txtUsername.BackColor = Color.FromArgb(70, 70, 80);
            txtUsername.ForeColor = Color.White;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            
            txtPassword.BackColor = Color.FromArgb(70, 70, 80);
            txtPassword.ForeColor = Color.White;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.PasswordChar = '●';
            
            txtConfirm.BackColor = Color.FromArgb(70, 70, 80);
            txtConfirm.ForeColor = Color.White;
            txtConfirm.BorderStyle = BorderStyle.FixedSingle;
            txtConfirm.PasswordChar = '●';
            
            // Buttons
            btnRegister.Text = "REGISTER";
            btnRegister.BackColor = Color.FromArgb(0, 120, 215);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            
            btnBack.Text = "BACK TO LOGIN";
            btnBack.BackColor = Color.FromArgb(60, 60, 70);
            btnBack.ForeColor = Color.LightGray;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Font = new Font("Segoe UI", 9);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords don't match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text.Length < 4)
            {
                MessageBox.Show("Password must be at least 4 characters!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["SnakeDB"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string hashedPassword = HashPassword(txtPassword.Text);
                    
                    SqlCommand cmd = new SqlCommand("INSERT INTO Users (Username, Password) VALUES (@user, @pass)", conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass", hashedPassword);
                    
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Registration successful!", "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new LoginForm().Show();
                        this.Hide();
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                        MessageBox.Show("Username already exists!", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Hide();
        }
    }
}
