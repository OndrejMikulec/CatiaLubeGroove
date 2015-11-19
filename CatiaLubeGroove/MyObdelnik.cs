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

namespace CatiaLubeGroove
{
	/// <summary>
	/// Description of MyObdelnik.
	/// </summary>
   public class myObdelnik
    {
        
        double p1x;
        double p1y;
        double p2x;
        double p2y;
        
        double a;
        double b;
        
        double[] line1;
        double[] line2;
        double[] line3;
        double[] line4;

        
        public myObdelnik(double p1x, double p1y, double p2x, double p2y)
        {
    
            this.p1x = p1x;
            this.p1y = p1y;
            this.p2x = p2x;
            this.p2y = p2y;
            
            refresh();

        }
        
        private void refresh()
        {
            a = p2x-p1x;
            b = p2y-p1y;
            
            if (a<0||b<0) {
                a = 0;
                b = 0;
            }
            
            line1 = new double[] {p1x,p1y,p2x,p1y};
            line2 = new double[] {p2x,p1y,p2x,p2y};
            line3 = new double[] {p2x,p2y,p1x,p2y};
            line4 = new double[] {p1x,p2y,p1x,p1y};            
        }
        
        public double P1x
        {
            get {
                return p1x;
            }

        }
        public double P1y
        {
            get {
                return p1y;
            }

        }
        public double P2x
        {
            get {
                return p2x;
            }

        }
        public double P2y
        {
            get {
                return p2y;
            }

        }

        public double obsah
        {
            get {
                return a*b;
            }
        }
        public double A
        {
            get {
                return a;
            }
        }
        public double B
        {
            get {
                return b;
            }
        }
        public double ABRatio
        {
            get {
                return Math.Min(a/b,b/a);
            }
        }

            
        public bool anyLineFromListCrossObdelnik(List<double[]> linesList)
        {
            foreach (double[] l in linesList) {
        
                double projectionXL = l[2]-l[0];
                double projectionYL = l[3]-l[1];
                if (
            		(
            		p1x<=Math.Min(l[0],l[2])&&p2x>=Math.Min(l[0],l[2])
                 	||p1x>=Math.Min(l[0],l[2])&&p2x<=Math.Max(l[0],l[2])
                  	||p1x<=Math.Max(l[0],l[2])&&p2x>=Math.Max(l[0],l[2])
                  	)&&(
            	 	p1y<=Math.Min(l[1],l[3])&&p2y>=Math.Min(l[1],l[3])
                    ||p1y>=Math.Min(l[1],l[3])&&p2y<=Math.Max(l[1],l[3])
            	 	||p1y<=Math.Max(l[1],l[3])&&p2y>=Math.Max(l[1],l[3])
        		 	)
    	 	   		) {
                    if (projectionXL!=0&&projectionYL!=0) {
                
                        double lineAngleL = projectionYL/projectionXL;
                        double  yInterceptL = l[1]-lineAngleL*l[0];
                          
                        //y=a*x+c 
                        double[] intersection1 = new double[] {(p1y-yInterceptL)/lineAngleL,p1y};
                        if (intersection1[0]>=p1x&&intersection1[0]<=p2x) {
                                return true;
                        }                    
                        double[] intersection2 = new double[] {p2x,(lineAngleL*p2x)+yInterceptL};
                        if (intersection2[1]>=p1y&&intersection2[1]<=p2y) {
                            return true;
                        }
                        double[] intersection3 = new double[] {(p2y-yInterceptL)/lineAngleL,p2y};
                        if (intersection3[0]>=p1x&&intersection3[0]<=p2x) {
                            return true;
                        }                    
                        double[] intersection4 = new double[] {p1x,(lineAngleL*p1x)+yInterceptL};
                        if (intersection4[1]>=p1y&&intersection4[1]<=p2y) {
                            return true;
                        }
                    }
                	
                   if (projectionXL==0||projectionYL==0) {
                        return true;
                    }
        		 }
        	}
            return false;
         }
        
        public bool inflateBottomWillCross(List<double[]> linesList,double x)
        {
            double orig = p1y;
            bool returningBool = false;
            p1y -= x;
            refresh();
            if (anyLineFromListCrossObdelnik(linesList)) {
                returningBool = true;
            }
            p1y = orig;
            refresh();
            return returningBool;
        }    
        public bool inflateRightWillCross(List<double[]> linesList,double x)
        {
            double orig = p2x;
            bool returningBool = false;
            p2x += x;
            refresh();
            if (anyLineFromListCrossObdelnik(linesList)) {
                returningBool = true;
            }
            p2x = orig;
            refresh();
            return returningBool;
        }    
        public bool inflateTopWillCross(List<double[]> linesList,double x)
        {
            double orig = p2y;
            bool returningBool = false;
            p2y += x;
            refresh();
            if (anyLineFromListCrossObdelnik(linesList)) {
                returningBool = true;
            }
            p2y = orig;
            refresh();
            return returningBool;
        }    
        public bool inflateLeftWillCross(List<double[]> linesList,double x)
        {
            double orig = p1x;
            bool returningBool = false;
            p1x -= x;
            refresh();
            if (anyLineFromListCrossObdelnik(linesList)) {
                returningBool = true;
            }
            p1x = orig;
            refresh();
            return returningBool;
        }            
    
        public void inflateBottom(double x)
        {
            p1y -= x;
            refresh();
        }
        public void inflateRigth(double x)
        {
            p2x += x;
            refresh();
        }
        public void inflateTop(double x)
        {
            p2y += x;
            refresh();
        }
        public void inflateLeft(double x)
        {
            p1x -= x;
            refresh();
        }
        
        public void resizeAllEdges(double x)
        {
            p1x -= x;
            p1y -= x;
            p2x += x;
            p2y += x;
            
            refresh();            
        }
        
        
    }
}
