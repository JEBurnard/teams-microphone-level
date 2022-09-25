namespace TeamsMicrophoneLevel
{
    partial class StatusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusForm));
            this.audioGroupBox = new System.Windows.Forms.GroupBox();
            this.deviceValueLabel = new System.Windows.Forms.Label();
            this.deviceLabel = new System.Windows.Forms.Label();
            this.processGroupBox = new System.Windows.Forms.GroupBox();
            this.debugPortValueLabel = new System.Windows.Forms.Label();
            this.debugPortLabel = new System.Windows.Forms.Label();
            this.muteGroupBox = new System.Windows.Forms.GroupBox();
            this.micOnValueLabel = new System.Windows.Forms.Label();
            this.micOnLabel = new System.Windows.Forms.Label();
            this.callSessionActiveValueLabel = new System.Windows.Forms.Label();
            this.callSessionActiveLabel = new System.Windows.Forms.Label();
            this.debugSessionsValueLabel = new System.Windows.Forms.Label();
            this.debugSessionsLabel = new System.Windows.Forms.Label();
            this.connectedValueLabel = new System.Windows.Forms.Label();
            this.connectedLabel = new System.Windows.Forms.Label();
            this.muteDebugPortValueLabel = new System.Windows.Forms.Label();
            this.muteDebugPortLabel = new System.Windows.Forms.Label();
            this.pollTimer = new System.Windows.Forms.Timer(this.components);
            this.audioGroupBox.SuspendLayout();
            this.processGroupBox.SuspendLayout();
            this.muteGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // audioGroupBox
            // 
            this.audioGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audioGroupBox.Controls.Add(this.deviceValueLabel);
            this.audioGroupBox.Controls.Add(this.deviceLabel);
            this.audioGroupBox.Location = new System.Drawing.Point(12, 62);
            this.audioGroupBox.Name = "audioGroupBox";
            this.audioGroupBox.Size = new System.Drawing.Size(358, 44);
            this.audioGroupBox.TabIndex = 0;
            this.audioGroupBox.TabStop = false;
            this.audioGroupBox.Text = "Audio Device";
            // 
            // deviceValueLabel
            // 
            this.deviceValueLabel.AutoSize = true;
            this.deviceValueLabel.Location = new System.Drawing.Point(120, 19);
            this.deviceValueLabel.Name = "deviceValueLabel";
            this.deviceValueLabel.Size = new System.Drawing.Size(24, 15);
            this.deviceValueLabel.TabIndex = 1;
            this.deviceValueLabel.Text = "NA";
            // 
            // deviceLabel
            // 
            this.deviceLabel.AutoSize = true;
            this.deviceLabel.Location = new System.Drawing.Point(6, 19);
            this.deviceLabel.Name = "deviceLabel";
            this.deviceLabel.Size = new System.Drawing.Size(42, 15);
            this.deviceLabel.TabIndex = 0;
            this.deviceLabel.Text = "Name:";
            // 
            // processGroupBox
            // 
            this.processGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.processGroupBox.Controls.Add(this.debugPortValueLabel);
            this.processGroupBox.Controls.Add(this.debugPortLabel);
            this.processGroupBox.Location = new System.Drawing.Point(12, 12);
            this.processGroupBox.Name = "processGroupBox";
            this.processGroupBox.Size = new System.Drawing.Size(358, 44);
            this.processGroupBox.TabIndex = 2;
            this.processGroupBox.TabStop = false;
            this.processGroupBox.Text = "Process";
            // 
            // debugPortValueLabel
            // 
            this.debugPortValueLabel.AutoSize = true;
            this.debugPortValueLabel.Location = new System.Drawing.Point(120, 19);
            this.debugPortValueLabel.Name = "debugPortValueLabel";
            this.debugPortValueLabel.Size = new System.Drawing.Size(24, 15);
            this.debugPortValueLabel.TabIndex = 1;
            this.debugPortValueLabel.Text = "NA";
            // 
            // debugPortLabel
            // 
            this.debugPortLabel.AutoSize = true;
            this.debugPortLabel.Location = new System.Drawing.Point(6, 19);
            this.debugPortLabel.Name = "debugPortLabel";
            this.debugPortLabel.Size = new System.Drawing.Size(70, 15);
            this.debugPortLabel.TabIndex = 0;
            this.debugPortLabel.Text = "Debug Port:";
            // 
            // muteGroupBox
            // 
            this.muteGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.muteGroupBox.Controls.Add(this.micOnValueLabel);
            this.muteGroupBox.Controls.Add(this.micOnLabel);
            this.muteGroupBox.Controls.Add(this.callSessionActiveValueLabel);
            this.muteGroupBox.Controls.Add(this.callSessionActiveLabel);
            this.muteGroupBox.Controls.Add(this.debugSessionsValueLabel);
            this.muteGroupBox.Controls.Add(this.debugSessionsLabel);
            this.muteGroupBox.Controls.Add(this.connectedValueLabel);
            this.muteGroupBox.Controls.Add(this.connectedLabel);
            this.muteGroupBox.Controls.Add(this.muteDebugPortValueLabel);
            this.muteGroupBox.Controls.Add(this.muteDebugPortLabel);
            this.muteGroupBox.Location = new System.Drawing.Point(12, 112);
            this.muteGroupBox.Name = "muteGroupBox";
            this.muteGroupBox.Size = new System.Drawing.Size(358, 106);
            this.muteGroupBox.TabIndex = 2;
            this.muteGroupBox.TabStop = false;
            this.muteGroupBox.Text = "Mute";
            // 
            // micOnValueLabel
            // 
            this.micOnValueLabel.AutoSize = true;
            this.micOnValueLabel.Location = new System.Drawing.Point(120, 79);
            this.micOnValueLabel.Name = "micOnValueLabel";
            this.micOnValueLabel.Size = new System.Drawing.Size(23, 15);
            this.micOnValueLabel.TabIndex = 9;
            this.micOnValueLabel.Text = "No";
            // 
            // micOnLabel
            // 
            this.micOnLabel.AutoSize = true;
            this.micOnLabel.Location = new System.Drawing.Point(6, 79);
            this.micOnLabel.Name = "micOnLabel";
            this.micOnLabel.Size = new System.Drawing.Size(94, 15);
            this.micOnLabel.TabIndex = 8;
            this.micOnLabel.Text = "Microphone On:";
            // 
            // callSessionActiveValueLabel
            // 
            this.callSessionActiveValueLabel.AutoSize = true;
            this.callSessionActiveValueLabel.Location = new System.Drawing.Point(120, 64);
            this.callSessionActiveValueLabel.Name = "callSessionActiveValueLabel";
            this.callSessionActiveValueLabel.Size = new System.Drawing.Size(23, 15);
            this.callSessionActiveValueLabel.TabIndex = 7;
            this.callSessionActiveValueLabel.Text = "No";
            // 
            // callSessionActiveLabel
            // 
            this.callSessionActiveLabel.AutoSize = true;
            this.callSessionActiveLabel.Location = new System.Drawing.Point(6, 64);
            this.callSessionActiveLabel.Name = "callSessionActiveLabel";
            this.callSessionActiveLabel.Size = new System.Drawing.Size(108, 15);
            this.callSessionActiveLabel.TabIndex = 6;
            this.callSessionActiveLabel.Text = "Call Session Active:";
            // 
            // debugSessionsValueLabel
            // 
            this.debugSessionsValueLabel.AutoSize = true;
            this.debugSessionsValueLabel.Location = new System.Drawing.Point(120, 49);
            this.debugSessionsValueLabel.Name = "debugSessionsValueLabel";
            this.debugSessionsValueLabel.Size = new System.Drawing.Size(13, 15);
            this.debugSessionsValueLabel.TabIndex = 5;
            this.debugSessionsValueLabel.Text = "0";
            // 
            // debugSessionsLabel
            // 
            this.debugSessionsLabel.AutoSize = true;
            this.debugSessionsLabel.Location = new System.Drawing.Point(6, 49);
            this.debugSessionsLabel.Name = "debugSessionsLabel";
            this.debugSessionsLabel.Size = new System.Drawing.Size(92, 15);
            this.debugSessionsLabel.TabIndex = 4;
            this.debugSessionsLabel.Text = "Debug Sessions:";
            // 
            // connectedValueLabel
            // 
            this.connectedValueLabel.AutoSize = true;
            this.connectedValueLabel.Location = new System.Drawing.Point(120, 34);
            this.connectedValueLabel.Name = "connectedValueLabel";
            this.connectedValueLabel.Size = new System.Drawing.Size(23, 15);
            this.connectedValueLabel.TabIndex = 3;
            this.connectedValueLabel.Text = "No";
            // 
            // connectedLabel
            // 
            this.connectedLabel.AutoSize = true;
            this.connectedLabel.Location = new System.Drawing.Point(6, 34);
            this.connectedLabel.Name = "connectedLabel";
            this.connectedLabel.Size = new System.Drawing.Size(68, 15);
            this.connectedLabel.TabIndex = 2;
            this.connectedLabel.Text = "Connected:";
            // 
            // muteDebugPortValueLabel
            // 
            this.muteDebugPortValueLabel.AutoSize = true;
            this.muteDebugPortValueLabel.Location = new System.Drawing.Point(120, 19);
            this.muteDebugPortValueLabel.Name = "muteDebugPortValueLabel";
            this.muteDebugPortValueLabel.Size = new System.Drawing.Size(24, 15);
            this.muteDebugPortValueLabel.TabIndex = 1;
            this.muteDebugPortValueLabel.Text = "NA";
            // 
            // muteDebugPortLabel
            // 
            this.muteDebugPortLabel.AutoSize = true;
            this.muteDebugPortLabel.Location = new System.Drawing.Point(6, 19);
            this.muteDebugPortLabel.Name = "muteDebugPortLabel";
            this.muteDebugPortLabel.Size = new System.Drawing.Size(70, 15);
            this.muteDebugPortLabel.TabIndex = 0;
            this.muteDebugPortLabel.Text = "Debug Port:";
            // 
            // pollTimer
            // 
            this.pollTimer.Interval = 1000;
            this.pollTimer.Tick += new System.EventHandler(this.PollTimer_Tick);
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 232);
            this.Controls.Add(this.muteGroupBox);
            this.Controls.Add(this.processGroupBox);
            this.Controls.Add(this.audioGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatusForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Teams Volume Level - Status";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StatusForm_FormClosed);
            this.Shown += new System.EventHandler(this.StatusForm_Shown);
            this.audioGroupBox.ResumeLayout(false);
            this.audioGroupBox.PerformLayout();
            this.processGroupBox.ResumeLayout(false);
            this.processGroupBox.PerformLayout();
            this.muteGroupBox.ResumeLayout(false);
            this.muteGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox audioGroupBox;
        private Label deviceLabel;
        private Label deviceValueLabel;
        private GroupBox processGroupBox;
        private Label debugPortValueLabel;
        private Label debugPortLabel;
        private GroupBox muteGroupBox;
        private Label muteDebugPortValueLabel;
        private Label muteDebugPortLabel;
        private Label connectedLabel;
        private Label debugSessionsLabel;
        private Label connectedValueLabel;
        private Label callSessionActiveValueLabel;
        private Label callSessionActiveLabel;
        private Label debugSessionsValueLabel;
        private Label micOnLabel;
        private Label micOnValueLabel;
        private System.Windows.Forms.Timer pollTimer;
    }
}