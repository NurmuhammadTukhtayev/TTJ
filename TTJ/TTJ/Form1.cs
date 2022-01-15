using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TTJ
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // connect to db with connectionstring
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TTJ"].ConnectionString);

            // open cinnection
            sqlConnection.Open();

            //fill combobox
            SqlCommand cmd = new SqlCommand(@"SELECT FName, DName FROM Faculty F 
                                    INNER JOIN Direction D ON F.Fid=D.FID;", sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            cmd.ExecuteNonQuery();

            comboBox1.DataSource = ds.Tables[0];
            comboBox2.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "FName";
            comboBox2.DisplayMember = "DName";
           



            // check connection
            if(sqlConnection.State != ConnectionState.Open)
            {
                MessageBox.Show("No Connection!");
            }
            else
            {
                MessageBox.Show("Connected to DB");
            }

        }

        private void Student_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT Name AS FIO, Gender AS Jinsi, [Address] AS Manzil, FName AS [Fakultet nomi], 
DName AS [yo`nalishi] FROM Students S JOIN Direction D ON S.DID = D.DID JOIN Faculty F ON F.Fid = D.FID",
sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];

        }

        private void rooms_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT RNum AS [Xona raqami], 
Price AS Narxi, [Brom qilingan] = CASE WHEN IsBromed=1 THEN 'HA' ELSE 'Yoq' END, 
BID AS[Bino raqami], [Address] AS[Manzil] 
FROM Rooms R INNER JOIN Building B ON R.BNum = B.BID", sqlConnection);


            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource=ds.Tables[0];
        }

        private void payment_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT [Name] AS FIO, FName AS Fakultet, DName AS [Yo`nalish], P.RNum AS Xona, 
StartDate AS [Brom qilingan kun], EndDate AS [Oxirgi muddat], [Oy miqdori]=DATEDIFF(MONTH, StartDate, EndDate),
Payed AS [To`landi], Price AS Narxi, [To`langan summa]=(Price*Months), Qarzdorlik=(Price*Months)-Payed
FROM Payment P INNER JOIN Students S ON P.[SID] = S.Id
INNER JOIN Rooms R ON P.RNum = R.RNum
INNER JOIN Building B ON B.BID = R.BNum
INNER JOIN Direction D ON D.DID = S.DID
INNER JOIN Faculty F ON F.Fid = D.FID", sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void emptyRooms_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT RNum AS [Xona raqami], 
Price AS Narxi, BID AS[Bino raqami], [Address] AS[Manzil] 
FROM Rooms R INNER JOIN Building B ON R.BNum = B.BID WHERE IsBromed = 0", sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        // qarzdor talabalar
        private void label1_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT [Name] AS FIO, FName AS Fakultet, DName AS [Yo`nalish], P.RNum AS Xona, 
StartDate AS [Brom qilingan kun], EndDate AS [Oxirgi muddat], [Oy miqdori]=DATEDIFF(MONTH, StartDate, EndDate),
Payed AS [To`landi], Price AS Narxi, [To`langan summa]=(Price*Months), Qarzdorlik=(Price*Months)-Payed
FROM Payment P INNER JOIN Students S ON P.[SID] = S.Id
INNER JOIN Rooms R ON P.RNum = R.RNum
INNER JOIN Building B ON B.BID = R.BNum
INNER JOIN Direction D ON D.DID = S.DID
INNER JOIN Faculty F ON F.Fid = D.FID 
WHERE (Price*Months)-Payed>0", sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        private void searchBut_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($@"SELECT Name AS FIO, Gender AS Jinsi, [Address] AS Manzil, FName AS [Fakultet nomi], 
DName AS [yo`nalishi] FROM Students S JOIN Direction D ON S.DID = D.DID JOIN Faculty F ON F.Fid = D.FID
WHERE ([Name] LIKE '%{textBox1.Text}%' OR '{textBox1.Text}'='')
AND (DName='{comboBox2.Text}' OR FName='{comboBox1.Text}')", sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            //MessageBox.Show($"hello, {textBox1.Text}");
        }
    }
}
