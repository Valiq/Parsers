namespace ParserStellox
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Start = new Button();
            Abort = new Button();
            textBox = new TextBox();
            labelLeft = new Label();
            labelMid = new Label();
            labelRight = new Label();
            labelBot = new Label();
            dataGrid = new DataGridView();
            folderBrowserDialog = new FolderBrowserDialog();
            contextMenu = new ContextMenuStrip(components);
            FillEmptyStripMenu = new ToolStripMenuItem();
            UploadToolStrip = new ToolStripMenuItem();
            FillToolStrip = new ToolStripMenuItem();
            CheckNameToolStrip = new ToolStripMenuItem();
            SaveImgToolStrip = new ToolStripMenuItem();
            ClearToolStrip = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            labBotSecond = new Label();
            labelThreads = new Label();
            comboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            contextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // Start
            // 
            Start.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Start.Location = new Point(40, 550);
            Start.Name = "Start";
            Start.Size = new Size(75, 23);
            Start.TabIndex = 0;
            Start.Text = "Start";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // Abort
            // 
            Abort.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Abort.Location = new Point(40, 579);
            Abort.Name = "Abort";
            Abort.Size = new Size(75, 23);
            Abort.TabIndex = 1;
            Abort.Text = "Abort";
            Abort.UseVisualStyleBackColor = true;
            Abort.Click += Abort_Click;
            // 
            // textBox
            // 
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            textBox.Location = new Point(12, 30);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.Size = new Size(153, 514);
            textBox.TabIndex = 2;
            textBox.TextChanged += textBox_TextChanged;
            // 
            // labelLeft
            // 
            labelLeft.AutoSize = true;
            labelLeft.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            labelLeft.Location = new Point(12, 6);
            labelLeft.Name = "labelLeft";
            labelLeft.Size = new Size(19, 21);
            labelLeft.TabIndex = 3;
            labelLeft.Text = "0";
            // 
            // labelMid
            // 
            labelMid.AutoSize = true;
            labelMid.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            labelMid.Location = new Point(157, 6);
            labelMid.Name = "labelMid";
            labelMid.Size = new Size(17, 21);
            labelMid.TabIndex = 4;
            labelMid.Text = "/";
            // 
            // labelRight
            // 
            labelRight.AutoSize = true;
            labelRight.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            labelRight.Location = new Point(180, 6);
            labelRight.Name = "labelRight";
            labelRight.Size = new Size(19, 21);
            labelRight.TabIndex = 5;
            labelRight.Text = "0";
            // 
            // labelBot
            // 
            labelBot.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelBot.AutoSize = true;
            labelBot.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            labelBot.Location = new Point(171, 579);
            labelBot.Name = "labelBot";
            labelBot.Size = new Size(64, 21);
            labelBot.TabIndex = 6;
            labelBot.Text = "Готово";
            // 
            // dataGrid
            // 
            dataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Location = new Point(171, 30);
            dataGrid.Name = "dataGrid";
            dataGrid.RowTemplate.Height = 25;
            dataGrid.Size = new Size(852, 543);
            dataGrid.TabIndex = 7;
            dataGrid.RowsAdded += dataGrid_RowsAdded;
            dataGrid.RowsRemoved += dataGrid_RowsRemoved;
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new ToolStripItem[] { FillEmptyStripMenu, UploadToolStrip, FillToolStrip, CheckNameToolStrip, SaveImgToolStrip, ClearToolStrip });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new Size(190, 136);
            // 
            // FillEmptyStripMenu
            // 
            FillEmptyStripMenu.Name = "FillEmptyStripMenu";
            FillEmptyStripMenu.Size = new Size(189, 22);
            FillEmptyStripMenu.Text = "Заполнить пропуски";
            FillEmptyStripMenu.Click += FillEmptyStripMenu_Click;
            // 
            // UploadToolStrip
            // 
            UploadToolStrip.Name = "UploadToolStrip";
            UploadToolStrip.Size = new Size(189, 22);
            UploadToolStrip.Text = "Выгрузить всё";
            UploadToolStrip.Click += UploadToolStrip_Click;
            // 
            // FillToolStrip
            // 
            FillToolStrip.Name = "FillToolStrip";
            FillToolStrip.Size = new Size(189, 22);
            FillToolStrip.Text = "Заполнить шаблон";
            FillToolStrip.Click += FillToolStrip_Click;
            // 
            // CheckNameToolStrip
            // 
            CheckNameToolStrip.Name = "CheckNameToolStrip";
            CheckNameToolStrip.Size = new Size(189, 22);
            CheckNameToolStrip.Text = "Проверить названия";
            CheckNameToolStrip.Click += CheckNameToolStrip_Click;
            // 
            // SaveImgToolStrip
            // 
            SaveImgToolStrip.Name = "SaveImgToolStrip";
            SaveImgToolStrip.Size = new Size(189, 22);
            SaveImgToolStrip.Text = "Скачать картинки";
            SaveImgToolStrip.Click += SaveImgToolStrip_Click;
            // 
            // ClearToolStrip
            // 
            ClearToolStrip.Name = "ClearToolStrip";
            ClearToolStrip.Size = new Size(189, 22);
            ClearToolStrip.Text = "Очистить";
            ClearToolStrip.Click += ClearToolStrip_Click;
            // 
            // labBotSecond
            // 
            labBotSecond.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            labBotSecond.AutoSize = true;
            labBotSecond.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            labBotSecond.Location = new Point(821, 579);
            labBotSecond.Name = "labBotSecond";
            labBotSecond.Size = new Size(61, 21);
            labBotSecond.TabIndex = 8;
            labBotSecond.Text = "Буфер";
            // 
            // labelThreads
            // 
            labelThreads.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelThreads.AutoSize = true;
            labelThreads.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            labelThreads.Location = new Point(783, 2);
            labelThreads.Name = "labelThreads";
            labelThreads.Size = new Size(176, 21);
            labelThreads.TabIndex = 9;
            labelThreads.Text = "Количество потоков:";
            // 
            // comboBox
            // 
            comboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox.FormattingEnabled = true;
            comboBox.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            comboBox.Location = new Point(965, 4);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(58, 23);
            comboBox.TabIndex = 10;
            comboBox.Text = "10";
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1035, 613);
            Controls.Add(comboBox);
            Controls.Add(labelThreads);
            Controls.Add(labBotSecond);
            Controls.Add(dataGrid);
            Controls.Add(labelBot);
            Controls.Add(labelRight);
            Controls.Add(labelMid);
            Controls.Add(labelLeft);
            Controls.Add(textBox);
            Controls.Add(Abort);
            Controls.Add(Start);
            Name = "Form";
            Text = "ParserStellox";
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Start;
        private Button Abort;
        private TextBox textBox;
        private Label labelLeft;
        private Label labelMid;
        private Label labelRight;
        internal Label labelBot;
        internal DataGridView dataGrid;
        private FolderBrowserDialog folderBrowserDialog;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem UploadToolStrip;
        private ToolStripMenuItem FillToolStrip;
        private ToolStripMenuItem SaveImgToolStrip;
        private ToolStripMenuItem ClearToolStrip;
        private ToolStripMenuItem FillEmptyStripMenu;
        private OpenFileDialog openFileDialog;
        private ToolStripMenuItem CheckNameToolStrip;
        internal Label labBotSecond;
        private Label labelThreads;
        private ComboBox comboBox;
    }
}