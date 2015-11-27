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
	/// Description of crossLube.
	/// </summary>
    public class crossLube
    {
    	double[] p1;
    	double[] p2;
    	double[] p3;
    	double[] p4;
    	double[] p5;
    	double[] p6;
    	double[] p7;
    	double[] p8;
    	double[] p9;
    	double[] p10;
    	double[] p11;
    	double[] p12;
    	
    	double width;
    	double depth;

    	
    	
    	public crossLube(myRectangle obl, double width, double depth)
    	{
    		double cornerX = Math.Cos(Math.PI-Math.PI/2-Math.Atan2(obl.B,obl.A))*width;
    		double cornerY = Math.Sin(Math.PI-Math.PI/2-Math.Atan2(obl.B,obl.A))*width;
    		
    		p1 = new double[] {obl.P1x+cornerX,			obl.P1y };
    		p2 = null;
    		p3 = new double[] {obl.P2x-cornerX,			obl.P1y};
    		p4 = new double[] {obl.P2x,					obl.P1y+cornerY};
    		p5 = null;
    		p6 = new double[] {obl.P2x,					obl.P2y-cornerY};
    		p7 = new double[] {obl.P2x-cornerX,			obl.P2y};
    		p8 = null;
    		p9 = new double[] {obl.P1x+cornerX,			obl.P2y  };
    		p10 = new double[] {obl.P1x,				obl.P2y-cornerY };
    		p11 = null;
    		p12 = new double[] {obl.P1x,				obl.P1y+cornerY};
    		
    		p2 = new double[] {intersection(p1,p6,p3,p10)[0],intersection(p1,p6,p3,p10)[1]};
    		p5 = new double[] {intersection(p4,p9,p1,p6)[0],intersection(p4,p9,p1,p6)[1]};
    		p8 = new double[] {intersection(p12,p7,p4,p9)[0],intersection(p12,p7,p4,p9)[1]};
    		p11 = new double[] {intersection(p12,p7,p3,p10)[0],intersection(p12,p7,p3,p10)[1]};
    		
    		this.width = width;
    		this.depth = depth;

    	}
    	
    	double[] intersection(double[] pp1, double[] pp2, double[] pp3, double[] pp4)
    	{
    		double[] l1 = new double[] {pp1[0],pp1[1],pp2[0],pp2[1]};
    		double[] l2 = new double[] {pp3[0],pp3[1],pp4[0],pp4[1]};
    		
            double projectionXL1 = l1[2]-l1[0];
            double projectionYL1 = l1[3]-l1[1];
            
            double a = projectionYL1/projectionXL1;
            double  c = l1[1]-a*l1[0];
            
            double projectionXL2 = l2[2]-l2[0];
            double projectionYL2 = l2[3]-l2[1];
            
            double b = projectionYL2/projectionXL2;
            double  d = l2[1]-b*l2[0];
            
            return new double[] {(d-c)/(a-b),(a*d-b*c)/(a-b)};
  
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
            MECMOD.Line2D oLine2D8 =  oFactory2D.CreateLine(p8[0],p8[1],p9[0],p9[1]);
            MECMOD.Line2D oLine2D9 =  oFactory2D.CreateLine(p9[0],p9[1],p10[0],p10[1]);
            MECMOD.Line2D oLine2D10 =  oFactory2D.CreateLine(p10[0],p10[1],p11[0],p11[1]);
            MECMOD.Line2D oLine2D11 =  oFactory2D.CreateLine(p11[0],p11[1],p12[0],p12[1]);
            MECMOD.Line2D oLine2D12 =  oFactory2D.CreateLine(p12[0],p12[1],p1[0],p1[1]);    		
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
	    public double[] P9
	    {
	    	get {
	    		return p9;
	    	}
	    }
	    public double[] P10
	    {
	    	get {
	    		return p10;
	    	}
	    }
	    public double[] P11
	    {
	    	get {
	    		return p11;
	    	}
	    }
	    public double[] P12
	    {
	    	get {
	    		return p12;
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
