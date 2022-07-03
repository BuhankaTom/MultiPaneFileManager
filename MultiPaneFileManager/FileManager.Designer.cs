
namespace MultiPaneFileManager
{
    partial class FileManager
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BackButton = new System.Windows.Forms.Button();
            this.DirectoryTextBox = new System.Windows.Forms.TextBox();
            this.DrivesComboBox = new System.Windows.Forms.ComboBox();
            this.FilesListView = new System.Windows.Forms.ListView();
            this.NameCollumn = new System.Windows.Forms.ColumnHeader();
            this.TypeCollumn = new System.Windows.Forms.ColumnHeader();
            this.SizeCollumn = new System.Windows.Forms.ColumnHeader();
            this.DateCollumn = new System.Windows.Forms.ColumnHeader();
            this.ShowInNewWindowButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Controls.Add(this.BackButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.DirectoryTextBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.DrivesComboBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.FilesListView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ShowInNewWindowButton, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(760, 537);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // BackButton
            // 
            this.BackButton.Image = global::MultiPaneFileManager.Properties.Resources.back;
            this.BackButton.Location = new System.Drawing.Point(3, 3);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(32, 25);
            this.BackButton.TabIndex = 1;
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // DirectoryTextBox
            // 
            this.DirectoryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DirectoryTextBox.Location = new System.Drawing.Point(125, 3);
            this.DirectoryTextBox.Name = "DirectoryTextBox";
            this.DirectoryTextBox.Size = new System.Drawing.Size(562, 23);
            this.DirectoryTextBox.TabIndex = 2;
            this.DirectoryTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DirectoryTextBox_KeyDown);
            // 
            // DrivesComboBox
            // 
            this.DrivesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DrivesComboBox.FormattingEnabled = true;
            this.DrivesComboBox.Location = new System.Drawing.Point(41, 3);
            this.DrivesComboBox.Name = "DrivesComboBox";
            this.DrivesComboBox.Size = new System.Drawing.Size(78, 23);
            this.DrivesComboBox.TabIndex = 0;
            this.DrivesComboBox.SelectedIndexChanged += new System.EventHandler(this.DrivesComboBox_SelectedIndexChanged);
            // 
            // FilesListView
            // 
            this.FilesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameCollumn,
            this.TypeCollumn,
            this.SizeCollumn,
            this.DateCollumn});
            this.tableLayoutPanel1.SetColumnSpan(this.FilesListView, 5);
            this.FilesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilesListView.HideSelection = false;
            this.FilesListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.FilesListView.Location = new System.Drawing.Point(3, 34);
            this.FilesListView.Name = "FilesListView";
            this.FilesListView.Size = new System.Drawing.Size(754, 500);
            this.FilesListView.TabIndex = 3;
            this.FilesListView.UseCompatibleStateImageBehavior = false;
            this.FilesListView.View = System.Windows.Forms.View.Details;
            this.FilesListView.DoubleClick += new System.EventHandler(this.FilesListView_DoubleClick);
            this.FilesListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FilesListView_MouseUp);
            // 
            // NameCollumn
            // 
            this.NameCollumn.Text = "Name";
            this.NameCollumn.Width = 200;
            // 
            // TypeCollumn
            // 
            this.TypeCollumn.Text = "Type";
            this.TypeCollumn.Width = 100;
            // 
            // SizeCollumn
            // 
            this.SizeCollumn.Text = "Size";
            this.SizeCollumn.Width = 100;
            // 
            // DateCollumn
            // 
            this.DateCollumn.Text = "Date of change";
            this.DateCollumn.Width = 150;
            // 
            // ShowInNewWindowButton
            // 
            this.ShowInNewWindowButton.Image = global::MultiPaneFileManager.Properties.Resources.open_new_window;
            this.ShowInNewWindowButton.Location = new System.Drawing.Point(693, 3);
            this.ShowInNewWindowButton.Name = "ShowInNewWindowButton";
            this.ShowInNewWindowButton.Size = new System.Drawing.Size(29, 23);
            this.ShowInNewWindowButton.TabIndex = 4;
            this.ShowInNewWindowButton.UseVisualStyleBackColor = true;
            this.ShowInNewWindowButton.Click += new System.EventHandler(this.ShowInNewWindowButton_Click);
            // 
            // button1
            // 
            this.button1.Image = global::MultiPaneFileManager.Properties.Resources.remove;
            this.button1.Location = new System.Drawing.Point(728, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Remove_Click);
            // 
            // FileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FileManager";
            this.Size = new System.Drawing.Size(760, 537);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.ComboBox DrivesComboBox;
        private System.Windows.Forms.TextBox DirectoryTextBox;
        private System.Windows.Forms.ListView FilesListView;
        private System.Windows.Forms.ColumnHeader NameCollumn;
        private System.Windows.Forms.ColumnHeader SizeCollumn;
        private System.Windows.Forms.ColumnHeader DateCollumn;
        private System.Windows.Forms.ColumnHeader TypeCollumn;
        private System.Windows.Forms.Button ShowInNewWindowButton;
        private System.Windows.Forms.Button button1;
    }
}
