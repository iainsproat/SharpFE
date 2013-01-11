//-----------------------------------------------------------------------
// <copyright file="ElementStiffnessMatrix.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    using MathNet.Numerics.LinearAlgebra.Generic;

    /// <summary>
    /// ElementStiffnessMatrix is a KeyedMatrix which uses NodalDegreeOfFreedom structs as the keys.
    /// It is designed to be used for holding data about stiffnesses of finite elements.
    /// </summary>
    public class StiffnessMatrix : KeyedMatrix<NodalDegreeOfFreedom>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementStiffnessMatrix" /> class.
        /// </summary>
        /// <param name="keys">The keys which will be used to look up rows and columns of this square matrix. One unique key is expected per row.</param>
        public StiffnessMatrix(IList<NodalDegreeOfFreedom> keys)
            : this(keys, keys)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementStiffnessMatrix" /> class
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public StiffnessMatrix(IList<NodalDegreeOfFreedom> keysForRows, IList<NodalDegreeOfFreedom> keysForColumns)
            : base(keysForRows, keysForColumns)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementStiffnessMatrix" /> class
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        /// <param name="initialValueOfAllElements">The value to which we assign to each element of the matrix</param>
        public StiffnessMatrix(IList<NodalDegreeOfFreedom> keysForRows, IList<NodalDegreeOfFreedom> keysForColumns, double initialValueOfAllElements)
            : base(keysForRows, keysForColumns, initialValueOfAllElements)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementStiffnessMatrix" /> class
        /// </summary>
        /// <param name="matrix">The matrix which holds the data to copy into this new matrix</param>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public StiffnessMatrix(Matrix<double> matrix, IList<NodalDegreeOfFreedom> keysForRows, IList<NodalDegreeOfFreedom> keysForColumns)
            : base(matrix, keysForRows, keysForColumns)
        {
            // empty
        }
        
        public StiffnessMatrix(KeyedMatrix<NodalDegreeOfFreedom> matrix)
            : base(matrix)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementStiffnessMatrix" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds the keys and data to copy into this new matrix</param>
        /// <remarks>Undertakes a deep clone of the data of the matrix, but only a shallow clone of the keys</remarks>
        public StiffnessMatrix(StiffnessMatrix matrix)
            : base(matrix)
        {
            // empty
        }
        
        /// <summary>
        /// Returns a submatrix of the stiffness matrix.
        /// </summary>
        /// <param name="nodeRowKey">The rows of the returned matrix relate to <see cref="NodalDegreeOfFreedom">NodalDegreeOfFreedoms</see>, where the Node property of these NodalDegreeOfFreedom equals the nodeRowKey</param>
        /// <param name="nodeColumnKey">the node which defines the columns of the submatrix</param>
        /// <returns>A submatrix of the stiffness matrix</returns>
        public StiffnessMatrix SubMatrix(FiniteElementNode nodeRowKey, FiniteElementNode nodeColumnKey)
        {
            // TODO guard against bad parameters
            IList<NodalDegreeOfFreedom> validRowKeys = this.GetAllRowKeysWithMatchingNode(nodeRowKey);
            IList<NodalDegreeOfFreedom> validColumnKeys = this.GetAllColumnKeysWithMatchingNode(nodeColumnKey);
            
            StiffnessMatrix result = new StiffnessMatrix(validRowKeys, validColumnKeys);
            
            foreach (NodalDegreeOfFreedom rowKey in validRowKeys)
            {
                foreach (NodalDegreeOfFreedom columnKey in validColumnKeys)
                {
                    result.At(rowKey, columnKey, this.At(rowKey, columnKey));
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Retrieves the requested element from the matrix
        /// </summary>
        /// <param name="nodeComponentOfRowKey">the node which in part defines the row of the element</param>
        /// <param name="degreeOfFreedomComponentOfRowKey">the degree of freedom which in part defines the row of the element</param>
        /// <param name="nodeComponentOfColumnKey">the node which in part defines the column of the element</param>
        /// <param name="degreeOfFreedomComponentOfColumnKey">the degree of freedom which in part defines the column of the element</param>
        /// <returns>The requested element</returns>
        public double At(FiniteElementNode nodeComponentOfRowKey, DegreeOfFreedom degreeOfFreedomComponentOfRowKey, FiniteElementNode nodeComponentOfColumnKey, DegreeOfFreedom degreeOfFreedomComponentOfColumnKey)
        {
            return this.At(new NodalDegreeOfFreedom(nodeComponentOfRowKey, degreeOfFreedomComponentOfRowKey), new NodalDegreeOfFreedom(nodeComponentOfColumnKey, degreeOfFreedomComponentOfColumnKey));
        }
        
        /// <summary>
        /// Sets the value of the element at the given location
        /// </summary>
        /// <param name="nodeComponentOfRowKey">the node which in part defines the row of the element</param>
        /// <param name="degreeOfFreedomComponentOfRowKey">the degree of freedom which in part defines the row of the element</param>
        /// <param name="nodeComponentOfColumnKey">the node which in part defines the column of the element</param>
        /// <param name="degreeOfFreedomComponentOfColumnKey">the degree of freedom which in part defines the column of the element</param>
        /// <param name="value">The value with which to set the value of the element</param>
        public void At(FiniteElementNode nodeComponentOfRowKey, DegreeOfFreedom degreeOfFreedomComponentOfRowKey, FiniteElementNode nodeComponentOfColumnKey, DegreeOfFreedom degreeOfFreedomComponentOfColumnKey, double value)
        {
            this.At(new NodalDegreeOfFreedom(nodeComponentOfRowKey, degreeOfFreedomComponentOfRowKey), new NodalDegreeOfFreedom(nodeComponentOfColumnKey, degreeOfFreedomComponentOfColumnKey), value);
        }
        
        /// <summary>
        /// Clones this matrix
        /// </summary>
        /// <returns>A deep clone of the data of this matrix, but only a shallow clone of the keys</returns>
        public override Matrix<double> Clone()
        {
            return new StiffnessMatrix(this);
        }
        
        /// <summary>
        /// Gets all the items whose node property matches the given node
        /// </summary>
        /// <param name="listToFindMatchesFrom">The list from which to select items</param>
        /// <param name="nodeToMatch">the node to match</param>
        /// <returns>A list of all items which have Node properties which equal the given node</returns>
        private static IList<NodalDegreeOfFreedom> GetAllRowKeysWithMatchingNode(IList<NodalDegreeOfFreedom> listToFindMatchesFrom, FiniteElementNode nodeToMatch)
        {
            IList<NodalDegreeOfFreedom> matchingKeys = new List<NodalDegreeOfFreedom>();
            
            // TODO Linq
            foreach (NodalDegreeOfFreedom nodalDof in listToFindMatchesFrom)
            {
                if (nodalDof.Node.Equals(nodeToMatch))
                {
                    matchingKeys.Add(nodalDof);
                }
            }
            
            return matchingKeys;
        }
        
        /// <summary>
        /// Gets all the row keys which have a Node property which equals the given node
        /// </summary>
        /// <param name="nodeToMatch">The node to match</param>
        /// <returns>A list of all the row keys which have Node properties which equal the given node</returns>
        private IList<NodalDegreeOfFreedom> GetAllRowKeysWithMatchingNode(FiniteElementNode nodeToMatch)
        {
            return StiffnessMatrix.GetAllRowKeysWithMatchingNode(this.RowKeys, nodeToMatch);
        }
        
        /// <summary>
        /// Gets all the column keys which have a Node property which equals the given node
        /// </summary>
        /// <param name="nodeToMatch">The node to match</param>
        /// <returns>A list of all the column keys which have Node properties which equal the given node</returns>
        private IList<NodalDegreeOfFreedom> GetAllColumnKeysWithMatchingNode(FiniteElementNode nodeToMatch)
        {
            return StiffnessMatrix.GetAllRowKeysWithMatchingNode(this.ColumnKeys, nodeToMatch);
        }
    }
}
