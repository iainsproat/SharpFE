namespace SharpFE
{
	using System;

	using SharpFE.Elements;
	using SharpFE.Stiffness;

	/// <summary>
	/// Linear element which has constant cross section
	/// and constant material properties.
	/// Also known as a Rod element.
	/// </summary>
	public class LinearTruss : FiniteElement1D, IHasMaterial, IHasConstantCrossSection
	{
		public LinearTruss(FiniteElementNode start, FiniteElementNode end, IMaterial material, ICrossSection crossSection)
			:base(start, end)
		{
			this.CrossSection = crossSection;
			this.Material = material;
		}
		
		public ICrossSection CrossSection
		{
			get;
			private set;
		}
		
		public IMaterial Material
		{
			get;
			private set;
		}
		
		public override bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom)
		{
			switch(degreeOfFreedom)
			{
				case DegreeOfFreedom.X:
					return true;
				case DegreeOfFreedom.Y:
				case DegreeOfFreedom.Z:
				case DegreeOfFreedom.XX:
				case DegreeOfFreedom.YY:
				case DegreeOfFreedom.ZZ:
				default:
					return false;
			}
		}
	}
}
