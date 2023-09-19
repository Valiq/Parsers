namespace ParserImg
{
    partial class ParserImgForm
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
            buttonStart = new Button();
            buttonSave = new Button();
            textBox = new TextBox();
            filePath = new TextBox();
            folderBrowserDialog = new FolderBrowserDialog();
            dataGrid = new DataGridView();
            contextMenu = new ContextMenuStrip(components);
            Clear = new ToolStripMenuItem();
            countLeft = new Label();
            label2 = new Label();
            countRight = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            contextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // buttonStart
            // 
            buttonStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonStart.Location = new Point(48, 530);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(75, 23);
            buttonStart.TabIndex = 0;
            buttonStart.Text = "Start";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += ClickButtonStart;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSave.Location = new Point(968, 12);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "SaveDir";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // textBox
            // 
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            textBox.Location = new Point(12, 59);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.ScrollBars = ScrollBars.Both;
            textBox.Size = new Size(170, 465);
            textBox.TabIndex = 3;
            textBox.TextChanged += textBox_TextChanged;
            // 
            // filePath
            // 
            filePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            filePath.Enabled = false;
            filePath.Location = new Point(12, 12);
            filePath.Name = "filePath";
            filePath.Size = new Size(950, 23);
            filePath.TabIndex = 4;
            // 
            // dataGrid
            // 
            dataGrid.AllowUserToAddRows = false;
            dataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Location = new Point(188, 59);
            dataGrid.Name = "dataGrid";
            dataGrid.RowTemplate.Height = 25;
            dataGrid.Size = new Size(855, 494);
            dataGrid.TabIndex = 5;
            dataGrid.RowsAdded += dataGrid_RowsAdded;
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new ToolStripItem[] { Clear });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new Size(127, 26);
            // 
            // Clear
            // 
            Clear.Name = "Clear";
            Clear.Size = new Size(126, 22);
            Clear.Text = "Очистить";
            Clear.Click += Clear_Click;
            // 
            // countLeft
            // 
            countLeft.AutoSize = true;
            countLeft.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            countLeft.Location = new Point(12, 35);
            countLeft.Name = "countLeft";
            countLeft.Size = new Size(19, 21);
            countLeft.TabIndex = 6;
            countLeft.Text = "0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(177, 35);
            label2.Name = "label2";
            label2.Size = new Size(17, 21);
            label2.TabIndex = 7;
            label2.Text = "/";
            // 
            // countRight
            // 
            countRight.AutoSize = true;
            countRight.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            countRight.Location = new Point(188, 35);
            countRight.Name = "countRight";
            countRight.Size = new Size(19, 21);
            countRight.TabIndex = 8;
            countRight.Text = "0";
            // 
            // ParserImgForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1053, 573);
            Controls.Add(countRight);
            Controls.Add(label2);
            Controls.Add(countLeft);
            Controls.Add(dataGrid);
            Controls.Add(filePath);
            Controls.Add(textBox);
            Controls.Add(buttonSave);
            Controls.Add(buttonStart);
            Name = "ParserImgForm";
            Text = "ParserImg";
            FormClosing += ParserImgForm_FormClosing;
            Load += ParserImgForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonStart;
        private Button buttonSave;
        private TextBox textBox;
        private TextBox filePath;
        private FolderBrowserDialog folderBrowserDialog;
        private DataGridView dataGrid;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem Clear;
        private Label countLeft;
        private Label label2;
        private Label countRight;
    }
}