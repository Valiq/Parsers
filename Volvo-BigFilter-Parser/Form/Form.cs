using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParserVolvo.Core;
using System.Data;
using static System.Windows.Forms.Design.AxImporter;

namespace ParserVolvo
{
    public partial class Form : System.Windows.Forms.Form
    {
        internal DataTable dataTable = new DataTable();
        internal static bool _cancel = true;

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

            Parsing(options, textBox.Lines);
        }

        private void Abort_Click(object sender, EventArgs e)
        {
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
                ExcelWorker excel = new ExcelWorker();

                await Task.Run(() =>
                {
                    excel.FullUpload(dataTable);
                });

                labelBot.Text = "Готово";
            }
        }

        private void ClearToolStrip_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Очистить таблицу?", "Clear", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                dataTable.Clear();
                dataTable.Columns.Clear();
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
                if ((id != -1) && (!string.IsNullOrEmpty(row[id].ToString())))
                {
                    id = dataTable.Columns.IndexOf("Артикул Студии");
                    emptyList.Add(row[id].ToString());

                    dataTable.Rows[i].Delete();
                    i--;
                }
            }

            _cancel = false;

            labelBot.Text = "Идёт процесс заполнения...";

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");

            string[] mas = emptyList.ToArray();
            Parsing(options, mas);
        }

        private void dataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            labelRight.Text = (dataGrid.Rows.Count - 1).ToString();
        }

        private void DownloadImgToolStrip_Click(object sender, EventArgs e)
        {
            if (dataGrid.Rows.Count > 0)
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    DownloadWorker worker = new DownloadWorker();

                    #region Volvo
                    //for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                    //{
                    //    DataRow row = dataTable.Rows[i];

                    //    string? sufix = dataTable.Columns.IndexOf("Part Number") != -1 ? row[dataTable.Columns.IndexOf("Part Number")].ToString() : null;
                    //    string? images = dataTable.Columns.IndexOf("Images") != -1 ? row[dataTable.Columns.IndexOf("Images")].ToString() : null;
                    //    string? diagrams = dataTable.Columns.IndexOf("Diagrams") != -1 ? row[dataTable.Columns.IndexOf("Diagrams")].ToString() : null;

                    //    if (!string.IsNullOrEmpty(images))
                    //    {
                    //        CheckDir("Images");
                    //        worker.Download(images, @$"{folderBrowserDialog.SelectedPath}\Images", sufix);
                    //    }

                    //    if (!string.IsNullOrEmpty(diagrams))
                    //    {
                    //        CheckDir("Diagrams");
                    //        worker.Download(diagrams, @$"{folderBrowserDialog.SelectedPath}\Diagrams", sufix);
                    //    }
                    //}
                    #endregion

                    #region BigFilter

                    #endregion

                    MessageBox.Show("Downloading Begin");
                }
            }
        }

        private async void FillToolStrip_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                labelBot.Text = "Заполнение выбранного шаблона...";

                ExcelWorker excel = new ExcelWorker();
                await Task.Run(() =>
                {
                    excel.DocGeneration(dataTable, openFileDialog.FileName);
                });
                labelBot.Text = "Готово";
            }
        }
    }
}