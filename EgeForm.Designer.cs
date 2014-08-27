namespace FIS_EGE_2013
{
    partial class EgeForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvAbits = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.tbReq = new System.Windows.Forms.TextBox();
            this.tbAns = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnPacketImport = new System.Windows.Forms.Button();
            this.btnUpdateExists = new System.Windows.Forms.Button();
            this.btnGetExists = new System.Windows.Forms.Button();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.tbRegNumSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFIOSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbits)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(722, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvAbits
            // 
            this.dgvAbits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAbits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAbits.Location = new System.Drawing.Point(12, 41);
            this.dgvAbits.Name = "dgvAbits";
            this.dgvAbits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAbits.Size = new System.Drawing.Size(704, 417);
            this.dgvAbits.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 461);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Всего:";
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(297, 461);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 13);
            this.lblCount.TabIndex = 6;
            // 
            // tbReq
            // 
            this.tbReq.AcceptsReturn = true;
            this.tbReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReq.Location = new System.Drawing.Point(792, 12);
            this.tbReq.Multiline = true;
            this.tbReq.Name = "tbReq";
            this.tbReq.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbReq.Size = new System.Drawing.Size(279, 201);
            this.tbReq.TabIndex = 7;
            // 
            // tbAns
            // 
            this.tbAns.AcceptsReturn = true;
            this.tbAns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAns.Location = new System.Drawing.Point(792, 219);
            this.tbAns.Multiline = true;
            this.tbAns.Name = "tbAns";
            this.tbAns.Size = new System.Drawing.Size(279, 295);
            this.tbAns.TabIndex = 8;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(722, 219);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(64, 23);
            this.btnImport.TabIndex = 9;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnPacketImport
            // 
            this.btnPacketImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPacketImport.Location = new System.Drawing.Point(722, 248);
            this.btnPacketImport.Name = "btnPacketImport";
            this.btnPacketImport.Size = new System.Drawing.Size(64, 36);
            this.btnPacketImport.TabIndex = 10;
            this.btnPacketImport.Text = "Import Packet";
            this.btnPacketImport.UseVisualStyleBackColor = true;
            this.btnPacketImport.Click += new System.EventHandler(this.btnPacketImport_Click);
            // 
            // btnUpdateExists
            // 
            this.btnUpdateExists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateExists.Location = new System.Drawing.Point(722, 86);
            this.btnUpdateExists.Name = "btnUpdateExists";
            this.btnUpdateExists.Size = new System.Drawing.Size(64, 37);
            this.btnUpdateExists.TabIndex = 11;
            this.btnUpdateExists.Text = "Update All Exists";
            this.btnUpdateExists.UseVisualStyleBackColor = true;
            this.btnUpdateExists.Click += new System.EventHandler(this.btnUpdateExists_Click);
            // 
            // btnGetExists
            // 
            this.btnGetExists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetExists.Location = new System.Drawing.Point(722, 129);
            this.btnGetExists.Name = "btnGetExists";
            this.btnGetExists.Size = new System.Drawing.Size(64, 37);
            this.btnGetExists.TabIndex = 12;
            this.btnGetExists.Text = "Update 1 Exists";
            this.btnGetExists.UseVisualStyleBackColor = true;
            this.btnGetExists.Click += new System.EventHandler(this.btnGetExists_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAll.Location = new System.Drawing.Point(722, 41);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(64, 25);
            this.btnSaveAll.TabIndex = 13;
            this.btnSaveAll.Text = "Save all";
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(12, 491);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(704, 23);
            this.pbProgress.TabIndex = 14;
            // 
            // tbRegNumSearch
            // 
            this.tbRegNumSearch.Location = new System.Drawing.Point(42, 15);
            this.tbRegNumSearch.Name = "tbRegNumSearch";
            this.tbRegNumSearch.Size = new System.Drawing.Size(124, 20);
            this.tbRegNumSearch.TabIndex = 15;
            this.tbRegNumSearch.TextChanged += new System.EventHandler(this.tbRegNumSearch_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "ИД";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "ФИО";
            // 
            // tbFIOSearch
            // 
            this.tbFIOSearch.Location = new System.Drawing.Point(212, 14);
            this.tbFIOSearch.Name = "tbFIOSearch";
            this.tbFIOSearch.Size = new System.Drawing.Size(335, 20);
            this.tbFIOSearch.TabIndex = 18;
            this.tbFIOSearch.TextChanged += new System.EventHandler(this.tbFIOSearch_TextChanged);
            // 
            // EgeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 526);
            this.Controls.Add(this.tbFIOSearch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbRegNumSearch);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.btnSaveAll);
            this.Controls.Add(this.btnGetExists);
            this.Controls.Add(this.btnUpdateExists);
            this.Controls.Add(this.btnPacketImport);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.tbAns);
            this.Controls.Add(this.tbReq);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvAbits);
            this.Controls.Add(this.btnSave);
            this.Name = "EgeForm";
            this.Text = "EgeForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvAbits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.TextBox tbReq;
        private System.Windows.Forms.TextBox tbAns;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnPacketImport;
        private System.Windows.Forms.Button btnUpdateExists;
        private System.Windows.Forms.Button btnGetExists;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.TextBox tbRegNumSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFIOSearch;
    }
}