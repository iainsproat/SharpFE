

namespace SharpFE
{
	using System;
	
	/// <summary>
	/// A material
	/// </summary>
	public interface IMaterial
	{
		double Density { get; }
		double YoungsModulus { get; }
		double PoissonsRatio { get; }
		double ShearModulusElasticity { get; }
	}
}
