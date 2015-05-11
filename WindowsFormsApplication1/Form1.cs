using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Windows.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool typeS, textF, monthS;
        string database;

        DateTime date;

        DispatcherTimer timer = new DispatcherTimer();

        System.Data.OleDb.OleDbConnectionStringBuilder builder;
        OleDbConnection data;
        OleDbCommand cmd;
        OleDbDataReader reader;



        public Form1()
        {
            
            InitializeComponent();
            textBox3.Text = DateTime.Today.ToString("dd/MM/yyyy");
            textBox4.Text = DateTime.Now.ToString("h:mm:ss tt");

            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            MessageBox.Show("Please Select Database Location.");

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                database = openFileDialog1.FileName;
                if (database.Contains(".mdb") ) 
                {
                    builder = new System.Data.OleDb.OleDbConnectionStringBuilder();
                    builder.Provider = "Microsoft.Jet.OLEDB.4.0";
                    builder.DataSource = database; 
                }
                else
                {
                    MessageBox.Show("Please Select Valid DataBase File");
                    Environment.Exit(0);
                }     

            } 
            else
            {
                MessageBox.Show("Please Select Valid DataBase File");
                Environment.Exit(0);
            }


        }



        private void button1_Click(object sender, EventArgs e)
        {
            
            if ( textF == true & monthS == true)
            {

                
                int index = comboBox1.SelectedIndex + 1;
                data = new OleDbConnection();
                Object returnValue;

                data.ConnectionString = builder.ToString();
                data.Open();



                cmd = new OleDbCommand();

                cmd.CommandText = ("INSERT into Pledges (Sharathon_ID, Sponsor_Type, Amount, TotalAmount, PledgeDate) VALUES (" +
                                "'27','" + index.ToString() + "','" + textBox1.Text + "','" + textBox2.Text + "','" + DateTime.Today.ToString("MM/dd/yyyy") + " " + textBox4.Text + "')");
                cmd.Connection = data;

                cmd.ExecuteNonQuery();

                cmd.CommandText = ("SELECT Pledge_ID, TotalAmount, PledgeDate from Pledges where TotalAmount = " + textBox2.Text + " and PledgeDate = #" + DateTime.Today.ToString("MM/dd/yyyy") + " " + textBox4.Text + "#" );

                returnValue = cmd.ExecuteScalar();

                data.Close();

                comboBox1.ResetText();
                comboBox2.ResetText();
                textBox1.ResetText();
                textBox2.ResetText();
                MessageBox.Show("Pledge has been saved. Pledge ID is: " + returnValue.ToString());

            }
            else if (monthS == false)
            {
                MessageBox.Show("Please select a Payment Type");
            }
            else if (textF == false)
            {
                MessageBox.Show("Please enter a pledge ammount in 'Amount'");
            }
            else if (typeS == false)
            {
                MessageBox.Show("Please select New/Renewal");
            }



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

                textBox4.Text = DateTime.Now.ToString("h:mm:ss tt");

        }

        /*
         *
         *This method is automatically called when the contents of the amount box is changed
         *it calculates thevalue of the monthly ammount based on wether montly or single is chosen in combobox1
         * 
         */

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int j;
            bool contents = textBox1.Text == "";
            bool result = Int32.TryParse(textBox1.Text, out j);
            if (result == true & contents == false)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    textBox2.Text = Convert.ToString(j * 12);
                } 
                else
                {
                    textBox2.Text = Convert.ToString(j);
                }
            }
            else if (result == false & contents == false)
            {
                //throws an error if there is text or non numeric characturs in the amount box
                MessageBox.Show("Please enter only numbers in 'Amount' box.");
            }
            else
            {
                //if textbox1 is blank set textbox 2 to zero
                textBox2.Text = "0";
                return;
            }
            textF = true;

        }

        private void textBox1_TextChanged()
        {
            int j;
            bool contents = textBox1.Text == "";
            bool result = Int32.TryParse(textBox1.Text, out j);
            if (result == true && contents == false)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    textBox2.Text = Convert.ToString(j * 12);
                }
                else
                {
                    textBox2.Text = Convert.ToString(j);
                }
            }
            else if (result == false && contents == false)
            {
                MessageBox.Show("Please enter only numbers in 'Amount' box.");
            }
            else
            {
                textBox2.Text = "0";
            }
            textF = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox1_TextChanged();
            monthS = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            typeS = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        void timer_Tick(object sender, object e)
        {
            textBox4.Text = DateTime.Now.ToString("h:mm:ss tt");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}
