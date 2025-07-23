using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;



namespace Attendence_Management_System.PAL.Forms
{
    public partial class FormLogin : Form
    {
        private readonly string connectionString = "Server=127.0.0.1;Port=3306;Database=Attendance_Management_System;Uid=root;Pwd=MUH2101021M;";
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            pictureBoxHide.Hide();
            pictureBoxError.Hide();
            labelError.Hide();
            textBoxPassword.UseSystemPasswordChar = true;
            textBoxName.Focus();
            TestDatabaseConnection();
            
        }
        private void TestDatabaseConnection()
        {
            try
            {
                using (MySqlConnection conn=new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Database connnection succesful!");
                    conn.Close();
                }
            }
            catch(MySqlException ex)
            {
                MessageBox.Show($"Database connection failed: {ex.Message}");
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxError_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxShow_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(pictureBoxShow, "Show Password");
        }

        private void pictureBoxHide_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(pictureBoxHide, "Hide Password");
        }

        private void pictureBoxShow_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = false;
            pictureBoxShow.Hide();
            pictureBoxHide.Show();
        }

        private void pictureBoxHide_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            pictureBoxShow.Show();
            pictureBoxHide.Hide();
        }

        private void pictureBoxClose_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(pictureBoxClose, "Close");

        }

        private void pictureBoxMinimize_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(pictureBoxMinimize, "Minimize");
        }

        private void pictureBoxMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                labelError.Text = "Please enter both username and password.";
                pictureBoxError.Show();
                labelError.Show();
                return;
            }

            string userRole = Attendance.IsValidNamePass(textBoxName.Text.Trim(), textBoxPassword.Text.Trim(), connectionString);
            if (!string.IsNullOrEmpty(userRole))
            {
                // Successful login
                textBoxName.Clear();
                textBoxPassword.Clear();
                pictureBoxHide_Click(sender, e); // Reset password visibility
                textBoxName.Focus();
                pictureBoxError.Hide();
                labelError.Hide();

                // Open main form
                FormMain formMain = new FormMain();
                this.Hide();
                formMain.ShowDialog();
                this.Close();
            }
            else
            {
                // Invalid credentials
                labelError.Text = "Invalid username or password.";
                pictureBoxError.Show();
                labelError.Show();
            }
        }

        private void textBoxName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
            }
        }

        private void textBoxPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                buttonLogin.PerformClick();
                e.Handled = true;
            }
        }

        private void labelFP_Click(object sender, EventArgs e)
        {
            FormForgotPassword formForgotPassword = new FormForgotPassword();
            formForgotPassword.ShowDialog();
        }

    }
    public class Attendance
    {
        public static string IsValidNamePass(string username, string password, string connectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Query to validate username and password
                    string query = "SELECT User_Role FROM User_Table WHERE User_Name = @username AND User_Pass = @password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        object result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : "";
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Log database errors (for debugging)
                Console.WriteLine("Database error: " + ex.Message);
                return "";
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.WriteLine("Error: " + ex.Message);
                return "";
            }
        }
    }
}
