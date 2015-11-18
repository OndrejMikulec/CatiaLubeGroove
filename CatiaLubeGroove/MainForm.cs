/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 18/11/2015
 * Time: 06:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;

namespace CatiaLubeGroove
{

	public partial class MainForm : Form
	{
       	static string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string directoryPath = System.IO.Path.GetDirectoryName(exePath);
        static string iniPath = directoryPath + @"\Setup.ini";

		
		public MainForm()
		{

			InitializeComponent();
			iniReadFormSetup();

		}
		void iniReadFormSetup()
		{
			try {			
				using (StreamReader readIni = new StreamReader(iniPath, Encoding.Default)) {

					readIni.ReadLine();
	        		int x = Int32.Parse(readIni.ReadLine());
	        		int y = Int32.Parse(readIni.ReadLine());
	        		
	        		Point oPoint = new Point(x,y);
	        		foreach (Screen scr in Screen.AllScreens) {
	        			if (scr.Bounds.Left< oPoint.X 
	        				&& (scr.Bounds.Right-Width) > oPoint.X
	        				&& scr.Bounds.Top < oPoint.Y
	        				&& (scr.Bounds.Bottom-Height) > oPoint.Y ){
		        			StartPosition = FormStartPosition.Manual;
	        				Location = oPoint;
	        			}
	    			}
	        		bool zamknuto = Boolean.Parse(readIni.ReadLine());
	        		if (zamknuto) {
		                TopMost = true;
		                buttonZamknuti.BackColor = Color.Red;
		            }
		            else
		            {
		                TopMost = false;
		                buttonZamknuti.BackColor = Color.Green;
		            }
					bool autoKeys = Boolean.Parse(readIni.ReadLine());
	        		if (autoKeys) {
						checkBoxIsolateAuto.Checked = true;
		            }
		            else
		            {
		                checkBoxIsolateAuto.Checked = false;
		            }
		            textBoxWidth.Text = Double.Parse(readIni.ReadLine()).ToString();
		            textBoxDepth.Text = Double.Parse(readIni.ReadLine()).ToString();
		            textBoxEdges.Text = Double.Parse(readIni.ReadLine()).ToString();
				}
			} catch {
				TopMost = true;
				checkBoxIsolateAuto.Checked = true;
	            textBoxWidth.Text = "1";
	            textBoxDepth.Text = "1";
	            textBoxEdges.Text = "5";
			}
		}
		void iniWriteSave()
		{
			try {
				using (StreamWriter writeIni = new StreamWriter(iniPath,false,Encoding.Default)) {
					writeIni.WriteLine("[SCREEN]");
		        	writeIni.WriteLine(Location.X);
		        	writeIni.WriteLine(Location.Y);
		        	writeIni.WriteLine(TopMost);
		        	writeIni.WriteLine("[VALUES]");
		        	writeIni.WriteLine(checkBoxIsolateAuto.Checked);
		        	writeIni.WriteLine(textBoxWidth.Text);
		        	writeIni.WriteLine(textBoxDepth.Text);
		        	writeIni.WriteLine(textBoxEdges.Text);
				}
			} catch {}				
		}
		void enableAll()
		{
        	foreach (Control con in Controls ) {
    			con.Enabled = true;	
			}			
		}
		
		void disableAll()
		{
        	foreach (Control con in Controls ) {
    			con.Enabled = false;	
			}			
		}
		
		void ButtonActionXClick(object sender, EventArgs e)
		{
			disableAll();
			MainAction.mainAction(Double.Parse(textBoxWidth.Text),Double.Parse(textBoxDepth.Text),Double.Parse(textBoxEdges.Text),checkBoxIsolateAuto.Checked,false,false);
			enableAll();
		}
		
		bool povolit_desetinouCarku;
        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            
            
            if (tb.Text.Contains(","))
            {
                povolit_desetinouCarku = false;
            }
            else
            {
                povolit_desetinouCarku = true;
            }
            
            List<Keys> globalnePovoleneKlavesy = new List<Keys>();
            globalnePovoleneKlavesy.Add(Keys.D0);
            globalnePovoleneKlavesy.Add(Keys.D1);
            globalnePovoleneKlavesy.Add(Keys.D2);
            globalnePovoleneKlavesy.Add(Keys.D3);
            globalnePovoleneKlavesy.Add(Keys.D4);
            globalnePovoleneKlavesy.Add(Keys.D5);
            globalnePovoleneKlavesy.Add(Keys.D6);
            globalnePovoleneKlavesy.Add(Keys.D7);
            globalnePovoleneKlavesy.Add(Keys.D8);
            globalnePovoleneKlavesy.Add(Keys.D9);
            globalnePovoleneKlavesy.Add(Keys.NumPad0);
            globalnePovoleneKlavesy.Add(Keys.NumPad1);
            globalnePovoleneKlavesy.Add(Keys.NumPad2);
            globalnePovoleneKlavesy.Add(Keys.NumPad3);
            globalnePovoleneKlavesy.Add(Keys.NumPad4);
            globalnePovoleneKlavesy.Add(Keys.NumPad5);
            globalnePovoleneKlavesy.Add(Keys.NumPad6);
            globalnePovoleneKlavesy.Add(Keys.NumPad7);
            globalnePovoleneKlavesy.Add(Keys.NumPad8);
            globalnePovoleneKlavesy.Add(Keys.NumPad9);
            globalnePovoleneKlavesy.Add(Keys.Left);
            globalnePovoleneKlavesy.Add(Keys.Right);
            globalnePovoleneKlavesy.Add(Keys.Back);
            globalnePovoleneKlavesy.Add(Keys.Add);
            globalnePovoleneKlavesy.Add(Keys.Subtract);            
            
            if (tb.SelectedText.Length>0 && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && globalnePovoleneKlavesy.Contains(e.KeyCode)) {
            	tb.Text = tb.Text.Replace(tb.SelectedText,"");
            }

            List<Keys> povoleneKlavesyProEditaci = new List<Keys>();
            povoleneKlavesyProEditaci.Add(Keys.Left);
            povoleneKlavesyProEditaci.Add(Keys.Right);
            povoleneKlavesyProEditaci.Add(Keys.Back);
            povoleneKlavesyProEditaci.Add(Keys.Add);
            povoleneKlavesyProEditaci.Add(Keys.Subtract);
            

            if (!povoleneKlavesyProEditaci.Contains(e.KeyCode)) {
                e.SuppressKeyPress = true;
            }


            if ((e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal) && povolit_desetinouCarku == true)
            {
                tb.AppendText(".");
            }
            
            if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0 ) {
            	tb.AppendText("0");
            }
            if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1 ) {
            	tb.AppendText("1");
            }
            if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2 ) {
            	tb.AppendText("2");
            }
            if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3 ) {
            	tb.AppendText("3");
            }
            if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4 ) {
            	tb.AppendText("4");
            }
            if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5 ) {
            	tb.AppendText("5");
            }
            if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6 ) {
            	tb.AppendText("6");
            }
            if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7 ) {
            	tb.AppendText("7");
            }
            if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8 ) {
            	tb.AppendText("8");
            }
            if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9 ) {
            	tb.AppendText("9");
            }
        }

        void textBox_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            povolit_desetinouCarku = true;
            tb.SelectAll();
        }
        
		void ButtonZamknutiClick(object sender, EventArgs e)
		{
            if (!TopMost)
            {
                TopMost = true;
                buttonZamknuti.BackColor = Color.Red;
            }
            else
            {
                TopMost = false;
                buttonZamknuti.BackColor = Color.Green;
            }	
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			iniWriteSave();
		}

	}
}
