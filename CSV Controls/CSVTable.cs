using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSV_Controls
{
    public partial class CSVTable : UserControl
    {

        private string fileName;

        private string[] headers;
        private string[][] data;

        List<Control> controls;

        public  EventHandler DataChanged;
        
        private char sep = ','; // the seperator character
        [Category("CSV")]
        public char Seperator
        {
            get => sep;
            set => sep = value;    
        }


        private int _colWidth=100;
        [Category("CSV")]
        public int ColWidth { get => _colWidth; set => _colWidth = value; }

        public CSVTable()
        {
            InitializeComponent();
        }

        private void CSVTable_Load(object sender, EventArgs e)
        {
            DataChanged += newData;
        }

        public void newData(object o, EventArgs e)
        {

        }

        public void ParseFile(string csvFile)
        {
            fileName = csvFile;

            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            fs.Dispose();

            string data = Encoding.UTF8.GetString(buffer);
            Array.Clear(buffer, 0, data.Length);

            Parse(data);
        }

        public void Parse(string data)
        {
            controls = new List<Control>();

            createHeaders(data);
            createControls();

            foreach(Control control in controls)
            {
                panelData.Controls.Add(control);
            }
        }

        private void createHeaders(string data)
        {
            if (headers != null)
            {
                Array.Clear(headers, 0, headers.Length);
                headers = null;
            }

            string line = "";
            if (data.Contains("\r\n"))
            {
                line = data.Split(new char[] { '\r', '\n' })[0];
            }
            else
            {
                line = data.Split('\n')[0];
            }

            headers = line.Split(sep);

            line = string.Empty;

            createTable(data);
        }

        private void createTable(string rawData)
        {
            string[] lines = null;
            if (rawData.Contains("\r\n"))
                lines = rawData.Split(new char[] { '\r', '\n' });
            else
                lines = rawData.Split('\n');

            List<string[]> rows = new List<string[]>();

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                    continue;

                string[] cols = lines[i].Split(sep);
                rows.Add(cols);
            }

            data = new string[rows.Count][];
            for (int i = 0; i < rows.Count; i++)
            {
                data[i] = rows[i];
            }
        }

        private void createControls()
        {
            int prevX = 0;
            int controlHeight = 0;

            for (int i = 0; i < headers.Length; i++)
            {
                Label l = new Label();
                l.Text = headers[i];
                l.Top = 0;
                l.Left = prevX;
                l.Width = ColWidth;

                prevX +=l.Width;
                if (controlHeight == 0) controlHeight = l.Height;

                controls.Add(l);
            }

            int prevY = controlHeight;

            for (int i = 0; i < data.Length; i++)
            {
                prevX = 0;
                for (int j = 0; j < data[i].Length; j++)
                {
                    TextBox t = new TextBox();
                    t.Text = data[i][j];
                    t.Top = prevY;
                    t.Left = prevX;
                    t.Width = ColWidth;

                    prevX += ColWidth;

                    t.TextChanged += T_TextChanged;
                    controls.Add(t);
                }
                prevY += controlHeight;
            }
        }

        private void T_TextChanged(object sender, EventArgs e)
        {
            this.DataChanged?.Invoke(sender, e);
        }
    }
}
