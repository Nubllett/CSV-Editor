using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CSV_Editor
{
    public partial class Form1 : Form
    {
        bool saved = true;
        bool opened = false;

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
                LoadFile();
            }


        }

        private void LoadFile()
        {
            table1.ParseFile(csvFile);
            table1.DataChanged += DataChanged;
        }

        private void DataChanged(object sender, EventArgs e)
        {
            saved = false;
        }

        private string LoadData(string file)
        {
            string retVal = "";

            System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] buffer = new byte[fs.Length];

            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            retVal = Encoding.UTF8.GetString(buffer);

            Array.Clear(buffer, 0, buffer.Length);

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
            else if (data.Contains("\n"))
            {
                line = data.Split('n')[0];
            }

            cols = line.Split(',');

            return cols;
        }

        private string stripData(string rawData)
        {
            string[] data = null;


            if (rawData.Contains("\r\n"))
            {
                data = rawData.Split(new char[] { '\r', '\n' });
            }
            else
            {
                data = rawData.Split('\n');
            }
            //we don't want the first line, this contains the heading names
            string cleanData = "";
            for (int i = 1; i < data.Length;)
            {
                //we don't want an empty line for data.
                if (!string.IsNullOrEmpty(data[i]))
                    cleanData += data[i];
            }


            return cleanData;
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (!opened)
            {
                System.Media.SystemSounds.Exclamation.Play();
                return; }


            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "CSV files *.csv|*.csv";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                saved = saveFile(dlg.FileName);
                System.Media.SystemSounds.Hand.Play();
            }
        }

        private bool saveFile(string fileName)
        {
            bool success = false;

            string data = table1.ToString();

            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            byte[] buffer = null;

            buffer = Encoding.UTF8.GetBytes(data);

            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
            fs.Close();
            fs.Dispose();

            success = true;

            Array.Clear(buffer, 0, buffer.Length);

            data = string.Empty;

            return success;

        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            saved=saveFile(csvFile);

            if (saved)
                System.Media.SystemSounds.Hand.Play();
        }
    }
}
