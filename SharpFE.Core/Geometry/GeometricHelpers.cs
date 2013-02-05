//-----------------------------------------------------------------------
// <copyright file="Geometry.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Geometry
{
    using System;

    /// <summary>
    /// Contains various methods for calculating geometric properties
    /// </summary>
    public static class GeometricHelpers
    {

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node0"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public static double AreaTriangle(IFiniteElementNode node0, IFiniteElementNode node1, IFiniteElementNode node2)
        {
            return GeometricHelpers.AreaTriangle(node0.Location, node1.Location, node2.Location);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double AreaTriangle(Point point0, Point point1, Point point2)
        {
            GeometricVector side01 = point1.Subtract(point0);
            GeometricVector side02 = point2.Subtract(point0);
            
            GeometricVector crossProduct = side01.CrossProduct(side02);
            double quadArea = crossProduct.Norm(2);
            return quadArea * 0.5;
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
            return GeometricHelpers.AreaQuadrilateral(node0.Location, node1.Location, node2.Location, node3.Location);
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
        public static double AreaQuadrilateral(Point point0, Point point1, Point point2, Point point3)
        {
            GeometricVector diagonal1 = point2.Subtract(point0);
            GeometricVector diagonal2 = point3.Subtract(point1);
            
            double crossProduct = (diagonal1[DegreeOfFreedom.X] * diagonal2[DegreeOfFreedom.Y]) - (diagonal1[DegreeOfFreedom.Y] * diagonal2[DegreeOfFreedom.X]);
            return 0.5 * crossProduct;
        }
    }
}
