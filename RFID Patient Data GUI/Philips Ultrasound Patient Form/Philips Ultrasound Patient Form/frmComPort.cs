using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Philips_Ultrasound_Patient_Form
{
    public partial class frmComPort : Form
    {
        private string strComName;

        public string selectedComPort
        {
            get { return strComName; }
        }
        public frmComPort()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            strComName = null;
            this.Close();
        }

        private string LoadFromConfig()
        {
            Properties.Settings set = Properties.Settings.Default;
            return set.COMPort;
        }

        private void frmComPort_Load(object sender, EventArgs e)
        {
            cmbPort.DataSource = SerialPort.GetPortNames();
            cmbPort.Text = LoadFromConfig();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Test Port

            //If ok return
            strComName = cmbPort.Text;
            this.Close();
        }
    }
}