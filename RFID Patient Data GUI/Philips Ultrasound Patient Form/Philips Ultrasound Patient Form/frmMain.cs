using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Philips_Ultrasound_Patient_Form
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        // Initialize the encryption and compression class
        // needed for the sending and receiving data
        private Encryption enc = new Encryption();
        private Compression comp = new Compression();
        Serial port;

        // This method will run when the user clicks the read button
        private void btnRead_Click(object sender, EventArgs e)
        {
            timerCheck.Stop();
            toolStripStatus.Text = "Reading...";
            Application.DoEvents();

            byte[] receivedData = port.TalkRFIDByte("readtag"); // Read data from the port

            if(receivedData != null){
                try
                {
                    IFormatter formatter = new BinaryFormatter();

                    byte[] encryptedData = receivedData; // Store data received from port 5 into encrypted data.

                    // Decrypt the data
                    // MemoryStream decryptedStream = new MemoryStream(enc.Decrypt(encryptedData));
                    MemoryStream serializedData = new MemoryStream(comp.Decompress(enc.Decrypt(encryptedData)));
                    PatientData outputData = (PatientData)formatter.Deserialize(serializedData);

                    // Put the patient data to associated fields on the form. 

                    txtName.Text = outputData.name;
                    cmbGender.Text = outputData.gender;
                    txtAge.Text = outputData.age.ToString();
                    txtAddress.Text = outputData.address;
                    txtCity.Text = outputData.city;
                    cmbState.Text = outputData.state;
                    txtZipcode.Text = outputData.zipcode;

                    // Close the streams used
                    serializedData.Close();

                    toolStripStatus.Text = "Reading Done!";
                }
                catch
                {
                    toolStripStatus.Text = "Reading Error!";
                }
            }
            else
            {
                toolStripStatus.Text = "Tag is not responding!";
            }
            timerCheck.Start();
        }

        // This method will run when the user clicks the write button
        private void btnWrite_Click(object sender, EventArgs e)
        {
            timerCheck.Stop();
            if (formValid())
            {
                toolStripStatus.Text = "Writing...(please do not remove the tag)";
                Application.DoEvents();

                try
                {
                    PatientData data = new PatientData(txtName.Text, cmbGender.Text, txtAge.Text, txtAddress.Text, txtCity.Text, cmbState.Text, txtZipcode.Text);
                    //PatientData data = new PatientData(txtName.Text, cmbGender.Text, txtAge.Text);
                    MemoryStream serializeData = new MemoryStream();
                    IFormatter formatter = new BinaryFormatter();

                    //Serialize the data
                    formatter.Serialize(serializeData, data);

                    //Compress the Data
                    MemoryStream compressedData = new MemoryStream();
                    comp.Compress(serializeData.ToArray(), compressedData);

                    //Encrypt the data 
                    byte[] output = enc.Encrypt(compressedData.ToArray());
                    Stream fileStream = new FileStream("patient_data.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                    fileStream.Write(output, 0, output.Length);

                    // Close the streams used
                    compressedData.Close();
                    fileStream.Close();
                    serializeData.Close();

                    if (port.Write(output))
                    {
                        System.Threading.Thread.Sleep(2500);
                        toolStripStatus.Text = "Writing Done! (" + output.Length.ToString() + " Bytes)";
                    }
                    else
                    {
                        toolStripStatus.Text = "Writing Error! Tag is not found.";
                    }
                    //FileInfo fileInfo = new FileInfo("patient_data.bin");
                    //toolStripStatus.Text = "Writing Done! (" + fileInfo.Length.ToString() + " Bytes)";
                }
                catch
                {
                    toolStripStatus.Text = "Writing Error! Please try again.";
                }
            }
            else
                MessageBox.Show("Please enter the required fields (Name, Age, and Gender).", "Form is not valid", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            timerCheck.Start();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            port.Close();
            // Exit the application
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear the Form
            txtName.Text = "";
            cmbGender.Text = "";
            txtAge.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            cmbState.Text = "";
            txtZipcode.Text = "";

            toolStripStatus.Text = "Ready";
        }

        private bool formValid()
        {
            if (txtName.Text == "" || txtAge.Text == "" || cmbGender.Text == "")
                return false;
            else
                return true;
        }

        private void exportAESKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream keyDataStream = null;
            
            // Open a Save File Dialog and get the file path for saving the AES key
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "AES key files (*.aes)|*.aes|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((keyDataStream = saveFileDialog.OpenFile()) != null)
                {
                    enc.ExportKey(keyDataStream);
                    keyDataStream.Close();
                }
            }
            toolStripStatus.Text = "Public Key Exported!";
        }


        private void importAESKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream keyDataStream = null;

            // Open an Open File Dialog and get the file path for reading the AES key
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            openFileDialog.Filter = "AES key files (*.aes)|*.aes|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((keyDataStream = openFileDialog.OpenFile()) != null)
                    {
                        using (keyDataStream)
                        {
                            enc.ImportKey(keyDataStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            toolStripStatus.Text = "AES Key Sucessfully Imported!";
        }

        // This function will be removed on the final version
        // for security reason.
        private void viewKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the AES Keys
            MessageBox.Show(enc.GetEncryptionKey(), "Encryption Keys");
        }

        private void generateNewAESKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to generate a new key? You will not be able to decrypt older data", "Generate New Key Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
                enc.GenerateNewKey();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Initialize the port needed for communications
            string portNo = getComPort();
            port = new Serial(portNo);

            if (!port.Open())
                MessageBox.Show("Cannot open" + portNo + "! Please change the COM Port settings.", "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                timerCheck.Start();
            }
        }

        private void configureCOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            frmComPort frmComPort = new frmComPort();
            frmComPort.ShowDialog();

            string selectedPort = frmComPort.selectedComPort;

            if (selectedPort != null)
            {
                port.ComName = selectedPort;

                //Save the ComName into application setting
                Properties.Settings set = Properties.Settings.Default;
                set.COMPort = selectedPort;
                set.Save();
            }
        }

        private string getComPort()
        {
            Properties.Settings set = Properties.Settings.Default;
            return set.COMPort;
        }

        private void fillDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtName.Text = "Mark John Merin";
            txtAge.Text = "3";
            cmbGender.Text = "F";
            txtAddress.Text = "1234 Somewhere Out There St";
            txtCity.Text = "Unknown";
            cmbState.Text = "WA";
            txtZipcode.Text = "00911";
        }

        private void setProtocol()
        {
            ////Initialize EVM
            //toolStripStatus.Text = port.TalkRFIDString("010C00030410002101000000");
            //toolStripStatus.Text = port.TalkRFIDString("0109000304F0000000");
            //toolStripStatus.Text = port.TalkRFIDString("0109000304F1FF0000");
        }

        private void setProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setProtocol();
        }

        private void timerCheck_Tick(object sender, EventArgs e)
        {
            timerCheck.Stop();
            try
            {
                string response = port.TalkRFIDString("findtag");
                if (response.Contains("YES"))
                {
                    btnRead.Enabled = true;
                    btnWrite.Enabled = true;
                }
                else
                {
                    btnRead.Enabled = false;
                    btnWrite.Enabled = false;
                }
                Application.DoEvents();
            }
            catch
            {
                //Do nothing
            }
            timerCheck.Start();
        }
    }
}