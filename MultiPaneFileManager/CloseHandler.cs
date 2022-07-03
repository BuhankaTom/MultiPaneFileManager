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
    /// Observation form if no window is open
    /// </summary>
    public partial class CloseHandler : Form
    {
        public CloseHandler()
        {
            InitializeComponent();
        }

        private void Update(object sender, EventArgs e)
        {
            if (Visible)
            {
                Hide();
            }
            if (Application.OpenForms.Count <= 1)
            {
                UpdateTimer.Enabled = false;
                Visible = false;
                Close();
            }
        }
    }
}
