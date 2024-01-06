using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Shop
{
    public partial class LOGIN : Form
    {
        public LOGIN()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearbtn_Click(object sender, EventArgs e)
        {
            username.Text = "";
            password.Text = "";
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""F:\DATA AMALIA PUTRI\Kuliah\Semester 5\Pemograman Framework Enterprise\projectFramework\Pengelolaan Bahan Centra Moda Textile\MawuliShop-master\MawuliShop-master\shop.mdf"";Integrated Security=True;Connect Timeout=30");

        private async void loginbtn_Click(object sender, EventArgs e)
        {
            if (username.Text == "" || password.Text == "")
            {
                MessageBox.Show("\tMissing Credentials\t");
            }
            else
            {
                if (role.SelectedIndex > -1)
                {
                    if (role.SelectedItem.ToString() == "ADMIN")
                    {
                        if (username.Text == "admin" && password.Text == "admin")
                        {
                            Attendants att = new Attendants();
                            await Task.Delay(2000);
                            att.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("\tAdmin Credentials Wrong\t");
                        }
                    }
                    else
                    {
                        SqlDataAdapter sqa = new SqlDataAdapter("select count(*) from AttTable where AttName='" + username.Text + "' and Password='" + password.Text + "'", Con);
                        DataTable dt = new DataTable();
                        sqa.Fill(dt);

                        if (dt.Rows[0][0].ToString() == "1")
                        {
                            Globals.Set(username.Text);
                            SellingForm sf = new SellingForm();
                            await Task.Delay(2000);
                            sf.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Username/Password is Incorrect. Please Try Again");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("\tSelect A Role\t");
                }
            }
        }

        private void formside_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void role_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LOGIN_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
    public static class Globals
    {
        static String NameOfUser;

        internal static string Get()
        {
            return NameOfUser;
        }

        internal static void Set(string text)
        {
            NameOfUser = text;
        }
    }
}
