using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BoxView {
	partial class Form1 {
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.DrawTimer = new System.Windows.Forms.Timer(this.components);
			this.Canvas = new BoxView.Display();
			this.SuspendLayout();
			// 
			// DrawTimer
			// 
			this.DrawTimer.Enabled = true;
			this.DrawTimer.Interval = 250;
			this.DrawTimer.Tick += new System.EventHandler(this.DrawTimer_Tick);
			// 
			// Canvas
			// 
			this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Canvas.Location = new System.Drawing.Point(0, 0);
			this.Canvas.MinimumSize = new System.Drawing.Size(512, 512);
			this.Canvas.Name = "Canvas";
			this.Canvas.Size = new System.Drawing.Size(512, 512);
			this.Canvas.TabIndex = 0;
			this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintCanvas);
			this.Canvas.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseWheel);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.Canvas);
			this.DoubleBuffered = true;
			this.Name = "Form1";
			this.Text = "Boxygen Preview";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private Display Canvas;
		private Timer DrawTimer;
	}
}

