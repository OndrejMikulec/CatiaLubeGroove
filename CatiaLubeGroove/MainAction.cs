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
using System.Windows.Forms;

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
        
        static grooveType type;
        
        public enum grooveType {Cross,ZigZag}
        

        /// <param name="type">accepting params: {Cross, ZigZag}</param>
        public static void mainAction(grooveType type, double width, double depth, double edges, bool isolateKeyAuto, bool debugRastr,  bool debugInflated )
        {
        	//all try
        	try {
        	
        	MainAction.isolateKeyAuto = isolateKeyAuto;
        	MainAction.debugRastr = debugRastr;
        	MainAction.debugInflated = debugInflated;
        	
        	MainAction.width = width;
        	MainAction.depth = depth;
        	MainAction.edges = edges;
        	
        	MainAction.type = type;

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

          	if (selectedPlanarfaces.Count>=1) {
            	foreach (PlanarfaceWithReference cpl in selectedPlanarfaces) {
          			action(cpl); 
          		}
          	} else {
          		MainForm.myForm.Activate();
          		MessageBox.Show("First select one or more planar faces!");
          		return;
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
            List<INFITF.AnyObject> mecmodlineForRemoveList = new List<INFITF.AnyObject>();
            
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
                		//NOT fully solved! Just gets endpoints and connets them by a line.
                        if (geo.GeometricType == MECMOD.CatGeometricType.catGeoTypeCircle2D) {
                            MECMOD.Circle2D oCircle = (MECMOD.Circle2D)geo;
                            isolate = true;
                            object[] oPointArray = new object[4];
                            oCircle.GetEndPoints(oPointArray);
                            linesListObjects.Add(oPointArray);
                            mecmodlineForRemoveList.Add(oCircle);
                        }
                		//NOT fully solved! Just gets endpoints and connets them by a line.
                        if (geo.GeometricType == MECMOD.CatGeometricType.catGeoTypeEllipse2D) {
                            MECMOD.Ellipse2D oElipse = (MECMOD.Ellipse2D)geo;
                            isolate = true;
                            object[] oPointArray = new object[4];
                            oElipse.GetEndPoints(oPointArray);
                            linesListObjects.Add(oPointArray);
                            mecmodlineForRemoveList.Add(oElipse);
                        }
                		//NOT fully solved! Just gets endpoints and connets them by a line.
                        if (geo.GeometricType == MECMOD.CatGeometricType.catGeoTypeHyperbola2D) {
                            MECMOD.Hyperbola2D oHyp = (MECMOD.Hyperbola2D)geo;
                            isolate = true;
                            object[] oPointArray = new object[4];
                            oHyp.GetEndPoints(oPointArray);
                            linesListObjects.Add(oPointArray);
                            mecmodlineForRemoveList.Add(oHyp);
                        }
                		//NOT fully solved! Just gets endpoints and connets them by a line.
                        if (geo.GeometricType == MECMOD.CatGeometricType.catGeoTypeParabola2D) {
                            MECMOD.Parabola2D oParab = (MECMOD.Parabola2D)geo;
                            isolate = true;
                            object[] oPointArray = new object[4];
                            oParab.GetEndPoints(oPointArray);
                            linesListObjects.Add(oPointArray);
                            mecmodlineForRemoveList.Add(oParab);
                        }
                		//NOT fully solved! Just gets endpoints and connets them by a line.
                        if (geo.GeometricType == MECMOD.CatGeometricType.catGeoTypeSpline2D) {
                            MECMOD.Spline2D oSpline = (MECMOD.Spline2D)geo;
                            isolate = true;
                            object[] oPointArray = new object[4];
                            oSpline.GetEndPoints(oPointArray);
                            linesListObjects.Add(oPointArray);
                            mecmodlineForRemoveList.Add(oSpline);
                        }
                		//?????????????????????????????
                        if (geo.GeometricType == MECMOD.CatGeometricType.catGeoTypeUnknown) {
                            MECMOD.Curve2D o = (MECMOD.Curve2D)geo;
                            isolate = true;
                            object[] oPointArray = new object[4];
                            o.GetEndPoints(oPointArray);
                            linesListObjects.Add(oPointArray);
                            mecmodlineForRemoveList.Add(o);
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
           foreach (INFITF.AnyObject l in mecmodlineForRemoveList) {
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
            
            double rastrX = (Math.Abs(maxX-minX))/delitel;
            if (rastrX<0.1) {
                rastrX = 0.1;
            }
            
            double rastrY = (Math.Abs(maxY-minY))/delitel;
            if (rastrY<0.1) {
                rastrY = 0.1;
            }
            
            List<myObdelnik> allObdelnikInThisLimit = new List<myObdelnik>();
            
            double rastrXvalue = minX;
            double rastrYvalue = minY;
            while (rastrYvalue<maxY) {
                while (rastrXvalue<maxX) {
                    myObdelnik rastrInsideLim = new myObdelnik(rastrXvalue,rastrYvalue,rastrXvalue+rastrX,rastrYvalue+rastrY);
                    allObdelnikInThisLimit.Add(rastrInsideLim);
                    rastrXvalue += rastrX;
                }
                rastrXvalue = minX;
                rastrYvalue += rastrY;
            }
 
            List<myObdelnik> allObdelnikInThisLimitNoZero = new List<myObdelnik>();
            foreach (myObdelnik obl in allObdelnikInThisLimit) {
                if (obl.obsah!=0) {
            		obl.resizeAllEdges(-Math.Min(rastrX,rastrY)*0.1);
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
            
            
                        
            double inflateX = rastrX/10;
            double inflateY = rastrY/10;
            double maxInflateAreaEdge = Math.Max(Math.Abs(maxX-minX)+rastrX,Math.Abs(maxY-minY)+rastrY);
            double maxInflateArea = maxInflateAreaEdge*maxInflateAreaEdge;
            List<myObdelnik> maxObdelnikListIflatedNoLeak = new List<myObdelnik>();
            int count = 1;
            foreach (myObdelnik obl in allObdelnikInThisLimitNoCross) {
                obl.resizeAllEdges(-Math.Min(rastrX,rastrY)*0.1);
                bool leaked = false;
                if (count==1 ) {
                    double initilaArea = obl.obsah;
                    leaked = SupportClass.inflationLoop(SupportClass.inflateDirection.B,obl,linesListDouble,inflateY,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.R,obl,linesListDouble,inflateX,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.T,obl,linesListDouble,inflateY,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.L,obl,linesListDouble,inflateX,initilaArea,maxInflateArea);
                }
                if (count==2 ) {
                    double initilaArea = obl.obsah;
                    leaked = SupportClass.inflationLoop(SupportClass.inflateDirection.T,obl,linesListDouble,inflateY,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.L,obl,linesListDouble,inflateX,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.B,obl,linesListDouble,inflateY,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.R,obl,linesListDouble,inflateX,initilaArea,maxInflateArea);
                }
                if (count==3 ) {
                    double initilaArea = obl.obsah;
                    leaked =  SupportClass.inflationLoop(SupportClass.inflateDirection.R,obl,linesListDouble,inflateX,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.T,obl,linesListDouble,inflateY,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.L,obl,linesListDouble,inflateX,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.B,obl,linesListDouble,inflateY,initilaArea,maxInflateArea);
                }
                if (count==4 ) {
                    double initilaArea = obl.obsah;
                    leaked =  SupportClass.inflationLoop(SupportClass.inflateDirection.L,obl,linesListDouble,inflateX,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.B,obl,linesListDouble,inflateY,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.R,obl,linesListDouble,inflateX,initilaArea,maxInflateArea)
                    || SupportClass.inflationLoop(SupportClass.inflateDirection.T,obl,linesListDouble,inflateY,initilaArea,maxInflateArea);
                }
                if (!leaked) {
                	if (type==grooveType.Cross&&Math.Min(obl.A,obl.B)>width*2) {
                		maxObdelnikListIflatedNoLeak.Add(obl);
                	}
                	if (type==grooveType.ZigZag&&Math.Min(obl.A,obl.B)>width*3) {
                		maxObdelnikListIflatedNoLeak.Add(obl);
                	}                	
                 }
                
                count++;
                if (count>4) {
                    count = 1;
                }
            }
            
            if (debugInflated) {
            	DebugCreateAll.createAll(maxObdelnikListIflatedNoLeak,oSketch,catiaInstance);
            }
            
            List<myObdelnik> maxObdelnikListIflatedNoLeakEdgesResized = new List<myObdelnik>();
            foreach (myObdelnik obl in maxObdelnikListIflatedNoLeak) {
            	obl.resizeAllEdges(-edges);
            	if (obl.obsah>0) {
            		maxObdelnikListIflatedNoLeakEdgesResized.Add(obl);
            	}
            }
   
            double finalP1x = 0;
            double finalP1y = 0;
            double finalP2x = 0;
            double finalP2y = 0;

            myObdelnik win = null;
            if (maxObdelnikListIflatedNoLeakEdgesResized.Count>0) {
             	
                win = SupportClass.optimalMaxAndABRatio(maxObdelnikListIflatedNoLeakEdgesResized);
                
                finalP1x = Math.Round( win.P1x,1);
                finalP1y =  Math.Round(win.P1y,1);
                finalP2x =  Math.Round(win.P2x,1);
                finalP2y =  Math.Round(win.P2y,1);
                
                myObdelnik finalObdelnik = new myObdelnik(finalP1x,finalP1y,finalP2x,finalP2y);
                crossLube finalcrossLube = new crossLube(finalObdelnik,width,depth);
                ZigZagLube finalZigZagLube = new ZigZagLube(finalObdelnik,width,depth);
           	            
	            oPart.Update();
	            oFactory2D = oSketch.OpenEdition();
             
	            if (type==grooveType.ZigZag) {
	            	finalZigZagLube.toSketch(oFactory2D);
	            } else {
	            	finalcrossLube.toSketch(oFactory2D);
	            }

	            oSketch.CloseEdition();
				oPart.Update();
				
				if (debugInflated||debugRastr) {
					return;
				}
				
				oPart.InWorkObject = oSketch;
				
				PARTITF.ShapeFactory oShapeFactory = (PARTITF.ShapeFactory)oPart.ShapeFactory;
				PARTITF.Pocket oNewPadPlus = oShapeFactory.AddNewPocket ( oSketch, finalcrossLube.Depth); 

				oPart.Update();
            } else {
            	MainForm.myForm.Activate();
            	MessageBox.Show(@"Groove for the face will not be created!
Area si too small.");
            }
            
            //all catch
          	}catch{}
        }
	}
}
