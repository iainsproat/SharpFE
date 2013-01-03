namespace SharpFE
{
	using System;
	
	/// <summary>
	/// Indicates that this finite element is defined by material properties
	/// </summary>
	public interface IHasMaterial
	{
		IMaterial Material { get; }
	}
}
