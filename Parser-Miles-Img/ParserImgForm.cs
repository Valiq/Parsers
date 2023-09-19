using Microsoft.AspNetCore.Http.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.LinkLabel;

namespace ParserImg
{
    public partial class ParserImgForm : Form
    {
        IWebDriver driver;
        DataTable dataTable = new DataTable();

        public ParserImgForm()
        {
            InitializeComponent();
        }

        private void ClickButtonStart(object sender, EventArgs e)
        {
            string[] lines = textBox.Lines;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");

            Parsing(options, lines);
        }

        private async void Parsing(ChromeOptions options, string[] lines)
        {
            driver = new ChromeDriver(options);
            //driver = new ChromeDriver();
            Logix worker = new Logix();

            try
            {
                string mainLink = "", addLink = "";
                foreach (string line in lines)
                {
                    if (line != "")
                    {
                        await Task.Run(() =>
                        {
                            worker.Work(driver, line, out mainLink, out addLink);
                        });

                        DataRow row = dataTable.NewRow();
                        row[dataTable.Columns.IndexOf("Article")] = line;
                        row[dataTable.Columns.IndexOf("Main Link")] = mainLink;
                        row[dataTable.Columns.IndexOf("Additional Links")] = addLink;
                        dataTable.Rows.Add(row);
                    }
                }

                driver.Quit();
                MessageBox.Show("Parsing: Jobs done");
            }
            catch (Exception ex)
            {
                driver.Quit();
                MessageBox.Show(ex.ToString());
            }
        }

        private void ParserImgForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (driver != null)
                driver.Quit();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (dataGrid.Rows.Count != 0)
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath.Text = folderBrowserDialog.SelectedPath;

                    Logix worker = new Logix();

                    for (int i = 0; i < dataGrid.Rows.Count; i++)
                    {
                        string mainUrl = dataGrid.Rows[i].Cells[1].Value.ToString() ?? "";
                        string addUrl = dataGrid.Rows[i].Cells[2].Value.ToString() ?? "";
                        string sufix = dataGrid.Rows[i].Cells[0].Value.ToString() ?? "";

                        worker.WriteFile(mainUrl, addUrl, filePath.Text, sufix.Replace(@"/", ""));
                    }

                    MessageBox.Show("Downloading: Start");
                }
            }
            else
            {
                MessageBox.Show("Отсутствуют ссылки для скачивания");
            }
        }

        private void ParserImgForm_Load(object sender, EventArgs e)
        {
            dataGrid.ContextMenuStrip = contextMenu;
            dataGrid.DataSource = dataTable;
            dataTable.Columns.Add("Article");
            dataTable.Columns.Add("Main Link");
            dataTable.Columns.Add("Additional Links");
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Очистить таблицу ?", "Очистка таблицы", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                dataTable.Clear();
                countRight.Text = "0";
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            countLeft.Text = textBox.Lines.Count().ToString();
        }

        private void dataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            countRight.Text = dataGrid.RowCount.ToString();
        }
    }
}