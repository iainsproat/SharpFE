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
			:base(new MajorAxisBarStiffnessMatrixBuilder(), start, end)
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
	}
}
