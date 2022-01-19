using System;
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM V_FacDir;", sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            cmd.ExecuteNonQuery();

            comboBox1.DataSource = ds.Tables[0];
            comboBox2.DataSource = ds.Tables[0];
            comboBox5.DataSource = ds.Tables[0];
            comboBox6.DataSource = ds.Tables[0];
            comboBox10.DataSource = ds.Tables[0];
            comboBox9.DataSource = ds.Tables[0];
            
            comboBox1.DisplayMember = "FName";
            comboBox2.DisplayMember = "DName";
            comboBox5.DisplayMember = "FName";
            comboBox6.DisplayMember = "DName";
            comboBox10.DisplayMember = "FName";
            comboBox9.DisplayMember = "DName";

            SqlCommand command = new SqlCommand("SELECT BID, Price FROM Building;", sqlConnection);
            SqlDataAdapter adapter1 = new SqlDataAdapter(command);
            DataSet ds1 = new DataSet();
            adapter1.Fill(ds1);
            command.ExecuteNonQuery();

            textBox7.Text = ds1.Tables[0].Select()[0][1].ToString();

            comboBox3.DataSource = ds1.Tables[0];
            comboBox3.DisplayMember = "BID";


            //update empty rooms
            SqlCommand updateCmd = new SqlCommand("EXECUTE Update_Rooms;", sqlConnection);
            int i = updateCmd.ExecuteNonQuery();
            //MessageBox.Show(i.ToString());
            

            // check connection
            if (sqlConnection.State != ConnectionState.Open)
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
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM V_ShowStudent_Info;", sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        private void rooms_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM V_Building_Rooms;", sqlConnection);


            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource=ds.Tables[0];
        }

        private void payment_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM V_Payment_Info;", sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        

        private void emptyRooms_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM V_Empty_Rooms;", sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        // qarzdor talabalar
        private void label1_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM V_Dept_Info;", sqlConnection);

            DataSet ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }

        private void searchBut_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SP_Search", sqlConnection);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", textBox1.Text);
            cmd.Parameters.AddWithValue("@DName", comboBox2.Text);
            cmd.Parameters.AddWithValue("@FName", comboBox1.Text);
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();

            try
            {
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                dt.Load(sqlDataReader);
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            dataGridView1.DataSource = dt;
        }

        private void archive_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM V_Archive", sqlConnection);

            DataSet ds =new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource=ds.Tables[0];
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {            
            if(textBox2.Text!="")
                 textBox4.Text = (float.Parse(textBox7.Text) * int.Parse(textBox2.Text)).ToString();
        }
        
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox5.Text = textBox3.Text;

            if(textBox5.Text!="" && textBox4.Text!="")
                textBox6.Text = (double.Parse(textBox4.Text)-double.Parse(textBox5.Text)).ToString();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            
            try
            {
                SqlCommand cmd = new SqlCommand(@"SP_InsertData", sqlConnection);
                
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", StudentName.Text);
                cmd.Parameters.AddWithValue("@gender", comboBox4.Text);
                cmd.Parameters.AddWithValue("@address", textBox4.Text);
                cmd.Parameters.AddWithValue("@DName", comboBox6.Text);
                cmd.Parameters.AddWithValue("@RNum", comboBox7.Text);
                cmd.Parameters.AddWithValue("@startDate", StartTime.Text);
                cmd.Parameters.AddWithValue("@endDate", EndTime.Text);
                cmd.Parameters.AddWithValue("@months", textBox2.Text);
                cmd.Parameters.AddWithValue("@payed", textBox3.Text);

                int i = cmd.ExecuteNonQuery();
                MessageBox.Show("Ma'lumot muvofaqqiyatli saqlandi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiritayotgan ma'lumotlar to'g'riligiga ishonch hosil qiling!");
            }
        }

        // When building has chosed
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd1 = new SqlCommand($"SELECT * FROM Rooms WHERE BNum = '{comboBox3.Text}' AND isBromed=0;", sqlConnection);
            SqlDataAdapter adapter2 = new SqlDataAdapter(cmd1);
            DataSet ds2 = new DataSet();
            adapter2.Fill(ds2);
            cmd1.ExecuteNonQuery();

            comboBox7.DataSource = ds2.Tables[0];
            comboBox7.DisplayMember = "RNum";
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var adapter = new SqlDataAdapter($"EXEC V_Select_Students '{comboBox9.Text}'", sqlConnection);
                var ds = new DataSet();
                adapter.Fill(ds);

                comboBox8.DataSource = ds.Tables[0];
                comboBox8.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            var adapter = new SqlDataAdapter($"EXEC V_Select_Students '{comboBox9.Text}'", sqlConnection);
            var ds = new DataSet();
            adapter.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var name = row["Name"];
                var bNum = row["BNum"];
                var rNum = row["RNum"];
                var payed = row["Payed"];
                var dept = row["Dept"];               

                if ((string)name == comboBox8.GetItemText(comboBox8.SelectedItem))
                {
                    textBox8.Text = bNum.ToString();
                    textBox9.Text = rNum.ToString();
                    textBox11.Text = payed.ToString();
                    textBox10.Text = dept.ToString();
                    break;
                }
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            var adapter = new SqlDataAdapter($"EXECUTE SP_Get_Student '{comboBox8.GetItemText(comboBox8.SelectedItem)}'", sqlConnection);
            var ds = new DataSet();
            adapter.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                double payed = double.Parse(row["Payed"].ToString());
                double months = double.Parse(row["Months"].ToString());
                double price = double.Parse(row["Price"].ToString());
                double debt = double.Parse(row["Dept"].ToString());

                textBox13.Text = (months*price-(double.Parse(textBox12.Text) + payed)).ToString();

            }
        }

        private void PaySaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var cmd = new SqlCommand("SP_Pay", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@N", comboBox8.Text);
                cmd.Parameters.AddWithValue("@P", textBox12.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("To'lov muvofaqqiyatli amalga oshirildi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
