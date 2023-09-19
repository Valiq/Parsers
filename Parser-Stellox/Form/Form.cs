using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserStellox.Core;
using System.Data;

namespace ParserStellox
{
    public partial class Form : System.Windows.Forms.Form
    {
        DataTable dataTable;
        private static bool _cancel;

        public Form()
        {
            InitializeComponent();

            FormClosing += Form_FormClosing;
        }

        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _cancel = false;

            if (driverCollection is not null)
            {
                foreach (var driver in driverCollection)
                {
                    driver.Quit();
                }
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            _cancel = true;
            dataTable = new DataTable();
            dataGrid.ContextMenuStrip = contextMenu;
            dataGrid.DataSource = dataTable;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (!isCansel())
            {
                MessageBox.Show("Дождитесь окончания или отмените предыдущий процесс");
                return;
            }

            _cancel = false;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");

            labelBot.Text = "Идёт процесс заполнения...";
            labBotSecond.Text = "Буфер";

            Parsing(options, TextBoxEdit());
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            ParserOzonWorker.cancel = true;

            _cancel = true;

            labelBot.Text = "Завершение процесса...";
            DisabledButtons();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            labelLeft.Text = textBox.Lines.Count().ToString();
        }

        private void dataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            labelRight.Text = (dataGrid.Rows.Count - 1).ToString();
        }

        private async void UploadToolStrip_Click(object sender, EventArgs e)
        {
            if (dataTable.Rows.Count > 0)
            {
                labelBot.Text = "Формирование документа...";

                await Task.Run(() =>
                {
                    ExcelWorker excel = new ExcelWorker();
                    excel.FullUpload(dataTable);
                });

                labelBot.Text = "Готово";
            }
        }

        private async void FillToolStrip_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                labelBot.Text = "Заполнение выбранного шаблона...";
                await Task.Run(() =>
                {
                    ExcelWorker excel = new ExcelWorker();
                    excel.DocGeneration(dataTable, openFileDialog.FileName);
                });
                labelBot.Text = "Готово";
            }
        }

        private void SaveImgToolStrip_Click(object sender, EventArgs e)
        {
            if (dataGrid.Rows.Count != 0)
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    DownloadWorker worker = new DownloadWorker();

                    int counter = 0;
                    for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                    {
                        DataRow row = dataTable.Rows[i];

                        int id = dataTable.Columns.IndexOf("img");
                        string? urlImgs = row[id].ToString();

                        id = dataTable.Columns.IndexOf("Артикул Студии");
                        string? sufix = row[id].ToString();

                        worker.Download(urlImgs, folderBrowserDialog.SelectedPath, sufix);
                    }
                    MessageBox.Show("Downloading: Start");
                }
            }
            else
            {
                MessageBox.Show("Отсутствуют ссылки для скачивания");
            }
        }

        private void ClearToolStrip_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Очистить таблицу?", "Clear", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                dataTable.Clear();
                dataTable.Columns.Clear();
                dataTable = new DataTable();
                dataGrid.DataSource = dataTable;
                labelRight.Text = "0";
            }
        }

        private void FillEmptyStripMenu_Click(object sender, EventArgs e)
        {
            if (!isCansel())
            {
                MessageBox.Show("Дождитесь окончания или отмените предыдущий процесс");
                return;
            }

            List<string> emptyList = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                int id = dataTable.Columns.IndexOf("Ошибка");
                if (id == -1)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(row[id].ToString()))
                {
                    id = dataTable.Columns.IndexOf("Артикул Студии");
                    emptyList.Add(row[id].ToString());

                    dataTable.Rows[i].Delete();
                    i--;
                }
            }

            _cancel = false;

            labelBot.Text = "Идёт процесс заполнения...";
            labBotSecond.Text = "Буфер";

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");

            string[] mas = emptyList.ToArray();
            Parsing(options, mas);
        }

        private void CheckNameToolStrip_Click(object sender, EventArgs e)
        {
            if (!isCansel())
            {
                MessageBox.Show("Дождитесь окончания или отмените предыдущий процесс");
                return;
            }

            _cancel = false;

            labelBot.Text = "Идёт процесс заполнения...";

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");

            ParsingName(options);
        }

        private void dataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            labelRight.Text = (dataGrid.Rows.Count - 1).ToString();
        }

    }
}