namespace TeamsMicrophoneLevel
{
    partial class LevelForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.volumeControl = new SkiaSharp.Views.Desktop.SKGLControl();
            this.SuspendLayout();
            // 
            // volumeControl
            // 
            this.volumeControl.BackColor = System.Drawing.Color.Black;
            this.volumeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volumeControl.Location = new System.Drawing.Point(0, 0);
            this.volumeControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.volumeControl.Name = "volumeControl";
            this.volumeControl.Size = new System.Drawing.Size(240, 20);
            this.volumeControl.TabIndex = 1;
            this.volumeControl.VSync = true;
            this.volumeControl.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.VolumeControl_PaintSurface);
            this.volumeControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VolumeControl_MouseDown);
            this.volumeControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VolumeControl_MouseMove);
            this.volumeControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.VolumeControl_MouseUp);
            // 
            // LevelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(240, 20);
            this.Controls.Add(this.volumeControl);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(50, 10);
            this.Name = "LevelForm";
            this.Opacity = 0.5D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Teams Microphone Level";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl volumeControl;
    }
}