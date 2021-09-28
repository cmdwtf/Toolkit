using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdwtf.Toolkit.WinForms
{
	partial class VolumeControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pictureBoxMute = new System.Windows.Forms.PictureBox();
            this.volumeControlBar = new cmdwtf.Toolkit.WinForms.ValueBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMute)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxMute
            // 
            this.pictureBoxMute.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pictureBoxMute.BackgroundImage = global::cmdwtf.Toolkit.WinForms.Resources.Images.Audio_16x;
            this.pictureBoxMute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxMute.Location = new System.Drawing.Point(4, 3);
            this.pictureBoxMute.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxMute.Name = "pictureBoxMute";
            this.pictureBoxMute.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxMute.TabIndex = 0;
            this.pictureBoxMute.TabStop = false;
            this.pictureBoxMute.Click += new System.EventHandler(this.PictureBoxMute_Click);
            this.pictureBoxMute.DoubleClick += new System.EventHandler(this.PictureBoxMute_Click);
            // 
            // volumeControlBar
            // 
            this.volumeControlBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeControlBar.Location = new System.Drawing.Point(28, 3);
            this.volumeControlBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.volumeControlBar.MinimumText = "Mute";
            this.volumeControlBar.Name = "volumeControlBar";
            this.volumeControlBar.Size = new System.Drawing.Size(100, 16);
            this.volumeControlBar.TabIndex = 0;
            this.volumeControlBar.Value = 100;
            this.volumeControlBar.ValueChangedFromMouseInput += new System.EventHandler<System.EventArgs>(this.VolumeControlBar_ValueChangedFromMouseInput);
            this.volumeControlBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VolumeControlBar_MouseDown);
            // 
            // VolumeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.volumeControlBar);
            this.Controls.Add(this.pictureBoxMute);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(64, 22);
            this.Name = "VolumeControl";
            this.Size = new System.Drawing.Size(132, 22);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMute)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBoxMute;
		private cmdwtf.Toolkit.WinForms.ValueBar volumeControlBar;
	}
}
