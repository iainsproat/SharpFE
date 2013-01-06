//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace SharpFE
{
	/// <summary>
	/// Description of Geometry.
	/// </summary>
	public class Geometry
	{
		public static Vector VectorBetweenPointAndLine(Vector point, Vector pointOnLine, Vector vectorDefiningLine)
		{
			Vector<double> betweenPoints = point.Subtract(pointOnLine);
			Vector<double> lineVector = vectorDefiningLine.Normalize(2);
			double projectionDistanceOfEndPointAlongSide1 = betweenPoints.DotProduct(lineVector);
			
			Vector<double> vectorFromNode1 = lineVector.Multiply(projectionDistanceOfEndPointAlongSide1);
			Vector<double> endPointOfPerpendicularLine = pointOnLine.Add(vectorFromNode1);
			
			Vector<double> result = endPointOfPerpendicularLine.Subtract(point);
			return (Vector)result;
		}
		
		public static double AreaTriangle(FiniteElementNode node0, FiniteElementNode node1, FiniteElementNode node2)
		{
			return Geometry.AreaTriangle(node0.AsVector(), node1.AsVector(), node2.AsVector());
		}
		
		public static double AreaTriangle(Vector point0, Vector point1, Vector point2)
		{
			Vector side01 = (Vector)point1.Subtract(point0);
			Vector side02 = (Vector)point2.Subtract(point0);
			
			Vector crossProduct = side01.CrossProduct(side02);
			double quadArea = crossProduct.Norm(2);
			return quadArea / 2.0;
		}
	}
}
