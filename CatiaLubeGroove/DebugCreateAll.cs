/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 17/11/2015
 * Time: 11:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace CatiaLubeGroove
{
	/// <summary>
	/// Description of DebugCreateAll.
	/// </summary>
	public static class DebugCreateAll
	{
		public static void createAll(List<myObdelnik> myObdelniksList, MECMOD.Sketch oSketch, INFITF.Application catiaInstance)
		{
			 MECMOD.Factory2D oFactory2D = oSketch.OpenEdition();
			 double count = 0;
 			foreach (myObdelnik obl in myObdelniksList) {
            
            	MECMOD.Line2D oLine2D1 =  oFactory2D.CreateLine(obl.P1x,obl.P1y,obl.P2x,obl.P1y);
            	MECMOD.Line2D oLine2D2 =  oFactory2D.CreateLine(obl.P2x,obl.P1y,obl.P2x,obl.P2y);
            	MECMOD.Line2D oLine2D3 =  oFactory2D.CreateLine(obl.P2x,obl.P2y,obl.P1x,obl.P2y);
            	MECMOD.Line2D oLine2D4 =  oFactory2D.CreateLine(obl.P1x,obl.P2y,obl.P1x,obl.P1y);
				
            	catiaInstance.set_StatusBar(Math.Round(count/myObdelniksList.Count*100) + "%");
            	
            	count++;
 			}
			 
			 oSketch.CloseEdition();
			
		}
	}
}
