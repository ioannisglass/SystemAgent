using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAgent
{
    public partial class frmActivate : Form
    {
        public frmActivate()
        {
            InitializeComponent();
            txtActID.Text = Program.g_setting.activation_key;
            txtCusID.Text = Program.g_setting.customer_id;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
