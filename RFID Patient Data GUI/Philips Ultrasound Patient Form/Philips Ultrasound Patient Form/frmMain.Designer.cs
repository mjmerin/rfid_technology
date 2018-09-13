namespace Philips_Ultrasound_Patient_Form
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.txtZipcode = new System.Windows.Forms.MaskedTextBox();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAESKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importAESKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.viewKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateNewAESKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureCOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerCheck = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRead
            // 
            this.btnRead.Enabled = false;
            this.btnRead.Location = new System.Drawing.Point(12, 265);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(83, 38);
            this.btnRead.TabIndex = 9;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Enabled = false;
            this.btnWrite.Location = new System.Drawing.Point(108, 265);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(83, 38);
            this.btnWrite.TabIndex = 10;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(204, 265);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(83, 38);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(149, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 54);
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 37);
            this.label8.TabIndex = 19;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAge);
            this.groupBox1.Controls.Add(this.txtZipcode);
            this.groupBox1.Controls.Add(this.cmbGender);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbState);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtCity);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 183);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Patient Data";
            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(55, 73);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(208, 20);
            this.txtAge.TabIndex = 4;
            // 
            // txtZipcode
            // 
            this.txtZipcode.Location = new System.Drawing.Point(174, 150);
            this.txtZipcode.Mask = "00000-9999";
            this.txtZipcode.Name = "txtZipcode";
            this.txtZipcode.Size = new System.Drawing.Size(89, 20);
            this.txtZipcode.TabIndex = 32;
            // 
            // cmbGender
            // 
            this.cmbGender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbGender.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            "M",
            "F"});
            this.cmbGender.Location = new System.Drawing.Point(56, 45);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(48, 21);
            this.cmbGender.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "State:";
            // 
            // cmbState
            // 
            this.cmbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Items.AddRange(new object[] {
            "AA",
            "AE",
            "AK",
            "AL",
            "AP",
            "AR",
            "AS",
            "AZ",
            "CA",
            "CO",
            "CT",
            "DC",
            "DE",
            "FL",
            "FM",
            "GA",
            "GU",
            "HI",
            "IA",
            "ID",
            "IL",
            "IN",
            "KS",
            "KY",
            "LA",
            "MA",
            "MD",
            "ME",
            "MH",
            "MI",
            "MN",
            "MO",
            "MP",
            "MS",
            "MT",
            "NC",
            "ND",
            "NE",
            "NH",
            "NJ",
            "NM",
            "NV",
            "NY",
            "OH",
            "OK",
            "OR",
            "PA",
            "PR",
            "PW",
            "RI",
            "SC",
            "SD",
            "TN",
            "TX",
            "UT",
            "VA",
            "VI",
            "VT",
            "WA",
            "WI",
            "WV",
            "WY"});
            this.cmbState.Location = new System.Drawing.Point(55, 150);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(49, 21);
            this.cmbState.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(115, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Zip Code:";
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(55, 124);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(208, 20);
            this.txtCity.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "City:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(55, 98);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(208, 20);
            this.txtAddress.TabIndex = 5;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(55, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(208, 20);
            this.txtName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Age:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Gender:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Name:";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 50);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(107, 21);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear Fields";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 309);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(299, 22);
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatus.Text = "Ready";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createKeyToolStripMenuItem,
            this.comPortToolStripMenuItem,
            this.fillDataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(299, 24);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // createKeyToolStripMenuItem
            // 
            this.createKeyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportAESKeyToolStripMenuItem,
            this.importAESKeyToolStripMenuItem,
            this.toolStripMenuItem2,
            this.viewKeysToolStripMenuItem,
            this.generateNewAESKeyToolStripMenuItem});
            this.createKeyToolStripMenuItem.Name = "createKeyToolStripMenuItem";
            this.createKeyToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.createKeyToolStripMenuItem.Text = "Encryption";
            // 
            // exportAESKeyToolStripMenuItem
            // 
            this.exportAESKeyToolStripMenuItem.Name = "exportAESKeyToolStripMenuItem";
            this.exportAESKeyToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exportAESKeyToolStripMenuItem.Text = "Export AES Key";
            this.exportAESKeyToolStripMenuItem.Click += new System.EventHandler(this.exportAESKeyToolStripMenuItem_Click);
            // 
            // importAESKeyToolStripMenuItem
            // 
            this.importAESKeyToolStripMenuItem.Name = "importAESKeyToolStripMenuItem";
            this.importAESKeyToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.importAESKeyToolStripMenuItem.Text = "Import AES Key";
            this.importAESKeyToolStripMenuItem.Click += new System.EventHandler(this.importAESKeyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(183, 6);
            // 
            // viewKeysToolStripMenuItem
            // 
            this.viewKeysToolStripMenuItem.Name = "viewKeysToolStripMenuItem";
            this.viewKeysToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewKeysToolStripMenuItem.Text = "View AES Key";
            this.viewKeysToolStripMenuItem.Click += new System.EventHandler(this.viewKeysToolStripMenuItem_Click);
            // 
            // generateNewAESKeyToolStripMenuItem
            // 
            this.generateNewAESKeyToolStripMenuItem.Name = "generateNewAESKeyToolStripMenuItem";
            this.generateNewAESKeyToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.generateNewAESKeyToolStripMenuItem.Text = "Generate New AES Key";
            this.generateNewAESKeyToolStripMenuItem.Click += new System.EventHandler(this.generateNewAESKeyToolStripMenuItem_Click);
            // 
            // comPortToolStripMenuItem
            // 
            this.comPortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureCOMPortToolStripMenuItem});
            this.comPortToolStripMenuItem.Name = "comPortToolStripMenuItem";
            this.comPortToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.comPortToolStripMenuItem.Text = "Serial Port";
            // 
            // configureCOMPortToolStripMenuItem
            // 
            this.configureCOMPortToolStripMenuItem.Name = "configureCOMPortToolStripMenuItem";
            this.configureCOMPortToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.configureCOMPortToolStripMenuItem.Text = "Configure COM Port";
            this.configureCOMPortToolStripMenuItem.Click += new System.EventHandler(this.configureCOMPortToolStripMenuItem_Click);
            // 
            // fillDataToolStripMenuItem
            // 
            this.fillDataToolStripMenuItem.Name = "fillDataToolStripMenuItem";
            this.fillDataToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.fillDataToolStripMenuItem.Text = "Fill Data";
            this.fillDataToolStripMenuItem.Click += new System.EventHandler(this.fillDataToolStripMenuItem_Click);
            // 
            // timerCheck
            // 
            this.timerCheck.Interval = 1000;
            this.timerCheck.Tick += new System.EventHandler(this.timerCheck_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 331);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnRead);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Philips RFID Project";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.MaskedTextBox txtZipcode;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAESKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importAESKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem viewKeysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateNewAESKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureCOMPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fillDataToolStripMenuItem;
        private System.Windows.Forms.Timer timerCheck;
    }
}

