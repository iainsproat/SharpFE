//-----------------------------------------------------------------------
// <copyright file="KeyedMatrix.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra.Generic;

    /// <summary>
    /// A KeyedMatrix is a matrix whose elements can be accessed by Keys, rather than just index integers.
    /// This is roughly analagous to what a Dictionary is to a List.
    /// </summary>
    /// <typeparam name="TKey">The type of the instances which form the keys to this KeyedMatrix</typeparam>
    public class KeyedMatrix<TKey> : KeyedRowColumnMatrix<TKey, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keys">The keys which will be used to look up rows and columns of this square matrix. One unique key is expected per row.</param>
        public KeyedMatrix(IList<TKey> keys)
            : base(keys, keys)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public KeyedMatrix(IList<TKey> keysForRows, IList<TKey> keysForColumns)
            : base(keysForRows, keysForColumns)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        /// <param name="initialValueOfAllElements">The value to which we assign to each element of the matrix</param>
        public KeyedMatrix(IList<TKey> keysForRows, IList<TKey> keysForColumns, double initialValueOfAllElements)
            : base(keysForRows, keysForColumns, initialValueOfAllElements)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds data to copy into this new matrix</param>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public KeyedMatrix(Matrix<double> matrix, IList<TKey> keysForRows, IList<TKey> keysForColumns)
            : base(matrix, keysForRows, keysForColumns)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds the keys and data to copy into this new matrix</param>
        public KeyedMatrix(KeyedMatrix<TKey> matrix)
            : base(matrix)
        {
            // empty
        }
        
        /// <summary>
        /// Clones this matrix
        /// </summary>
        /// <returns>A shallow clone of this matrix</returns>
        public override Matrix<double> Clone()
        {
            return new KeyedMatrix<TKey>(this);
        }
        
        /// <summary>
        /// Creates a matrix which contains the values of the requested submatrix
        /// </summary>
        /// <param name="rowKey">The key which defines the first row to start copying from</param>
        /// <param name="rowCount">The number of rows to copy</param>
        /// <param name="columnKey">The key which defines the first column to start copying from</param>
        /// <param name="columnCount">The number of columns to copy</param>
        /// <returns>A matrix which is a submatrix of this KeyedMatrix</returns>
        public new KeyedMatrix<TKey> SubMatrix(TKey rowKey, int rowCount, TKey columnKey, int columnCount)
        {
        	return (KeyedMatrix<TKey>)base.SubMatrix(rowKey, rowCount, columnKey, columnCount);
        }
        
        /// <summary>
        /// Creates a matrix which contains values from the requested sub-matrix
        /// </summary>
        /// <param name="rowsToInclude">A list of the keys of rows to include in the new matrix</param>
        /// <param name="columnsToInclude">A list of the keys of columns to include in the new matrix</param>
        /// <returns>A KeyedMatrix which contains values from the requested sub-matrix</returns>
        public new KeyedMatrix<TKey> SubMatrix(IList<TKey> rowsToInclude, IList<TKey> columnsToInclude)
        {
        	return (KeyedMatrix<TKey>)base.SubMatrix(rowsToInclude, columnsToInclude);
        }
    }
}
