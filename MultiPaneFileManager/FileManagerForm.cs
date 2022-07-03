using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiPaneFileManager
{
    /// <summary>
    /// Form with file managers
    /// </summary>
    public partial class FileManagerForm : Form
    {
        public int FMCount => FMPanel.Controls.Count;

        public FileManagerForm(params FileManager[] fms)
        {
            InitializeComponent();
            FMPanel.ColumnStyles.Clear();
            FMPanel.ColumnCount = 0;
            if (fms.Length == 0)
            {
                fms = new FileManager[] { new(Images), new(Images) };
            }
            foreach (FileManager fm in fms)
            {
                AddFM(fm);
            }
        }

        /// <summary>
        /// Method for adding a file manager
        /// </summary>
        private void AddFM_Click(object sender, EventArgs e)
        {
            AddFM(new(Images));
        }

        /// <summary>
        /// Method for adding a file manager
        /// </summary>
        public void AddFM(FileManager fm)
        {
            FMPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            FMPanel.Controls.Add(fm, FMPanel.ColumnCount++, 0);
        }

        /// <summary>
        /// Method for removing a file manager
        /// </summary>
        public void RemoveFM(FileManager fm)
        {
            if (FMPanel.Controls.Contains(fm))
            {
                int column = FMPanel.GetColumn(fm);
                FMPanel.Controls.Remove(fm);
                FMPanel.ColumnStyles.RemoveAt(column);
                FMPanel.ColumnCount--;
                for (int i = column; i < FMPanel.ColumnCount; i++)
                {
                    Control c = FMPanel.Controls[i];
                    FMPanel.SetColumn(c, FMPanel.GetColumn(c) - 1);
                }
                if (FMPanel.Controls.Count == 0)
                {
                    Close();
                }
            }
        }
    }
}
