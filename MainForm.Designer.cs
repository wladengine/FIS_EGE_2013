namespace FIS_EGE_2013
{
    partial class MainForm
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbServiceAddress = new System.Windows.Forms.TextBox();
            this.dgvCompGroup = new System.Windows.Forms.DataGridView();
            this.cbStudyLevelGroup = new System.Windows.Forms.ComboBox();
            this.cbLicenseProgram = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnExportXML = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbIsCrimea = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbFaculty = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExportXML_part1 = new System.Windows.Forms.Button();
            this.btnExportXML_part3 = new System.Windows.Forms.Button();
            this.btnUpdateExanInCompetitiveGroup = new System.Windows.Forms.Button();
            this.chbFullImport = new System.Windows.Forms.CheckBox();
            this.btnImportOrdersOfAdmission = new System.Windows.Forms.Button();
            this.btnDeleteApplications = new System.Windows.Forms.Button();
            this.btnUpdateDics = new System.Windows.Forms.Button();
            this.btnExportRecommendedList = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompGroup)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbServiceAddress);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(666, 50);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Адрес сервиса";
            // 
            // tbServiceAddress
            // 
            this.tbServiceAddress.Location = new System.Drawing.Point(13, 19);
            this.tbServiceAddress.Name = "tbServiceAddress";
            this.tbServiceAddress.Size = new System.Drawing.Size(592, 20);
            this.tbServiceAddress.TabIndex = 2;
            this.tbServiceAddress.Text = "http://10.0.3.1:8080/import/importservice.svc";
            // 
            // dgvCompGroup
            // 
            this.dgvCompGroup.AllowUserToAddRows = false;
            this.dgvCompGroup.AllowUserToDeleteRows = false;
            this.dgvCompGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCompGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCompGroup.Location = new System.Drawing.Point(6, 19);
            this.dgvCompGroup.Name = "dgvCompGroup";
            this.dgvCompGroup.ReadOnly = true;
            this.dgvCompGroup.Size = new System.Drawing.Size(597, 396);
            this.dgvCompGroup.TabIndex = 0;
            // 
            // cbStudyLevelGroup
            // 
            this.cbStudyLevelGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStudyLevelGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStudyLevelGroup.FormattingEnabled = true;
            this.cbStudyLevelGroup.Location = new System.Drawing.Point(627, 148);
            this.cbStudyLevelGroup.Name = "cbStudyLevelGroup";
            this.cbStudyLevelGroup.Size = new System.Drawing.Size(101, 21);
            this.cbStudyLevelGroup.TabIndex = 14;
            this.cbStudyLevelGroup.SelectedIndexChanged += new System.EventHandler(this.cbStudyLevelGroup_SelectedIndexChanged);
            // 
            // cbLicenseProgram
            // 
            this.cbLicenseProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLicenseProgram.FormattingEnabled = true;
            this.cbLicenseProgram.Location = new System.Drawing.Point(87, 45);
            this.cbLicenseProgram.Name = "cbLicenseProgram";
            this.cbLicenseProgram.Size = new System.Drawing.Size(306, 21);
            this.cbLicenseProgram.TabIndex = 3;
            this.cbLicenseProgram.SelectedIndexChanged += new System.EventHandler(this.cbLicenseProgram_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dgvCompGroup);
            this.groupBox3.Location = new System.Drawing.Point(12, 148);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(609, 421);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Конкурсные группы";
            // 
            // btnExportXML
            // 
            this.btnExportXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExportXML.Location = new System.Drawing.Point(627, 528);
            this.btnExportXML.Name = "btnExportXML";
            this.btnExportXML.Size = new System.Drawing.Size(101, 41);
            this.btnExportXML.TabIndex = 10;
            this.btnExportXML.Text = "Создать выгрузку";
            this.btnExportXML.UseVisualStyleBackColor = true;
            this.btnExportXML.Click += new System.EventHandler(this.btnExportXML_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chbIsCrimea);
            this.groupBox2.Controls.Add(this.cbLicenseProgram);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbFaculty);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(268, 68);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(460, 74);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // chbIsCrimea
            // 
            this.chbIsCrimea.AutoSize = true;
            this.chbIsCrimea.Location = new System.Drawing.Point(399, 21);
            this.chbIsCrimea.Name = "chbIsCrimea";
            this.chbIsCrimea.Size = new System.Drawing.Size(55, 17);
            this.chbIsCrimea.TabIndex = 4;
            this.chbIsCrimea.Text = "Крым";
            this.chbIsCrimea.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Направление";
            // 
            // cbFaculty
            // 
            this.cbFaculty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFaculty.FormattingEnabled = true;
            this.cbFaculty.Location = new System.Drawing.Point(87, 19);
            this.cbFaculty.Name = "cbFaculty";
            this.cbFaculty.Size = new System.Drawing.Size(306, 21);
            this.cbFaculty.TabIndex = 1;
            this.cbFaculty.SelectedIndexChanged += new System.EventHandler(this.cbFaculty_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Факультет";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbPassword);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbLogin);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 74);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(94, 45);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(100, 20);
            this.tbPassword.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Пароль";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(94, 19);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(100, 20);
            this.tbLogin.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "Имя пользователя";
            // 
            // btnExportXML_part1
            // 
            this.btnExportXML_part1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportXML_part1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExportXML_part1.Location = new System.Drawing.Point(627, 175);
            this.btnExportXML_part1.Name = "btnExportXML_part1";
            this.btnExportXML_part1.Size = new System.Drawing.Size(101, 44);
            this.btnExportXML_part1.TabIndex = 16;
            this.btnExportXML_part1.Text = "Выгрузить AdmissionInfo";
            this.btnExportXML_part1.UseVisualStyleBackColor = true;
            this.btnExportXML_part1.Click += new System.EventHandler(this.btnExportXML_part1_Click);
            // 
            // btnExportXML_part3
            // 
            this.btnExportXML_part3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportXML_part3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExportXML_part3.Location = new System.Drawing.Point(627, 225);
            this.btnExportXML_part3.Name = "btnExportXML_part3";
            this.btnExportXML_part3.Size = new System.Drawing.Size(101, 44);
            this.btnExportXML_part3.TabIndex = 17;
            this.btnExportXML_part3.Text = "Выгрузить Applications";
            this.btnExportXML_part3.UseVisualStyleBackColor = true;
            this.btnExportXML_part3.Click += new System.EventHandler(this.btnExportXML_part3_Click);
            // 
            // btnUpdateExanInCompetitiveGroup
            // 
            this.btnUpdateExanInCompetitiveGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateExanInCompetitiveGroup.Location = new System.Drawing.Point(627, 461);
            this.btnUpdateExanInCompetitiveGroup.Name = "btnUpdateExanInCompetitiveGroup";
            this.btnUpdateExanInCompetitiveGroup.Size = new System.Drawing.Size(101, 34);
            this.btnUpdateExanInCompetitiveGroup.TabIndex = 18;
            this.btnUpdateExanInCompetitiveGroup.Text = "UpdateExamInCompetitiveGroup";
            this.btnUpdateExanInCompetitiveGroup.UseVisualStyleBackColor = true;
            this.btnUpdateExanInCompetitiveGroup.Click += new System.EventHandler(this.btnUpdateExanInCompetitiveGroup_Click);
            // 
            // chbFullImport
            // 
            this.chbFullImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbFullImport.AutoSize = true;
            this.chbFullImport.Location = new System.Drawing.Point(627, 375);
            this.chbFullImport.Name = "chbFullImport";
            this.chbFullImport.Size = new System.Drawing.Size(74, 17);
            this.chbFullImport.TabIndex = 19;
            this.chbFullImport.Text = "Full Import";
            this.chbFullImport.UseVisualStyleBackColor = true;
            // 
            // btnImportOrdersOfAdmission
            // 
            this.btnImportOrdersOfAdmission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportOrdersOfAdmission.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnImportOrdersOfAdmission.Location = new System.Drawing.Point(627, 275);
            this.btnImportOrdersOfAdmission.Name = "btnImportOrdersOfAdmission";
            this.btnImportOrdersOfAdmission.Size = new System.Drawing.Size(101, 44);
            this.btnImportOrdersOfAdmission.TabIndex = 20;
            this.btnImportOrdersOfAdmission.Text = "Выгрузить OrderOfAdm";
            this.btnImportOrdersOfAdmission.UseVisualStyleBackColor = true;
            this.btnImportOrdersOfAdmission.Click += new System.EventHandler(this.btnImportOrdersOfAdmission_Click);
            // 
            // btnDeleteApplications
            // 
            this.btnDeleteApplications.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteApplications.Location = new System.Drawing.Point(627, 501);
            this.btnDeleteApplications.Name = "btnDeleteApplications";
            this.btnDeleteApplications.Size = new System.Drawing.Size(101, 21);
            this.btnDeleteApplications.TabIndex = 21;
            this.btnDeleteApplications.Text = "DeleteApps";
            this.btnDeleteApplications.UseVisualStyleBackColor = true;
            this.btnDeleteApplications.Click += new System.EventHandler(this.btnDeleteApplications_Click);
            // 
            // btnUpdateDics
            // 
            this.btnUpdateDics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateDics.Location = new System.Drawing.Point(627, 434);
            this.btnUpdateDics.Name = "btnUpdateDics";
            this.btnUpdateDics.Size = new System.Drawing.Size(101, 21);
            this.btnUpdateDics.TabIndex = 22;
            this.btnUpdateDics.Text = "UpdateDics";
            this.btnUpdateDics.UseVisualStyleBackColor = true;
            this.btnUpdateDics.Click += new System.EventHandler(this.btnUpdateDics_Click);
            // 
            // btnExportRecommendedList
            // 
            this.btnExportRecommendedList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportRecommendedList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExportRecommendedList.Location = new System.Drawing.Point(627, 325);
            this.btnExportRecommendedList.Name = "btnExportRecommendedList";
            this.btnExportRecommendedList.Size = new System.Drawing.Size(101, 44);
            this.btnExportRecommendedList.TabIndex = 23;
            this.btnExportRecommendedList.Text = "Выгрузить Recommended";
            this.btnExportRecommendedList.UseVisualStyleBackColor = true;
            this.btnExportRecommendedList.Click += new System.EventHandler(this.btnExportRecommendedList_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 581);
            this.Controls.Add(this.btnExportRecommendedList);
            this.Controls.Add(this.btnUpdateDics);
            this.Controls.Add(this.btnDeleteApplications);
            this.Controls.Add(this.btnImportOrdersOfAdmission);
            this.Controls.Add(this.chbFullImport);
            this.Controls.Add(this.btnUpdateExanInCompetitiveGroup);
            this.Controls.Add(this.btnExportXML_part3);
            this.Controls.Add(this.btnExportXML_part1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cbStudyLevelGroup);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnExportXML);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompGroup)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbServiceAddress;
        private System.Windows.Forms.DataGridView dgvCompGroup;
        private System.Windows.Forms.ComboBox cbStudyLevelGroup;
        private System.Windows.Forms.ComboBox cbLicenseProgram;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnExportXML;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbFaculty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExportXML_part1;
        private System.Windows.Forms.Button btnExportXML_part3;
        private System.Windows.Forms.Button btnUpdateExanInCompetitiveGroup;
        private System.Windows.Forms.CheckBox chbFullImport;
        private System.Windows.Forms.Button btnImportOrdersOfAdmission;
        private System.Windows.Forms.Button btnDeleteApplications;
        private System.Windows.Forms.Button btnUpdateDics;
        private System.Windows.Forms.CheckBox chbIsCrimea;
        private System.Windows.Forms.Button btnExportRecommendedList;
    }
}

