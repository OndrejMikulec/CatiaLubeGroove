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

namespace CatiaLubeGroove
{
	/// <summary>
	/// Description of ZigZagLube.
	/// </summary>
	public class ZigZagLube
	{
    	double[] p1;
    	double[] p2;
    	double[] p3;
    	double[] p4;
    	double[] p5;
    	double[] p6;
    	double[] p7;
    	double[] p8;
    	
    	double width;
    	double depth;
    	
		public ZigZagLube(myRectangle obl, double width, double depth)
		{
    		this.width = width;
    		this.depth = depth;
    		
    		if (obl.A<=obl.B) {
    			
    			double uhel = Math.Atan2(obl.B/3,obl.A);
	    		double cornerX = Math.Cos(Math.PI-Math.PI/2-uhel)*width;
	    		double cornerY = Math.Sin(Math.PI-Math.PI/2-uhel)*width;
	    		
	    		p1 = new double[] {obl.P1x+cornerX,					obl.P1y };
	    		p2 = new double[] {obl.P2x,							obl.P1y+obl.B/3};
				p3 = new double[] {obl.P1x+width/(Math.Sin(uhel)),	obl.P2y-obl.B/3};
	    		p4 = new double[] {obl.P2x,							obl.P2y-cornerY};
	    		p5 = new double[] {obl.P2x-cornerX,					obl.P2y};
	    		p6 = new double[] {obl.P1x,							obl.P2y-obl.B/3};
	    		p7 = new double[] {obl.P2x-width/(Math.Sin(uhel)),	obl.P1y+obl.B/3};
	    		p8 = new double[] {obl.P1x,							obl.P1y+cornerY}; 			
    		} else {
	    		
    			double uhel = Math.Atan2(obl.A/3,obl.B);
	    		double cornerX = Math.Sin(Math.PI-Math.PI/2-uhel)*width;
	    		double cornerY = Math.Cos(Math.PI-Math.PI/2-uhel)*width;
	    		
	    		p1 = new double[] {obl.P1x,					obl.P1y+cornerY };
				p2 = new double[] {obl.P1x+obl.A/3,			obl.P2y};
				p3 = new double[] {obl.P2x-obl.A/3,			obl.P1y+width/(Math.Sin(uhel))};
	    		p4 = new double[] {obl.P2x-cornerX,			obl.P2y};
	    		p5 = new double[] {obl.P2x,					obl.P2y-cornerY};
	    		p6 = new double[] {obl.P2x-obl.A/3,			obl.P1y};
	    		p7 = new double[] {obl.P1x+obl.A/3,			obl.P2y-width/(Math.Sin(uhel))};
	    		p8 = new double[] {obl.P1x+cornerX,			obl.P1y};
    		}
		}
		
    	public void toSketch(MECMOD.Factory2D oFactory2D)
    	{
         	MECMOD.Line2D oLine2D1 =  oFactory2D.CreateLine(p1[0],p1[1],p2[0],p2[1]);
            MECMOD.Line2D oLine2D2 =  oFactory2D.CreateLine(p2[0],p2[1],p3[0],p3[1]);
            MECMOD.Line2D oLine2D3 =  oFactory2D.CreateLine(p3[0],p3[1],p4[0],p4[1]);
            MECMOD.Line2D oLine2D4 =  oFactory2D.CreateLine(p4[0],p4[1],p5[0],p5[1]);
            MECMOD.Line2D oLine2D5 =  oFactory2D.CreateLine(p5[0],p5[1],p6[0],p6[1]);
            MECMOD.Line2D oLine2D6 =  oFactory2D.CreateLine(p6[0],p6[1],p7[0],p7[1]);
            MECMOD.Line2D oLine2D7 =  oFactory2D.CreateLine(p7[0],p7[1],p8[0],p8[1]);
            MECMOD.Line2D oLine2D8 =  oFactory2D.CreateLine(p8[0],p8[1],p1[0],p1[1]);
    		
    	}
    	
	    public double[] P1
	    {
	    	get {
	    		return p1;
	    	}
	    }
	    
	    public double[] P2
	    {
	    	get {
	    		return p2;
	    	}
	    }
	    public double[] P3
	    {
	    	get {
	    		return p3;
	    	}
	    }
	    public double[] P4
	    {
	    	get {
	    		return p4;
	    	}
	    }
	    public double[] P5
	    {
	    	get {
	    		return p5;
	    	}
	    }
	    public double[] P6
	    {
	    	get {
	    		return p6;
	    	}
	    }
	    public double[] P7
	    {
	    	get {
	    		return p7;
	    	}
	    }
	    public double[] P8
	    {
	    	get {
	    		return p8;
	    	}
	    }
	    public double Depth
	    {
	    	get {
	    		return depth;
	    	}
	    }
	    public double Width
	    {
	    	get {
	    		return width;
	    	}
	    }
	}
}
