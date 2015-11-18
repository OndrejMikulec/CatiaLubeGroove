/*
 * Created by SharpDevelop.
 * 
The MIT License (MIT)

Copyright (c) 2015 Ondrej Mikulec
o.mikulec@seznam.cz
Vsetin, Czech Republic

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CatiaLubeGroove
{
	
	public static class SupportClass
	{
		[System.Runtime.InteropServices.DllImport ("User32.dll")]
		static extern int SetForegroundWindow(IntPtr point);

        public static myObdelnik optimalMaxAndABRatio(List<myObdelnik> inputList)
        {
            inputList = inputList.OrderBy(o=>o.obsah).ToList();
            inputList.Reverse();
            
            myObdelnik winner = inputList[0];
            
            foreach (myObdelnik obl in inputList) {
            	if (obl!=winner && obl.obsah>=winner.obsah*0.75 && Math.Max(obl.A,obl.B)*0.5>Math.Max(winner.A,winner.B) && obl.ABRatio>0.1) {
                    winner = obl;
                }
            }
            
            return winner;
        }
        
        public static bool inflationLoop(string direction, myObdelnik obl, List<double[]> linesListDouble, double inflate, double initilaArea,double maxInflateArea)
        {
            if (direction=="R") {
                while (!obl.inflateRightWillCross(linesListDouble,inflate)) {
                    obl.inflateRigth(inflate);
                    if (obl.obsah>maxInflateArea) {
                        return true;
                    }
                }                
            }
            if (direction=="L") {
                while (!obl.inflateLeftWillCross(linesListDouble,inflate)) {
                    obl.inflateLeft(inflate);
                    if (obl.obsah>maxInflateArea) {
                        return true;
                    }
                }                
            }
            if (direction=="B") {
                while (!obl.inflateBottomWillCross(linesListDouble,inflate)) {
                    obl.inflateBottom(inflate);
                    if (obl.obsah>maxInflateArea) {
                        return true;
                    }
                }                
            }
            if (direction=="T") {
                while (!obl.inflateTopWillCross(linesListDouble,inflate)) {
                    obl.inflateTop(inflate);
                    if (obl.obsah>maxInflateArea) {
                        return true;
                    }
                }                
            }            
            
            return false;
        }
        
        public static void isolateKeyAuto()
        {
        	 System.Diagnostics.Process ppp = System.Diagnostics.Process.GetProcessesByName("CNEXT").FirstOrDefault();
			if( ppp != null)
			{
			    IntPtr h = ppp.MainWindowHandle;
			    SetForegroundWindow(h);
			    Thread.Sleep(500);
			    System.Windows.Forms.SendKeys.SendWait("%E");
			    

			    	SetForegroundWindow(h);
			    	Thread.Sleep(100);
                	System.Windows.Forms.SendKeys.SendWait("{UP} ");			    	

        	 	SetForegroundWindow(h);
        	 	Thread.Sleep(500);
        	 	System.Windows.Forms.SendKeys.SendWait("{RIGHT} ");
        	 	SetForegroundWindow(h);
        	 	Thread.Sleep(500);
        	 	System.Windows.Forms.SendKeys.SendWait("{I} ");
			}  	
        }

	}
}
