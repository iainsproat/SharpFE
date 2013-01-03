namespace SharpFE
{
	using System;

	using SharpFE.Elements;
	using SharpFE.Stiffness;

	/// <summary>
	/// Linear element which has constant cross section
	/// and constant material properties
	/// </summary>
	public class LinearTrussElement : FiniteElement1D, IHasMaterial, IHasConstantCrossSection
	{
		public LinearTrussElement(FiniteElementNode start, FiniteElementNode end, IMaterial material, ICrossSection crossSection)
			:base(new Linear1DElasticMaterialCrossSectionStiffnessBuilder(), start, end)
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
	}
}
