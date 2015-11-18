/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 17/11/2015
 * Time: 17:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace CatiaLubeGroove
{
	/// <summary>
	/// Description of PlanarfaceWithReference.
	/// </summary>
	public class PlanarfaceWithReference
	{
		MECMOD.PlanarFace oPlanarFace;
		INFITF.Reference oReference;
        
		public PlanarfaceWithReference(MECMOD.PlanarFace oPlanarFace,INFITF.Reference oReference)
		{
			this.oPlanarFace = oPlanarFace;
			this.oReference = oReference;
		}
		
		public MECMOD.PlanarFace OPlanarFace
		{
			get {
				return oPlanarFace;
			}
		}
		
		public INFITF.Reference OReference
		{
			get {
				return oReference;
			}
		}
	}
}
