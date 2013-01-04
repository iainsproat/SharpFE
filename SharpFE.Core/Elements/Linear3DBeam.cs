

namespace SharpFE
{
	using System;
	using SharpFE.Elements;
	using SharpFE.Stiffness;
	
	/// <summary>
	/// Beam which carries moment and shear force in both major and minor axes as well as axial force and torsion.
	/// </summary>
	public class Linear3DBeam : FiniteElement1D, IHasConstantCrossSection, IHasMaterial
	{
		
		public Linear3DBeam(FiniteElementNode start, FiniteElementNode end, IMaterial mat, ICrossSection section)
			:base(new Linear3DBeamStiffnessMatrixBuilder(), start, end)
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
				case DegreeOfFreedom.X:  //axial
				case DegreeOfFreedom.Y:  //minor-axis shear
				case DegreeOfFreedom.Z:  //major-axis shear
				case DegreeOfFreedom.XX: //torsion
				case DegreeOfFreedom.YY: //major-axis moment
				case DegreeOfFreedom.ZZ: //minor-axis moment
					return true;
				default:
					return false;
			}
		}
	}
}
