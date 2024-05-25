using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CSV_Editor
{
    public partial class Form1 : Form
    {
        bool saved = true;

        string csvFile = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                if (MessageBox.Show("You have unsaved data!\nAre you sure you want to quit?", "Unsaved data!",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "CSV files *.csv|*.csv";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                csvFile = dlg.FileName;
                saved = false;
                LoadFile();
            }
        }

        private void LoadFile()
        {
            string rawData = LoadData(csvFile);

            string[] cols = getColumns(rawData);

            DataTable table=new DataTable();
            DataSet ds = new DataSet();
            
            foreach(string col in cols)
            {
                table.Columns.Add(col);
            }

            string data = stripData(rawData);
            string[] lines = null;
            if (data.Contains("\r\n"))
            {
                lines = data.Split(new char[] { '\r', '\n' });
            }
            else
            {
                lines = data.Split('\n');
            }

            foreach(string line in lines)
            {
                DataRow row = table.NewRow();
                string[] colValues = line.Split(',');
                for(int i = 0; i<row.Table.Columns.Count - 1; i++)
                {
                    row[i] = colValues[i];
                }
            }

            ds.Tables.Add(table);
            Grid1.DataSource = ds;
        }

        private string LoadData(string file)
        {
            string retVal = "";

            System.IO.FileStream fs = new System.IO.FileStream(file,System.IO.FileMode.Open,System.IO.FileAccess.Read);
            byte[] buffer = new byte[fs.Length];

            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            retVal = Encoding.UTF8.GetString(buffer);

            Array.Clear(buffer,0, buffer.Length);

            return retVal;
        }

        private string[] getColumns(string data)
        {
            string[] cols = null;

            //we only need the first line.
            string line = "";
            if (data.Contains("\r\n"))
            {
                line = data.Split(new char[] { '\r', '\n' })[0];
            }
            else if(data.Contains("\n"))
            {
                line = data.Split('n')[0];
            }

            cols = line.Split(',');

            return cols;
        }

        private string stripData(string rawData)
        {
            string[] data = null;


            if(rawData.Contains("\r\n"))
            {
                data=rawData.Split(new char[] { '\r', '\n' });
            }
            else
            {
                data = rawData.Split('\n');
            }
            //we don't want the first line, this contains the heading names
            string cleanData = "";
            for(int i =1; i < data.Length;)
            {
                //we don't want an empty line for data.
                if (!string.IsNullOrEmpty( data[i]))
                    cleanData += data[i];   
            }


            return cleanData;
        }
    }
}
