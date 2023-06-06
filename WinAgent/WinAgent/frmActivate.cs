using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAgent.Helpers;

namespace WinAgent
{
    public partial class frmActivate : Form
    {
        public frmActivate()
        {
            InitializeComponent();
            lbActivateResult.Text = string.Empty;
            txtActID.Text = Program.g_setting.activation_key;
            txtCusID.Text = Program.g_setting.customer_id;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Program.g_setting.activation_key = txtActID.Text.Trim();
            Program.g_setting.customer_id = txtCusID.Text.Trim();
            int w_nRet = LicenseHelper.checkActivated(Program.g_setting.customer_id, Program.g_setting.activation_key);
            if (w_nRet == ConstEnv.AGENT_REGISTERED)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (w_nRet == ConstEnv.AGENT_NO_ACTIVATED)
                lbActivateResult.Text = "Customer not Activated.";
            else if (w_nRet == ConstEnv.AGENT_NO_REGISTERED)
                lbActivateResult.Text = "No Registered Customer.";
            else
                lbActivateResult.Text = "Server No Response.";
        }

        private void frmActivate_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.g_setting.Save();
        }
    }
}
