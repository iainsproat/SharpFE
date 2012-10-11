//-----------------------------------------------------------------------
// <copyright file="StiffnessMatrixBuilder.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using MathNet.Numerics.LinearAlgebra.Double;
    
    /// <summary>
    /// Builds various types of stiffness matrices from a Finite Element model
    /// </summary>
    public class StiffnessMatrixBuilder
    {
        /// <summary>
        /// The finite element model from which to build the matrices
        /// </summary>
        private FiniteElementModel parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="StiffnessMatrixBuilder" /> class.
        /// </summary>
        /// <param name="parentModel">The model from which the stiffness matrices will be built.</param>
        public StiffnessMatrixBuilder(FiniteElementModel parentModel)
        {
            this.parent = parentModel;
        }
        
        /// <summary>
        /// The stiffness matrix which relates to known forces and known displacements
        /// </summary>
        /// <returns>A stiffness matrix for known forces and known displacements</returns>
        public Matrix BuildKnownForcesKnownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> knownForces = this.parent.DegreesOfFreedomWithKnownForce;
            
            IList<NodalDegreeOfFreedom> knownDisplacements = this.parent.DegreesOfFreedomWithKnownDisplacement;
            if (knownDisplacements == null || knownDisplacements.Count == 0)
            {
                throw new InvalidOperationException("The has no constraints and therefore cannot be solved");
            }
            
            return this.BuildStiffnessSubMatrix(knownForces, knownDisplacements);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to known forces and unknown displacements
        /// </summary>
        /// <returns>A stiffness matrix for known forces and unknown displacements</returns>
        public Matrix BuildKnownForcesUnknownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> knownForces = this.parent.DegreesOfFreedomWithKnownForce;
            if (knownForces == null || knownForces.Count == 0)
            {
                throw new InvalidOperationException("The model has too many constraints and no displacements will occur.  The reactions of each node equals the forces applied to each node.");
            }
            
            IList<NodalDegreeOfFreedom> unknownDisplacements = this.parent.DegreesOfFreedomWithUnknownDisplacement;
            
            return this.BuildStiffnessSubMatrix(knownForces, unknownDisplacements);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to unknown forces and known displacements
        /// </summary>
        /// <returns>A stiffness matrix for unknown forces and known displacements</returns>
        public Matrix BuildUnknownForcesKnownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> unknownForces = this.parent.DegreesOfFreedomWithUnknownForce;
            
            IList<NodalDegreeOfFreedom> knownDisplacements = this.parent.DegreesOfFreedomWithKnownDisplacement;
            return this.BuildStiffnessSubMatrix(unknownForces, knownDisplacements);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to unknown forces and unknown displacements
        /// </summary>
        /// <returns>A stiffness matrix for unknown forces and unknown displacements</returns>
        public Matrix BuildUnknownForcesUnknownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> unknownForces = this.parent.DegreesOfFreedomWithUnknownForce;
            
            IList<NodalDegreeOfFreedom> unknownDisplacements = this.parent.DegreesOfFreedomWithUnknownDisplacement;
            return this.BuildStiffnessSubMatrix(unknownForces, unknownDisplacements);
        }
        
        /// <summary>
        /// Sums all the stiffnesses across all elements which are connected to the given nodes
        /// </summary>
        /// <param name="connectedElements">The list of all connected elements to the row node</param>
        /// <param name="row">The node and degree of freedom which define the row</param>
        /// <param name="column">The node and degree of freedom which define the column</param>
        /// <returns>A double representing the stiffness for this element</returns>
        private static double SumStiffnessesForAllElementsAt(IList<FiniteElement> connectedElements, NodalDegreeOfFreedom row, NodalDegreeOfFreedom column)
        {
            double totalStiffness = 0.0;
            foreach (FiniteElement e in connectedElements)
            {
                if (e.Nodes.Contains(column.Node))
                {
                    totalStiffness += e.GetStiffnessAt(row.Node, row.DegreeOfFreedom, column.Node, column.DegreeOfFreedom);
                }
            }
            
            return totalStiffness;
        }
        
        /// <summary>
        /// This function iterates through all the elements which provide stiffnesses for the given combination of nodal degree of freedoms
        /// and sums them to provide the total stiffness
        /// </summary>
        /// <param name="rowData">The nodal degree of freedoms which represent the rows of the matrix</param>
        /// <param name="columnData">The nodal degree of freedoms which represents the columns of the matrix</param>
        /// <returns>A matrix representing the stiffness for these combinations of rows and columns</returns>
        private Matrix BuildStiffnessSubMatrix(IList<NodalDegreeOfFreedom> rowData, IList<NodalDegreeOfFreedom> columnData)
        {
            Guard.AgainstNullArgument(rowData, "rowData");
            Guard.AgainstNullArgument(columnData, "columnData");
            
            int numRows = rowData.Count;
            int numCols = columnData.Count;
            
            Matrix result = new DenseMatrix(numRows, numCols);
            IList<FiniteElement> connectedElements;
            NodalDegreeOfFreedom row;
            
            // TODO Parallel?
            for (int i = 0; i < numRows; i++)
            {
                row = rowData[i];
                connectedElements = this.parent.GetAllElementsConnectedTo(row.Node);
                
                for (int j = 0; j < numCols; j++)
                {
                    double currentResult = StiffnessMatrixBuilder.SumStiffnessesForAllElementsAt(connectedElements, row, columnData[j]);
                    result.At(i, j, currentResult);
                }
            }
            
            return result;
        }
    }
}
