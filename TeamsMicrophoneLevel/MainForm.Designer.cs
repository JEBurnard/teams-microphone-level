namespace TeamsMicrophoneLevel
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.launchTeamsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchTeamsWithPortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label = new System.Windows.Forms.Label();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.trayMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Teams Volume Level";
            this.notifyIcon.Visible = true;
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchTeamsMenuItem,
            this.launchTeamsWithPortMenuItem,
            this.changeScreenToolStripMenuItem,
            this.statusMenuItem,
            this.exitMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(209, 136);
            // 
            // launchTeamsMenuItem
            // 
            this.launchTeamsMenuItem.Name = "launchTeamsMenuItem";
            this.launchTeamsMenuItem.Size = new System.Drawing.Size(208, 22);
            this.launchTeamsMenuItem.Text = "&Launch Teams";
            this.launchTeamsMenuItem.Click += new System.EventHandler(this.LaunchTeams_Click);
            // 
            // launchTeamsWithPortMenuItem
            // 
            this.launchTeamsWithPortMenuItem.Name = "launchTeamsWithPortMenuItem";
            this.launchTeamsWithPortMenuItem.Size = new System.Drawing.Size(208, 22);
            this.launchTeamsWithPortMenuItem.Text = "Launch Teams (with &port)";
            this.launchTeamsWithPortMenuItem.Click += new System.EventHandler(this.LaunchTeamsWithPortMenuItem_Click);
            // 
            // changeScreenToolStripMenuItem
            // 
            this.changeScreenToolStripMenuItem.Name = "changeScreenToolStripMenuItem";
            this.changeScreenToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.changeScreenToolStripMenuItem.Text = "Choose Scr&een";
            this.changeScreenToolStripMenuItem.Click += new System.EventHandler(this.ChooseScreenMenuItem_Click);
            // 
            // statusMenuItem
            // 
            this.statusMenuItem.Name = "statusMenuItem";
            this.statusMenuItem.Size = new System.Drawing.Size(208, 22);
            this.statusMenuItem.Text = "&Status";
            this.statusMenuItem.Click += new System.EventHandler(this.StatusMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(255, 15);
            this.label.TabIndex = 0;
            this.label.Text = "Program entry point - for system tray icon only";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 35);
            this.Controls.Add(this.label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Teams Volume Level";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NotifyIcon notifyIcon;
        private Label label;
        private ContextMenuStrip trayMenu;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem launchTeamsMenuItem;
        private ToolStripMenuItem launchTeamsWithPortMenuItem;
        private ToolStripMenuItem statusMenuItem;
        private ToolStripMenuItem changeScreenToolStripMenuItem;
    }
}