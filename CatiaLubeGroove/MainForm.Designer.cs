/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 18/11/2015
 * Time: 06:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace CatiaLubeGroove
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button buttonActionX;
		private System.Windows.Forms.Button buttonZamknuti;
		private System.Windows.Forms.Button buttonActionW;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.TextBox textBoxWidth;
		private System.Windows.Forms.TextBox textBoxDepth;
		private System.Windows.Forms.Label labelDepth;
		private System.Windows.Forms.TextBox textBoxEdges;
		private System.Windows.Forms.Label labelEdges;
		private System.Windows.Forms.CheckBox checkBoxIsolateAuto;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonActionX = new System.Windows.Forms.Button();
			this.buttonZamknuti = new System.Windows.Forms.Button();
			this.buttonActionW = new System.Windows.Forms.Button();
			this.labelWidth = new System.Windows.Forms.Label();
			this.textBoxWidth = new System.Windows.Forms.TextBox();
			this.textBoxDepth = new System.Windows.Forms.TextBox();
			this.labelDepth = new System.Windows.Forms.Label();
			this.textBoxEdges = new System.Windows.Forms.TextBox();
			this.labelEdges = new System.Windows.Forms.Label();
			this.checkBoxIsolateAuto = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// buttonActionX
			// 
			this.buttonActionX.Location = new System.Drawing.Point(12, 6);
			this.buttonActionX.Name = "buttonActionX";
			this.buttonActionX.Size = new System.Drawing.Size(25, 23);
			this.buttonActionX.TabIndex = 0;
			this.buttonActionX.TabStop = false;
			this.buttonActionX.Text = "X";
			this.buttonActionX.UseVisualStyleBackColor = true;
			this.buttonActionX.Click += new System.EventHandler(this.ButtonActionXClick);
			// 
			// buttonZamknuti
			// 
			this.buttonZamknuti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonZamknuti.BackColor = System.Drawing.Color.Red;
			this.buttonZamknuti.Location = new System.Drawing.Point(442, 9);
			this.buttonZamknuti.Name = "buttonZamknuti";
			this.buttonZamknuti.Size = new System.Drawing.Size(20, 20);
			this.buttonZamknuti.TabIndex = 22;
			this.buttonZamknuti.TabStop = false;
			this.buttonZamknuti.UseVisualStyleBackColor = false;
			this.buttonZamknuti.Click += new System.EventHandler(this.ButtonZamknutiClick);
			// 
			// buttonActionW
			// 
			this.buttonActionW.Location = new System.Drawing.Point(43, 6);
			this.buttonActionW.Name = "buttonActionW";
			this.buttonActionW.Size = new System.Drawing.Size(25, 23);
			this.buttonActionW.TabIndex = 23;
			this.buttonActionW.TabStop = false;
			this.buttonActionW.Text = "W";
			this.buttonActionW.UseVisualStyleBackColor = true;
			// 
			// labelWidth
			// 
			this.labelWidth.Location = new System.Drawing.Point(75, 5);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(38, 22);
			this.labelWidth.TabIndex = 24;
			this.labelWidth.Text = "Width:";
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxWidth
			// 
			this.textBoxWidth.Location = new System.Drawing.Point(119, 8);
			this.textBoxWidth.Name = "textBoxWidth";
			this.textBoxWidth.Size = new System.Drawing.Size(30, 20);
			this.textBoxWidth.TabIndex = 25;
			this.textBoxWidth.Enter += new System.EventHandler(this.textBox_Enter);
			this.textBoxWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
			// 
			// textBoxDepth
			// 
			this.textBoxDepth.Location = new System.Drawing.Point(199, 8);
			this.textBoxDepth.Name = "textBoxDepth";
			this.textBoxDepth.Size = new System.Drawing.Size(30, 20);
			this.textBoxDepth.TabIndex = 27;
			this.textBoxDepth.Enter += new System.EventHandler(this.textBox_Enter);
			this.textBoxDepth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
			// 
			// labelDepth
			// 
			this.labelDepth.Location = new System.Drawing.Point(155, 5);
			this.labelDepth.Name = "labelDepth";
			this.labelDepth.Size = new System.Drawing.Size(39, 22);
			this.labelDepth.TabIndex = 26;
			this.labelDepth.Text = "Depth:";
			this.labelDepth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxEdges
			// 
			this.textBoxEdges.Location = new System.Drawing.Point(279, 8);
			this.textBoxEdges.Name = "textBoxEdges";
			this.textBoxEdges.Size = new System.Drawing.Size(30, 20);
			this.textBoxEdges.TabIndex = 29;
			this.textBoxEdges.Enter += new System.EventHandler(this.textBox_Enter);
			this.textBoxEdges.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
			// 
			// labelEdges
			// 
			this.labelEdges.Location = new System.Drawing.Point(235, 5);
			this.labelEdges.Name = "labelEdges";
			this.labelEdges.Size = new System.Drawing.Size(40, 22);
			this.labelEdges.TabIndex = 28;
			this.labelEdges.Text = "Edges:";
			this.labelEdges.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkBoxIsolateAuto
			// 
			this.checkBoxIsolateAuto.Location = new System.Drawing.Point(315, 6);
			this.checkBoxIsolateAuto.Name = "checkBoxIsolateAuto";
			this.checkBoxIsolateAuto.Size = new System.Drawing.Size(119, 24);
			this.checkBoxIsolateAuto.TabIndex = 30;
			this.checkBoxIsolateAuto.TabStop = false;
			this.checkBoxIsolateAuto.Text = "Isolate -> AutoKeys";
			this.checkBoxIsolateAuto.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(474, 36);
			this.Controls.Add(this.checkBoxIsolateAuto);
			this.Controls.Add(this.textBoxEdges);
			this.Controls.Add(this.labelEdges);
			this.Controls.Add(this.textBoxDepth);
			this.Controls.Add(this.labelDepth);
			this.Controls.Add(this.textBoxWidth);
			this.Controls.Add(this.labelWidth);
			this.Controls.Add(this.buttonActionW);
			this.Controls.Add(this.buttonZamknuti);
			this.Controls.Add(this.buttonActionX);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(490, 75);
			this.Name = "MainForm";
			this.Text = "CatiaLubeGroove";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
