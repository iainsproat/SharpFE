//-----------------------------------------------------------------------
// <copyright file="ModelTypeExtensions.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// This is an extension to the ModelType enum which provides useful information
	/// about the degrees of freedom each model type allows.
	/// </summary>
	public static class ModelTypeExtensions
	{
		/// <summary>
		/// The allowed degrees of freedom in which the model geometry can be created.
		/// </summary>
		/// <param name="modelType">The modeltype which dictates the allowed degrees of freedom</param>
		/// <returns>A list of allowed degrees of freedom for the geometry</returns>
		public static IList<DegreeOfFreedom> GetAllowedDegreesOfFreedomForGeometry(this ModelType modelType)
		{
			switch (modelType)
			{
				case ModelType.Truss1D:
					return new List<DegreeOfFreedom>(1)
					{
						DegreeOfFreedom.X
					};
				case ModelType.Beam1D:
					return new List<DegreeOfFreedom>(1)
					{
						DegreeOfFreedom.X
					};
				case ModelType.Truss2D:
					return new List<DegreeOfFreedom>(2)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Z
					};
				case ModelType.Frame2D:
					return new List<DegreeOfFreedom>(2)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Z,
					};
				case ModelType.Slab2D:
					return new List<DegreeOfFreedom>(2)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Y
					};
				case ModelType.Truss3D:
					return new List<DegreeOfFreedom>(3)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Y,
						DegreeOfFreedom.Z
					};
				case ModelType.MultiStorey2DSlab:
					return new List<DegreeOfFreedom>(3)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Y,
						DegreeOfFreedom.Z
					};
				case ModelType.Full3D:
					return new List<DegreeOfFreedom>(3)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Y,
						DegreeOfFreedom.Z
					};
				default:
					throw new NotImplementedException(string.Format(
						System.Globalization.CultureInfo.InvariantCulture,
						"Allowed Degrees Of Freedom have not been defined for a model type of {0}",
						modelType.ToString()));
			}
		}
		
		/// <summary>
		/// Indicates whether a degree of freedom is allowed for the creation of geometry by this model type.
		/// </summary>
		/// <param name="modelType">The <see cref="ModelType" /> which dictates the allowable degrees of freedom.</param>
		/// <param name="candidate">The degree of freedom to check as to whether it is allowed.</param>
		/// <returns>true if the degree of freedom is allowed for the creation of geometry for this model type.</returns>
		public static bool IsAllowedDegreeOfFreedomForGeometry(this ModelType modelType, DegreeOfFreedom candidate)
		{
			IList<DegreeOfFreedom> allowedDOF = modelType.GetAllowedDegreesOfFreedomForGeometry();
			return allowedDOF.Contains(candidate);
		}
		
		/// <summary>
		/// Indicates how many dimensions the geometry can be created in by this type of model type.
		/// </summary>
		/// <param name="modelType">The <see cref="ModelType" /> which dictates the geometrical dimensions.</param>
		/// <returns>The geometrical dimensionality (1D, 2D, 3D) allowed by this model type.</returns>
		public static GeometryDimensionality GetDimensions(this ModelType modelType)
		{
			switch (modelType)
			{
				case ModelType.Truss1D:
				case ModelType.Beam1D:
					return GeometryDimensionality.OneDimension;
				case ModelType.Truss2D:
				case ModelType.Frame2D:
				case ModelType.Slab2D:
					return GeometryDimensionality.TwoDimension;
				case ModelType.Truss3D:
				case ModelType.MultiStorey2DSlab:
				case ModelType.Full3D:
					return GeometryDimensionality.ThreeDimension;
				default:
					throw new NotImplementedException(string.Format(
						System.Globalization.CultureInfo.InvariantCulture,
						"GeometryDimensionality has not been defined for a model type of {0}",
						modelType.ToString()));
			}
		}
		
		/// <summary>
		/// The allowed degrees of freedom in which the applied forces and constraints can be created.
		/// </summary>
		/// <param name="modelType">The modeltype which dictates the allowed degrees of freedom</param>
		/// <returns>A list of allowed degrees of freedom for the forces and constraints</returns>
		public static IList<DegreeOfFreedom> GetAllowedDegreesOfFreedomForBoundaryConditions(this ModelType modelType)
		{
			switch (modelType)
			{
				case ModelType.Truss1D:
					return new List<DegreeOfFreedom>(1)
					{
						DegreeOfFreedom.X
					};
				case ModelType.Beam1D:
					return new List<DegreeOfFreedom>(3)
					{
						DegreeOfFreedom.Z,
						DegreeOfFreedom.YY
					};
				case ModelType.Truss2D:
					return new List<DegreeOfFreedom>(2)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Z
					};
				case ModelType.Frame2D:
					return new List<DegreeOfFreedom>(3)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Z,
						DegreeOfFreedom.YY
					};
				case ModelType.Slab2D:
					return new List<DegreeOfFreedom>(4)
					{
						DegreeOfFreedom.Z,
						DegreeOfFreedom.XX,
						DegreeOfFreedom.YY
					};
				case ModelType.Truss3D:
					return new List<DegreeOfFreedom>(3)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Y,
						DegreeOfFreedom.Z
					};
				case ModelType.MultiStorey2DSlab:
					return new List<DegreeOfFreedom>(3)
					{
						DegreeOfFreedom.Z,
						DegreeOfFreedom.XX,
						DegreeOfFreedom.YY
					};
				case ModelType.Full3D:
					return new List<DegreeOfFreedom>(6)
					{
						DegreeOfFreedom.X,
						DegreeOfFreedom.Y,
						DegreeOfFreedom.Z,
						DegreeOfFreedom.XX,
						DegreeOfFreedom.YY,
						DegreeOfFreedom.ZZ
					};
				default:
					throw new NotImplementedException(string.Format(
						System.Globalization.CultureInfo.InvariantCulture,
						"Allowed Degrees Of Freedom have not been defined for a model type of {0}",
						modelType.ToString()));
			}
		}
		
		/// <summary>
		/// Indicates whether a degree of freedom is allowed for the creation of boundary conditions by this model type.
		/// </summary>
		/// <param name="modelType">The <see cref="ModelType" /> which dictates the allowable degrees of freedom.</param>
		/// <param name="candidate">The degree of freedom to check as to whether it is allowed.</param>
		/// <returns>true if the degree of freedom is allowed for the creation of boundary conditions for this model type.</returns>
		public static bool IsAllowedDegreeOfFreedomForBoundaryConditions(this ModelType modelType, DegreeOfFreedom candidate)
		{
			IList<DegreeOfFreedom> allowedDOF = modelType.GetAllowedDegreesOfFreedomForBoundaryConditions();
			return allowedDOF.Contains(candidate);
		}
	}
}
