﻿using System;
using SharpFE.Elements;
using SharpFE.Stiffness;

namespace SharpFE
{
	/// <summary>
	/// This is a linear 1D finite element.
	/// It only calculates major axis bending moment and major-axis shear force.
	/// </summary>
	public class Linear1DBeam : LinearBeam
	{
		public Linear1DBeam(FiniteElementNode start, FiniteElementNode end, IMaterial mat, ICrossSection section)
			:base(start, end, mat, section)
		{
			// empty
		}
		
		public override bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom)
		{
			switch(degreeOfFreedom)
			{
				case DegreeOfFreedom.Z: //major-axis shear
				case DegreeOfFreedom.YY: //major-axis moment
					return true;
				case DegreeOfFreedom.X:  //axial force
				case DegreeOfFreedom.Y:  //minor-axis shear
				case DegreeOfFreedom.XX: //torsion
				case DegreeOfFreedom.ZZ: //minor-axis
				default:
					return false;
			}
		}
	}
}