/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 18/11/2015
 * Time: 06:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
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
