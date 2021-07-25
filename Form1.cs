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

namespace projekt
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;

        MySqlConnection sqlConn = new MySqlConnection();
        MySqlCommand sqlCmd = new MySqlCommand();
        DataTable sqlDt = new DataTable();
        string SqlQuery;
        MySqlDataAdapter DtA = new MySqlDataAdapter();
        MySqlDataReader sqlRd;

        DataSet DS = new DataSet();

        String server = "localhost";
        String username = "root";
        String password = "heslo";
        String database = "projekt";

        

        int selectedRow;

        public Form1()
        {
            InitializeComponent();
        }

        private void upLoadData()
        {
           sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
           




            sqlConn.Open();
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = "SELECT * FROM projekt.vypujcky";

            sqlRd = sqlCmd.ExecuteReader();
            sqlDt.Load(sqlRd);
            sqlRd.Close();
            sqlConn.Close();
            dataGridView1.DataSource = sqlDt;
            
        }

        //vymazat txt pole
        private void btnRestart_Click(object sender, EventArgs e)
        {
            try
            {
                foreach(Control c in panel1.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();
                }
                txtVyhledat.Text = " ";
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           try
           {
               e.Graphics.DrawImage(bitmap, 0, 0);
           
           }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            upLoadData();
        }

        //přidat
        private void button1_Click(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;

            try
            {
                sqlConn.Open();
                SqlQuery = "insert into projekt.vypujcky (ID_vypujcky, ID_auta, ID_zakaznik, datum_vypujceni, datum_vraceni)" + 
                    "values('"+ txt1.Text + "','" + txt2.Text + "','"+ txt3.Text + "','"+ txt4.Text + "','"+ txt5.Text +"')";

                sqlCmd = new MySqlCommand(SqlQuery, sqlConn);

                sqlRd = sqlCmd.ExecuteReader();
                sqlConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
            upLoadData();
        }

        //změnit
        private void button3_Click(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            sqlConn.Open();

            

            try
            {
                
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.Connection = sqlConn;

                sqlCmd.CommandText = "Update autobazar.adresa SET  " +
                                    "ID_vypujcka = @ID_vypujcka " +
                                    "ID_auto = @ID_auto" +
                                    "ID_zakaznik = @ID_zakaznik" +
                                    "datum_vypujceni = @datum_vypujceni" +
                                    "datum_vraceni = @datum_vraceni ";


                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@ID_vypujcka", txt1.Text);
                sqlCmd.Parameters.AddWithValue("@ID_auto", txt2.Text);
                sqlCmd.Parameters.AddWithValue("@ID_zakaznik", txt3.Text);
                sqlCmd.Parameters.AddWithValue("@datum_vypujceni", txt4.Text);
                sqlCmd.Parameters.AddWithValue("@datum_vraceni", txt5.Text);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
                upLoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
            upLoadData();

        }

       
        //údaje do řádků
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedRow = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                txt1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txt2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txt3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txt4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txt5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

       //smazání
        private void button2_Click(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            sqlConn.Open();

            
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.Connection = sqlConn;

            int i;
            i = dataGridView1.SelectedCells[0].RowIndex;
            
            //sqlCmd.CommandText = "DELETE from projekt.vypujcky where ID_vypujcky = @ID_vypujcky ";
            sqlCmd.CommandText = "DELETE FROM projekt.vypujcky WHERE ID=" + dataGridView1.SelectedRows[i].Cells[0].Value.ToString() + "";
                sqlCmd = new MySqlCommand(SqlQuery, sqlConn);

            

            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.RemoveAt(item.Index);
                }


                foreach (Control c in panel1.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();
                }
               
                txtVyhledat.Text = " ";
            sqlRd = sqlCmd.ExecuteReader();

            sqlConn.Close();
           
            upLoadData();
            
          
        }

       //vyhledávací lišta
        private void txtVyhledat_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DataView dv = sqlDt.DefaultView;
                dv.RowFilter = string.Format("ID_vypujcky like '%{0}%'", txtVyhledat.Text);
                dataGridView1.DataSource = dv.ToTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
    }


}
