namespace SharpFE
{
	using System;
	
	/// <summary>
	/// Description of GenericElasticMaterial.
	/// </summary>
	public class GenericElasticMaterial : ILinearElasticMaterial
	{
		public GenericElasticMaterial(double rho, double E, double nu)
		{
			this.Density = rho;
			this.YoungsModulus = E;
			this.PoissonsRatio = nu;
		}
		
		public double Density
		{
			get;
			private set;
		}
		
		public double YoungsModulus
		{
			get;
			private set;
		}
		
		public double PoissonsRatio
		{
			get;
			private set;
		}
	}
}
