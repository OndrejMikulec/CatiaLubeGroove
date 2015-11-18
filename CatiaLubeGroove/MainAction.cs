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
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace CatiaLubeGroove
{
	/// <summary>
	/// Description of MainAction.
	/// </summary>
	public static class MainAction
	{
		static bool debugRastr;
		static bool debugInflated;
		static bool isolateKeyAuto;
		
		static double width;
		static double depth;
		static double edges;
    	
		static INFITF.Application catiaInstance;
		static MECMOD.PartDocument oPartDocument;
		
        static List<double[]> pointsListDouble = new List<double[]>();
        static List<double[]> linesListDouble = new List<double[]>();
        
        public static void mainAction(double width, double depth, double edges, bool isolateKeyAuto, bool debugRastr,  bool debugInflated )
        {
        	//all try
        	try {
        	
        	MainAction.isolateKeyAuto = isolateKeyAuto;
        	MainAction.debugRastr = debugRastr;
        	MainAction.debugInflated = debugInflated;
        	
        	MainAction.width = width;
        	MainAction.depth = depth;
        	MainAction.edges = edges;

            try {
                catiaInstance = (INFITF.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Catia.Application");
            } catch {return;}
            oPartDocument  = (MECMOD.PartDocument)catiaInstance.ActiveDocument;
            
            

            MECMOD.PlanarFace oPlanarFace = null;
            INFITF.Reference oReference = null;
            
          	List<PlanarfaceWithReference> selectedPlanarfaces = new List<PlanarfaceWithReference>();

          	int selectionCount = Int32.Parse( oPartDocument.Selection.Count2.ToString());
        	if (selectionCount>=1) {
          		for (int i = 1; i <= selectionCount; i++) {
             		try {
	 	               oPlanarFace = (MECMOD.PlanarFace )oPartDocument.Selection.Item2(i).Value;
		               oReference = (INFITF.Reference)oPartDocument.Selection.Item2(i).Value;
		               selectedPlanarfaces.Add(new PlanarfaceWithReference( oPlanarFace,oReference));
            		} catch{  }           		
            	}
            }

            foreach (PlanarfaceWithReference cpl in selectedPlanarfaces) {
          		action(cpl);
          	}
          	
          	//all catch
        	} catch {}
        }
            	
          static void action(PlanarfaceWithReference cpl)
          {
          	//all try
          	try {
          	
          	pointsListDouble.Clear();
        	linesListDouble.Clear();
          	
          	oPartDocument.Selection.Clear();
          	oPartDocument.Selection.Add(cpl.OPlanarFace);
           	MECMOD.Body oBody = (MECMOD.Body)oPartDocument.Selection.FindObject("CATIAShape").Parent;
			MECMOD.Part oPart = (MECMOD.Part)oBody.Parent;
            
            
            MECMOD.Sketch oSketch = oBody.Sketches.Add(cpl.OReference);
            MECMOD.Factory2D oFactory2D = oSketch.OpenEdition();
            
            oFactory2D.CreateProjection(cpl.OReference);
            oSketch.CloseEdition();
            
            oPart.Update();
            
            oPartDocument.Selection.Clear();
            
            List<object[]> pointsListObjects = new List<object[]>();
            List<MECMOD.Point2D> mecmodPointsForRemoveList = new List<MECMOD.Point2D>();
            List<object[]> linesListObjects = new List<object[]>();
            List<MECMOD.Line2D> mecmodlineForRemoveList = new List<MECMOD.Line2D>();
            
           string sketchOriginName = oSketch.get_Name();
           oSketch.set_Name(oSketch.get_Name()+ "_Isolate_me_please.");
            
            oPart.Update();
            oPartDocument.Selection.Add(oSketch);
            
            if (isolateKeyAuto) {
            	SupportClass.isolateKeyAuto();
            }
    
            bool isolate = false;
            int timeout = 60;
            const int sleep = 1000;
            while (!isolate) {
            	catiaInstance.set_StatusBar("Isolate the sketch "+oSketch.get_Name() +" "+(timeout*1000)/sleep + "s timeout");
                Thread.Sleep(sleep);
                if (timeout<=0) {
                	catiaInstance.set_StatusBar("Macro timeout");
                	oPartDocument.Selection.Clear();
                	oPartDocument.Selection.Add(oSketch);
                	oPartDocument.Selection.Delete();
                	return;
                }
                timeout--;
                try {
                    foreach (MECMOD.GeometricElement geo in oSketch.GeometricElements) {
                        if (geo.GeometricType == MECMOD.CatGeometricType.catGeoTypePoint2D) {
                            MECMOD.Point2D oPoint = (MECMOD.Point2D)geo;
                            isolate = true;
                            object[] oPointArray = new object[2];
                            oPoint.GetCoordinates(oPointArray);
                            pointsListObjects.Add(oPointArray);
                            mecmodPointsForRemoveList.Add(oPoint);
                        }
                        if (geo.GeometricType == MECMOD.CatGeometricType.catGeoTypeLine2D) {
                            MECMOD.Line2D oLine = (MECMOD.Line2D)geo;
                            isolate = true;
                            object[] oPointArray = new object[4];
                            oLine.GetEndPoints(oPointArray);
                            linesListObjects.Add(oPointArray);
                            mecmodlineForRemoveList.Add(oLine);
                        }
                    }
                } catch {    }
            }
            

            oSketch.set_Name(sketchOriginName);
            oPart.Update();
            
            
            oFactory2D = oSketch.OpenEdition();
            oPartDocument.Selection.Clear();
            foreach (MECMOD.Point2D p in mecmodPointsForRemoveList) {
            	oPartDocument.Selection.Add(p);
            }
           foreach (MECMOD.Line2D l in mecmodlineForRemoveList) {
            	oPartDocument.Selection.Add(l);
            }
            oPartDocument.Selection.Delete();
            oSketch.CloseEdition();
            
            oPart.Update();
            

            foreach (object[] point in pointsListObjects) {
                pointsListDouble.Add(new double[] {Double.Parse(point[0].ToString()),Double.Parse(point[1].ToString())});
            }
            
            foreach (object[] line in linesListObjects) {
                linesListDouble.Add(new double[] {Double.Parse(line[0].ToString()),Double.Parse(line[1].ToString()),Double.Parse(line[2].ToString()),Double.Parse(line[3].ToString())});
            }
            
            
            double minX = pointsListDouble.Min(setting => setting[0]);
            double maxX = pointsListDouble.Max(setting => setting[0]);
            double minY = pointsListDouble.Min(setting => setting[1]);
            double maxY = pointsListDouble.Max(setting => setting[1]);
            const double delitel = 20;
            
            double rastr = (Math.Min(Math.Abs(maxX-minX),(Math.Abs(maxY-minY))))/delitel;
            if (rastr<0.1) {
                rastr = 0.1;
            }
            
            List<myObdelnik> allObdelnikInThisLimit = new List<myObdelnik>();
            
            double rastrX = minX;
            double rastrY = minY;
            while (rastrY<maxY) {
                while (rastrX<maxX) {
                    myObdelnik rastrInsideLim = new myObdelnik(rastrX,rastrY,rastrX+rastr,rastrY+rastr);
                    allObdelnikInThisLimit.Add(rastrInsideLim);
                    rastrX += rastr;
                }
                rastrX = minX;
                rastrY += rastr;
            }
            


            List<myObdelnik> allObdelnikInThisLimitNoZero = new List<myObdelnik>();
            foreach (myObdelnik obl in allObdelnikInThisLimit) {
                if (obl.obsah!=0) {
                    obl.resizeAllEdges(-rastr*0.1);
                    allObdelnikInThisLimitNoZero.Add(obl);
                }
            }            

            List<myObdelnik> allObdelnikInThisLimitNoCross = new List<myObdelnik>();
            foreach (myObdelnik obl2 in allObdelnikInThisLimitNoZero) {
                if (!obl2.anyLineFromListCrossObdelnik(linesListDouble)) {
                    allObdelnikInThisLimitNoCross.Add(obl2);
                }
            }  

            
            if (debugRastr) {
            	DebugCreateAll.createAll(allObdelnikInThisLimitNoCross,oSketch,catiaInstance);
            }            

            
            double inflate = rastr/10;
            double maxInflateArea = Math.Max(Math.Abs(maxX-minX),(Math.Abs(maxY-minY))+rastr)*Math.Max(Math.Abs(maxX-minX),(Math.Abs(maxY-minY))+rastr);
            List<myObdelnik> maxObdelnikListIflatedNoLeak = new List<myObdelnik>();
            int count = 1;
            foreach (myObdelnik obl in allObdelnikInThisLimitNoCross) {
                obl.resizeAllEdges(-rastr*0.1);
                bool leaked = false;
                if (count==1 ) {
                    double initilaArea = obl.obsah;
                    leaked = SupportClass.inflationLoop("B",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("R",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("T",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("L",obl,linesListDouble,inflate,initilaArea,maxInflateArea);
                }
                if (count==2 ) {
                    double initilaArea = obl.obsah;
                    leaked = SupportClass.inflationLoop("T",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("L",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("B",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("R",obl,linesListDouble,inflate,initilaArea,maxInflateArea);
                }
                if (count==3 ) {
                    double initilaArea = obl.obsah;
                    leaked =  SupportClass.inflationLoop("R",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("T",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("L",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("B",obl,linesListDouble,inflate,initilaArea,maxInflateArea);
                }
                if (count==4 ) {
                    double initilaArea = obl.obsah;
                    leaked =  SupportClass.inflationLoop("L",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("B",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("R",obl,linesListDouble,inflate,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop("T",obl,linesListDouble,inflate,initilaArea,maxInflateArea);
                }
                if (!leaked) {
                    maxObdelnikListIflatedNoLeak.Add(obl);
                }
                
                count++;
                if (count>4) {
                    count = 1;
                }
            }
            
            
            if (debugInflated) {
            	DebugCreateAll.createAll(maxObdelnikListIflatedNoLeak,oSketch,catiaInstance);
            }
                
 
            double finalP1x = 0;
            double finalP1y = 0;
            double finalP2x = 0;
            double finalP2y = 0;

            myObdelnik win = null;
            if (maxObdelnikListIflatedNoLeak.Count>0) {
                win = SupportClass.optimalMaxAndABRatio(maxObdelnikListIflatedNoLeak);
                win.resizeAllEdges(-edges);
                finalP1x = Math.Round( win.P1x,1);
                finalP1y =  Math.Round(win.P1y,1);
                finalP2x =  Math.Round(win.P2x,1);
                finalP2y =  Math.Round(win.P2y,1);
                
                myObdelnik finalObdelnik = new myObdelnik(finalP1x,finalP1y,finalP2x,finalP2y);
                crossLube finalcrossLube = new crossLube(finalObdelnik,width,depth);
           	            
	            oPart.Update();
	            
	            oFactory2D = oSketch.OpenEdition();
             
            
	            MECMOD.Line2D oLine2D1 =  oFactory2D.CreateLine(finalcrossLube.P1[0],finalcrossLube.P1[1],finalcrossLube.P2[0],finalcrossLube.P2[1]);
	            MECMOD.Line2D oLine2D2 =  oFactory2D.CreateLine(finalcrossLube.P2[0],finalcrossLube.P2[1],finalcrossLube.P3[0],finalcrossLube.P3[1]);
	            MECMOD.Line2D oLine2D3 =  oFactory2D.CreateLine(finalcrossLube.P3[0],finalcrossLube.P3[1],finalcrossLube.P4[0],finalcrossLube.P4[1]);
	            MECMOD.Line2D oLine2D4 =  oFactory2D.CreateLine(finalcrossLube.P4[0],finalcrossLube.P4[1],finalcrossLube.P5[0],finalcrossLube.P5[1]);
	            MECMOD.Line2D oLine2D5 =  oFactory2D.CreateLine(finalcrossLube.P5[0],finalcrossLube.P5[1],finalcrossLube.P6[0],finalcrossLube.P6[1]);
	            MECMOD.Line2D oLine2D6 =  oFactory2D.CreateLine(finalcrossLube.P6[0],finalcrossLube.P6[1],finalcrossLube.P7[0],finalcrossLube.P7[1]);
	            MECMOD.Line2D oLine2D7 =  oFactory2D.CreateLine(finalcrossLube.P7[0],finalcrossLube.P7[1],finalcrossLube.P8[0],finalcrossLube.P8[1]);
	            MECMOD.Line2D oLine2D8 =  oFactory2D.CreateLine(finalcrossLube.P8[0],finalcrossLube.P8[1],finalcrossLube.P9[0],finalcrossLube.P9[1]);
	            MECMOD.Line2D oLine2D9 =  oFactory2D.CreateLine(finalcrossLube.P9[0],finalcrossLube.P9[1],finalcrossLube.P10[0],finalcrossLube.P10[1]);
	            MECMOD.Line2D oLine2D10 =  oFactory2D.CreateLine(finalcrossLube.P10[0],finalcrossLube.P10[1],finalcrossLube.P11[0],finalcrossLube.P11[1]);
	            MECMOD.Line2D oLine2D11 =  oFactory2D.CreateLine(finalcrossLube.P11[0],finalcrossLube.P11[1],finalcrossLube.P12[0],finalcrossLube.P12[1]);
	            MECMOD.Line2D oLine2D12 =  oFactory2D.CreateLine(finalcrossLube.P12[0],finalcrossLube.P12[1],finalcrossLube.P1[0],finalcrossLube.P1[1]);
	            
	            oSketch.CloseEdition();
				oPart.Update();
				
				if (debugInflated||debugRastr) {
					return;
				}
				
				oPart.InWorkObject = oSketch;
				
				PARTITF.ShapeFactory oShapeFactory = (PARTITF.ShapeFactory)oPart.ShapeFactory;
				PARTITF.Pocket oNewPadPlus = oShapeFactory.AddNewPocket ( oSketch, finalcrossLube.Depth); 


					oPart.Update();

				
            } 
            
            //all catch
          	}catch{}
        }
	}
}
