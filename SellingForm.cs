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
using System.Runtime.InteropServices;

namespace Shop
{
    public partial class SellingForm : Form
    {
        public SellingForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            sellername.Text = Globals.Get();
            date.Text = DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString();

        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""F:\DATA AMALIA PUTRI\Kuliah\Semester 5\Pemograman Framework Enterprise\projectFramework\Pengelolaan Bahan Centra Moda Textile\MawuliShop-master\MawuliShop-master\shop.mdf"";Integrated Security=True;Connect Timeout=30");

        private void prodList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            prodname.Text = prodList.SelectedRows[0].Cells[0].Value.ToString();
            Qcheck.Text = prodList.SelectedRows[0].Cells[1].Value.ToString();
            price.Text = prodList.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void fetchData()
        {
            Con.Open();
            string query = "select ProdName, Quantity, Price from ProdTable";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var data = new DataSet();
            sda.Fill(data);
            prodList.DataSource = data.Tables[0];
            Con.Close();
        }

        private void fetchDataSpecific()
        {
            Con.Open();
            string query = "select ProdName, Quantity, Price from ProdTable where category='" + categoryS.SelectedValue.ToString() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var data = new DataSet();
            sda.Fill(data);
            prodList.DataSource = data.Tables[0];
            Con.Close();
        }

        private void fetchDataSpecificText()
        {
            Con.Open();
            string query = "select ProdName, Quantity, Price from ProdTable where ProdName like '" + "%" + search.Text + "%" + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var data = new DataSet();
            sda.Fill(data);
            prodList.DataSource = data.Tables[0];
            Con.Close();
        }

        private void FetchCat()
        {
            Con.Open();
            String query = "select CatName from CatTable";
            SqlCommand command = new SqlCommand(query, Con);
            SqlDataReader read;
            read = command.ExecuteReader();
            DataTable data = new DataTable();
            data.Columns.Add("CatName", typeof(string));
            data.Load(read);
            categoryS.ValueMember = "catName";
            categoryS.DataSource = data;
            Console.WriteLine(data.GetType());
            Con.Close();
        }

        private void SellingForm_Load(object sender, EventArgs e)
        {
            fetchData();
            FetchCat();
        }

        int Gtotal = 0;
        int n = 0;
        private void prodaddbtn_Click(object sender, EventArgs e)
        {
            if (prodname.Text == "" || quantity.Text == "" || price.Text == "")
            {
                MessageBox.Show("Tidak bisa menambahkan !\t\nMissing Info");
            }
            else if (Convert.ToInt32(quantity.Text) > Convert.ToInt32(Qcheck.Text))
            {
                MessageBox.Show("Gagal !\t\nJumlah bahan tidak mencukupi");
            }
            else
            {
                int total = Convert.ToInt32(price.Text) * Convert.ToInt32(quantity.Text);
                n++;
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(activesale);
                row.Cells[0].Value = n;
                row.Cells[1].Value = prodname.Text;
                row.Cells[2].Value = price.Text;
                row.Cells[3].Value = quantity.Text;
                row.Cells[4].Value = Convert.ToInt32(price.Text) * Convert.ToInt32(quantity.Text);
                activesale.Rows.Add(row);
                Gtotal += total;
                totallb.Text = "Rp. " + Gtotal;
                prodname.Text = "";
                price.Text = "";
                quantity.Text = "";
            }
        }

        private void activesale_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (activesale.Rows.Count > 0)
            {
                prodidedit.Text = activesale.SelectedRows[0].Cells[0].Value.ToString();
                prodnameedit.Text = activesale.SelectedRows[0].Cells[1].Value.ToString();
                qtyedit.Text = activesale.SelectedRows[0].Cells[3].Value.ToString();
            }
        }

        private void prodeditbtn_Click(object sender, EventArgs e)
        {
            if (prodidedit.Text == "")
            {
                MessageBox.Show("Data belum dipilih \nPilih data untuk di edit");
            }
            else
            {
                activesale.SelectedRows[0].Cells[0].Value = prodidedit.Text;
                activesale.SelectedRows[0].Cells[3].Value = qtyedit.Text;
                Gtotal = Gtotal - Convert.ToInt32(activesale.SelectedRows[0].Cells[4].Value.ToString());
                activesale.SelectedRows[0].Cells[4].Value = Convert.ToInt32(qtyedit.Text) * Convert.ToInt32(activesale.SelectedRows[0].Cells[2].Value.ToString());
                Gtotal = Gtotal + Convert.ToInt32(activesale.SelectedRows[0].Cells[4].Value.ToString());
                totallb.Text = "Rp. " + Gtotal;
                prodidedit.Text = "";
                prodnameedit.Text = "";
                qtyedit.Text = "";
            }
        }

        private void proddelbtn_Click(object sender, EventArgs e)
        {
            if (prodidedit.Text == "")
            {
                MessageBox.Show("Data belum dipilih \nPilih data untuk di hapus");
            }
            else
            {
                Gtotal = Gtotal - Convert.ToInt32(activesale.SelectedRows[0].Cells[4].Value.ToString());
                totallb.Text = "Rp. " + Gtotal;
                prodidedit.Text = "";
                prodnameedit.Text = "";
                qtyedit.Text = "";
                activesale.Rows.Remove(activesale.SelectedRows[0]);
            }
        }

        private void calculate_Click(object sender, EventArgs e)
        {
            if (paid.Text == "")
            {
                MessageBox.Show("Masukkan jumlah pembayaran");
            }
            else
            {
                if (Gtotal > Convert.ToInt32(paid.Text))
                {
                    MessageBox.Show("Pembayran tidak mencukupi");
                }
                else
                {
                    change.Text = "Rp. " + Convert.ToString(Convert.ToInt32(paid.Text) - Convert.ToInt32(Gtotal));
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LOGIN lg = new LOGIN();
            lg.Show();
            this.Hide();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Centra Moda Textile", new Font("Ariel", 30, FontStyle.Bold), Brushes.Blue, new Point(250));

            string showdate = "Tanggal:\t" + date.Text + "\t" + DateTime.Now.ToString("h:mm:ss tt");
            string top = "********************************************************\n";
            string top1 = "\t\tResi Pembelian \t\n";
            string top2 = "********************************************************\n\n";
            string topunder = "--------------------------------------------------------\n\n";
            string top3 = "Bahan\tHarga(Rp)\tJumlah\tTotal(Rp)\n";
            string down = "***********************************************************\n";
            string downG = "Total Belanja:\t";
            string downP = "Pembayaran:\t\t";
            string downC = "Kembalian:\t";
            string down1 = "***********************************************************\n";
            string down2 = "\n\n\nTerimakasih Sudah Berbelanja\n\n\n";

            e.Graphics.DrawString(showdate, new Font("Ariel", 15, FontStyle.Bold), Brushes.Black, new Point(130, 100));

            e.Graphics.DrawString(top + top1 + top2, new Font("Ariel", 22, FontStyle.Bold), Brushes.Blue, new Point(75, 150));
            e.Graphics.DrawString(top3 + topunder, new Font("Ariel", 22, FontStyle.Bold), Brushes.Blue, new Point(130, 300));

            int n = 0, pt = 360;
            while (n < activesale.Rows.Count)
            {
                String output = activesale.Rows[n].Cells[1].Value.ToString() + "\t\t" + activesale.Rows[n].Cells[2].Value.ToString() + "\t     " + activesale.Rows[n].Cells[3].Value.ToString() + "\t      " + activesale.Rows[n].Cells[4].Value.ToString();
                e.Graphics.DrawString(output, new Font("Ariel", 20, FontStyle.Bold), Brushes.Black, new Point(130, pt));
                string NameU = activesale.Rows[n].Cells[1].Value.ToString();
                string PriceU = activesale.Rows[n].Cells[2].Value.ToString();
                string QtyU = activesale.Rows[n].Cells[3].Value.ToString();
                string TotalU = activesale.Rows[n].Cells[4].Value.ToString();
                AddSales("insert into AllSalesTable (Name, Price, Qty, Total, Date) values ('" + NameU + "','" + PriceU + "','" + QtyU + "','" + TotalU + "','" + DateTime.Now.ToString("dd/M/y h:mm:ss tt") + "')");
                UpdateProdQty("update ProdTable set Quantity=(Quantity -" + Convert.ToInt32(QtyU) + ") where ProdName='" + NameU + "'" + " and Price=" + PriceU + ";");

                n++;
                pt += 50;
            }
            String Paid = "Rp. " + paid.Text;
            e.Graphics.DrawString(down, new Font("Ariel", 20, FontStyle.Bold), Brushes.Blue, new Point(75, pt + 50));
            e.Graphics.DrawString(downG + totallb.Text + "\n", new Font("Ariel", 20, FontStyle.Bold), Brushes.Blue, new Point(130, pt + 100));
            e.Graphics.DrawString(downP + Paid + "\n", new Font("Ariel", 20, FontStyle.Bold), Brushes.Blue, new Point(130, pt + 150));
            e.Graphics.DrawString(downC + change.Text + "\n", new Font("Ariel", 20, FontStyle.Bold), Brushes.Blue, new Point(130, pt + 200));
            e.Graphics.DrawString(down1, new Font("Ariel", 20, FontStyle.Bold), Brushes.Blue, new Point(75, pt + 250));
            e.Graphics.DrawString(down2, new Font("Ariel", 20, FontStyle.Bold), Brushes.Blue, new Point(250, pt + 300));
        }

        private void AddSales(String sale)
        {
            Con.Open();
            String query = sale;
            SqlCommand command = new SqlCommand(query, Con);
            command.ExecuteNonQuery();
            Con.Close();
        }

        private void UpdateProdQty(String q)
        {
            Con.Open();
            String query = q;
            SqlCommand command = new SqlCommand(query, Con);
            command.ExecuteNonQuery();
            Con.Close();
        }

        private void AddHist()
        {
            Con.Open();
            String query = "insert into HistoryTable (AttName, Date, Amount) values ('" + sellername.Text + "','" + DateTime.Now.ToString("dd/M/y h:mm:ss tt") + "','" + Gtotal.ToString() + "')";
            SqlCommand command = new SqlCommand(query, Con);
            command.ExecuteNonQuery();
            Con.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (activesale.Rows.Count < 0 || totallb.Text == "" || change.Text == "")
            {
                MessageBox.Show("Informasi belum lengkap.\nCek bahan dan pembayaran");
            }
            else
            {
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
                AddHist();
                activesale.Rows.Clear();
                totallb.Text = "";
                change.Text = "";
                paid.Text = "";
                Gtotal = 0;
                n = 0;
                fetchData();
            }

        }

        private void categoryS_SelectionChangeCommitted(object sender, EventArgs e)
        {
            fetchDataSpecific();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            fetchDataSpecificText();
        }

        private void done_Click(object sender, EventArgs e)
        {
            if (activesale.Rows.Count < 0 || totallb.Text == "" || change.Text == "")
            {
                MessageBox.Show("Informasi belum lengkap.\nCek bahan dan pembayaran");
            }
            else
            {
                int m = 0;
                while (m < activesale.Rows.Count)
                {
                    string NameU = activesale.Rows[m].Cells[1].Value.ToString();
                    string PriceU = activesale.Rows[m].Cells[2].Value.ToString();
                    string QtyU = activesale.Rows[m].Cells[3].Value.ToString();
                    string TotalU = activesale.Rows[m].Cells[4].Value.ToString();
                    AddSales("insert into AllSalesTable (Name, Price, Qty, Total, Date) values ('" + NameU + "','" + PriceU + "','" + QtyU + "','" + TotalU + "','" + DateTime.Now.ToString("dd/M/y h:mm:ss tt") + "')");
                    UpdateProdQty("update ProdTable set Quantity=(Quantity -" + Convert.ToInt32(QtyU) + ") where ProdName='" + NameU + "'" + " and Price=" + PriceU + ";");

                    m++;
                }
                AddHist();
                activesale.Rows.Clear();
                totallb.Text = "";
                change.Text = "";
                paid.Text = "";
                Gtotal = 0;
                n = 0;
                fetchData();
            }
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            search.Clear();
            fetchData();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SellingForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void sellername_Click(object sender, EventArgs e)
        {



        }
    }
}