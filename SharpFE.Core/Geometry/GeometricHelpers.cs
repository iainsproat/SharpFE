//-----------------------------------------------------------------------
// <copyright file="Geometry.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra.Generic;

    /// <summary>
    /// Contains various methods for calculating geometric properties
    /// </summary>
    public static class GeometricHelpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pointOnLine"></param>
        /// <param name="vectorDefiningLine"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "OnLine", Justification = "Not referring to Online, i.e. internet, but a geometric description")]
        public static Vector VectorBetweenPointAndLine(IFiniteElementNode point, IFiniteElementNode pointOnLine, Vector vectorDefiningLine)
        {
            return GeometricHelpers.VectorBetweenPointAndLine(GeometricHelpers.NodeToVector(point), GeometricHelpers.NodeToVector(pointOnLine), vectorDefiningLine);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pointOnLine"></param>
        /// <param name="vectorDefiningLine"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "OnLine", Justification = "Not referring to Online, i.e. internet, but a geometric description")]
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node0"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public static double AreaTriangle(IFiniteElementNode node0, IFiniteElementNode node1, IFiniteElementNode node2)
        {
            return GeometricHelpers.AreaTriangle(GeometricHelpers.NodeToVector(node0), GeometricHelpers.NodeToVector(node1), GeometricHelpers.NodeToVector(node2));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double AreaTriangle(Vector point0, Vector point1, Vector point2)
        {
            Vector side01 = (Vector)point1.Subtract(point0);
            Vector side02 = (Vector)point2.Subtract(point0);
            
            Vector crossProduct = side01.CrossProduct(side02);
            double quadArea = crossProduct.Norm(2);
            return quadArea / 2.0;
        }
        
        /// <summary>
        /// Nodes to be specified in clockwise order
        /// </summary>
        /// <param name="node0"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="node3"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1025:ReplaceRepetitiveArgumentsWithParamsArray", Justification = "Requires exactly four points, so an adjustable params array is not necessary")]
        public static double AreaQuadrilateral(IFiniteElementNode node0, IFiniteElementNode node1, IFiniteElementNode node2, IFiniteElementNode node3)
        {
            return GeometricHelpers.AreaQuadrilateral(GeometricHelpers.NodeToVector(node0), GeometricHelpers.NodeToVector(node1), GeometricHelpers.NodeToVector(node2), GeometricHelpers.NodeToVector(node3));
        }
        
        /// <summary>
        /// Vectors to be provided in clockwise order
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1025:ReplaceRepetitiveArgumentsWithParamsArray", Justification = "Can only have four points, so params array is useless")]
        public static double AreaQuadrilateral(Vector point0, Vector point1, Vector point2, Vector point3)
        {
            Vector diagonal1 = (Vector)point2.Subtract(point0);
            Vector diagonal2 = (Vector)point3.Subtract(point1);
            
            double crossProduct = (diagonal1[0] * diagonal2[1]) - (diagonal1[1] * diagonal2[0]);
            return 0.5 * crossProduct;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static Vector NodeToVector(IFiniteElementNode node)
        {
            Vector result = new DenseVector(3);
            result[0] = node.X;
            result[1] = node.Y;
            result[2] = node.Z;
            return result;
        }
    }
}
