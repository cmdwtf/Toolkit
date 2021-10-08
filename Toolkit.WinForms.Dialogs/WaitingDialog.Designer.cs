namespace cmdwtf.Toolkit.WinForms.Dialogs
{
	partial class WaitingDialog
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
			this.panelBackground = new System.Windows.Forms.Panel();
			this.progressBarInfo = new System.Windows.Forms.ProgressBar();
			this.labelBodyText = new System.Windows.Forms.Label();
			this.labelMainText = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonRetry = new System.Windows.Forms.Button();
			this.timerTimeout = new System.Windows.Forms.Timer(this.components);
			this.timerCheckForDone = new System.Windows.Forms.Timer(this.components);
			this.panelBackground.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelBackground
			// 
			this.panelBackground.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panelBackground.Controls.Add(this.progressBarInfo);
			this.panelBackground.Controls.Add(this.labelBodyText);
			this.panelBackground.Controls.Add(this.labelMainText);
			this.panelBackground.Location = new System.Drawing.Point(0, 0);
			this.panelBackground.Name = "panelBackground";
			this.panelBackground.Size = new System.Drawing.Size(334, 130);
			this.panelBackground.TabIndex = 0;
			this.panelBackground.UseWaitCursor = true;
			// 
			// progressBar1
			// 
			this.progressBarInfo.Location = new System.Drawing.Point(16, 104);
			this.progressBarInfo.Name = "progressBar1";
			this.progressBarInfo.Size = new System.Drawing.Size(307, 17);
			this.progressBarInfo.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBarInfo.TabIndex = 2;
			this.progressBarInfo.UseWaitCursor = true;
			// 
			// labelBodyText
			// 
			this.labelBodyText.Location = new System.Drawing.Point(13, 41);
			this.labelBodyText.Name = "labelBodyText";
			this.labelBodyText.Size = new System.Drawing.Size(310, 60);
			this.labelBodyText.TabIndex = 1;
			this.labelBodyText.Text = "This is the body text. It can contain much more information than the main instruc" +
    "tion text. Its general job is to inform the user more about what is happening wh" +
    "ile they\'re waiting.";
			this.labelBodyText.UseWaitCursor = true;
			// 
			// labelMainText
			// 
			this.labelMainText.AutoSize = true;
			this.labelMainText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMainText.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.labelMainText.Location = new System.Drawing.Point(12, 9);
			this.labelMainText.Name = "labelMainText";
			this.labelMainText.Size = new System.Drawing.Size(234, 21);
			this.labelMainText.TabIndex = 0;
			this.labelMainText.Text = "This is the main instruction text...";
			this.labelMainText.UseWaitCursor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Enabled = false;
			this.buttonCancel.Location = new System.Drawing.Point(247, 136);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.UseWaitCursor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// buttonRetry
			// 
			this.buttonRetry.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonRetry.Enabled = false;
			this.buttonRetry.Location = new System.Drawing.Point(166, 136);
			this.buttonRetry.Name = "buttonRetry";
			this.buttonRetry.Size = new System.Drawing.Size(75, 23);
			this.buttonRetry.TabIndex = 4;
			this.buttonRetry.Text = "Retry";
			this.buttonRetry.UseVisualStyleBackColor = true;
			this.buttonRetry.UseWaitCursor = true;
			this.buttonRetry.Click += new System.EventHandler(this.ButtonRetry_Click);
			// 
			// timerTimeout
			// 
			this.timerTimeout.Tick += new System.EventHandler(this.TimerTimeout_Tick);
			// 
			// timerCheckForDone
			// 
			this.timerCheckForDone.Enabled = true;
			this.timerCheckForDone.Tick += new System.EventHandler(this.TimerCheckForDone_Tick);
			// 
			// WaitingDialog
			// 
			this.AcceptButton = this.buttonRetry;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(334, 165);
			this.ControlBox = false;
			this.Controls.Add(this.buttonRetry);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.panelBackground);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WaitingDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Please wait...";
			this.TopMost = true;
			this.UseWaitCursor = true;
			this.panelBackground.ResumeLayout(false);
			this.panelBackground.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelBackground;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonRetry;
		private System.Windows.Forms.ProgressBar progressBarInfo;
		private System.Windows.Forms.Label labelBodyText;
		private System.Windows.Forms.Label labelMainText;
		private System.Windows.Forms.Timer timerTimeout;
		private System.Windows.Forms.Timer timerCheckForDone;
	}
}
