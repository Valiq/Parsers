using Parser.Core;
using Parser.Core.Habra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Microsoft.Office.Interop.Excel;

namespace Parser
{
    public partial class FormMain : Form
    {
        ParserWorker<string[,]> parser;
        DataTable dataTable = new DataTable();

        public FormMain()
        {
            InitializeComponent();

            parser = new ParserWorker<string[,]>(
                    new HabraParser()
                );

            parser.OnCompleted += Parser_OnCompleted;
            parser.OnNewData += Parser_OnNewData;
        }

        private void Parser_OnNewData(object arg1, string[,] arg2)
        {

            for (int i = 0; i < arg2.GetLength(0); i++)
            {
                bool flag = dataTable.Columns.Contains(arg2[i, 0]);

                if (!flag)
                {
                    dataTable.Columns.Add($"{arg2[i, 0]}");
                }
            }

            DataRow row = dataTable.NewRow();

            //MessageBox.Show(row.ItemArray.Length.ToString() + "\n" + arg2.GetLength(0));

            string buf = "";
            List<string> checkList = new List<string>();

            for (int i = 0; i < arg2.GetLength(0); i++)
            {
                buf = arg2[i, 1].TrimEnd(' ');

                if (!checkList.Contains(arg2[i, 0]))
                {
                    for (int j = 1 + i; j < arg2.GetLength(0); j++)
                    {
                        if (arg2[i, 0] == arg2[j, 0])
                        {
                            arg2[j, 1] = arg2[j, 1].TrimEnd(' ');
                            buf = buf + "; " + arg2[j, 1];
                        }
                    }

                    row[dataTable.Columns.IndexOf(arg2[i, 0])] = buf;
                    checkList.Add(arg2[i, 0]);
                }

            }

            dataTable.Rows.Add(row);


          /*  for (int i = 0; i < arg2.GetLength(0); i++)
                ListTitles.Items.Add($"{arg2[i,0]} \t {arg2[i,1]}");
            ListTitles.Items.Add(""); */
        }

        private void Parser_OnCompleted(object obj)
        {
            MessageBox.Show("All works done!");
        }



        private void ButtonStart_Click(object sender, EventArgs e)
        {
            parser.Settings = new HabraSettings();

            List<string> listArt = ListData.Lines.OfType<string>().ToList();

            parser.Start(listArt);
        }

        private void ButtonAbort_Click(object sender, EventArgs e)
        {
            parser.Abort();
        }

        private void ToolStripClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Очистить таблицу ?", "Очистка таблицы", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
                dataTable.Clear();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            dataGrid.ContextMenuStrip = contextMenuStrip1;

            dataGrid.DataSource = dataTable;
        }

        private void ToolStripCopy_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(ListData.SelectedText);

        }

        private void ToolStripSelectAll_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dataGrid.Rows.Count.ToString());
        }
    }
}
