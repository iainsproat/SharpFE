using System;
using SharpFE.Stiffness;

namespace SharpFE.Elements
{
	/// <summary>
	/// This is a linear 1D finite element.
	/// It only calculates axial loading, and major axis bending moment and major-axis shear force.
	/// </summary>
	public class LinearMajorAxisBeam : FiniteElement1D
	{
		public LinearMajorAxisBeam(FiniteElementNode start, FiniteElementNode end, IMaterial mat, ICrossSection section)
			:base(new LinearMajorAxisBeamStiffnessMatrixBuilder(), start, end)
		{
			Guard.AgainstNullArgument(mat, "mat");
			Guard.AgainstNullArgument(section, "section");
			
			this.Material = mat;
			this.CrossSection = section;
		}
		
		public IMaterial Material
		{
			get;
			private set;
		}
		
		public ICrossSection CrossSection
		{
			get;
			private set;
		}
		
		
		
		public override bool IsASupportedLocalStiffnessDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom)
		{
			switch(degreeOfFreedom)
			{
				case DegreeOfFreedom.X:
				case DegreeOfFreedom.Z: //major-axis shear
				case DegreeOfFreedom.YY: //major-axis moment
					return true;
				case DegreeOfFreedom.Y:  //minor-axis shear
				case DegreeOfFreedom.XX: //torsion
				case DegreeOfFreedom.ZZ: //minor-axis
				default:
					return false;
			}
		}
	}
}
