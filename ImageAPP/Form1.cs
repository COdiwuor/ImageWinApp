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
using System.IO;

namespace ImageAPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=images;Integrated Security=True");
        SqlCommand cmd;

        //upload image button
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Select image(*.JPG; *.PNG; *.GIF) | *.JPG; *.PNG; *GIF";
            if(openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("INSERT INTO Table1(name_image,image1) VALUES(@name_image,@image1)", conn);
            cmd.Parameters.AddWithValue("name_image", textBox1.Text);
            MemoryStream memstr = new MemoryStream();
            pictureBox1.Image.Save(memstr, pictureBox1.Image.RawFormat);
            cmd.Parameters.AddWithValue("image1", memstr.ToArray());
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Data Inserted Successfully");
            load_data();
        }

        private void load_data()
        {
            cmd = new SqlCommand("SELECT * FROM Table1 ORDER BY ID ASC", conn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand= cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.RowTemplate.Height = 75;
            dataGridView1.DataSource = dt;
            DataGridViewImageColumn pic1 = new DataGridViewImageColumn();
            pic1 = (DataGridViewImageColumn)dataGridView1.Columns[2];
            pic1.ImageLayout = DataGridViewImageCellLayout.Stretch;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load_data();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            id1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            MemoryStream ms =  new MemoryStream((byte[])dataGridView1.CurrentRow.Cells[2].Value);
            pictureBox1.Image = Image.FromStream(ms);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("UPDATE Table1 Set name_image = @name_image,image1 = @image1 WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("name_image", textBox1.Text);
            MemoryStream memstr = new MemoryStream();
            pictureBox1.Image.Save(memstr, pictureBox1.Image.RawFormat);
            cmd.Parameters.AddWithValue("image1", memstr.ToArray());
            cmd.Parameters.AddWithValue("id", id1.Text);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Data Updated Successfully");
            load_data();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("DELETE FROM Table1 WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id1.Text);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Data Deleted Successfully");
            load_data();
            pictureBox1.Image = null;
            textBox1.Text = "";
            id1.Text = "";
        
        }

        //code to save/download image to the computer
        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "JPG(*.JPG) | *.jpg | PNG(*.PNG) | *.png";
            saveFileDialog.Filter = "JPG|*.jpg| PNG| *.png| JPEG| *.jpeg| GIF| *.gif| TXT| *.txt ";

            if (saveFileDialog.ShowDialog() == DialogResult.OK) 
            {
                pictureBox1.Image.Save(saveFileDialog.FileName);
            }
        }
    }
}
