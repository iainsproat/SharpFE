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
    using SharpFE.MathNetExtensions;

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
        
        public KeyedMatrix<TKey> Multiply(KeyedRowColumnMatrix<TKey, TKey> other)
        {
            KeyCompatibilityValidator<TKey, TKey> kcv = new KeyCompatibilityValidator<TKey, TKey>(this.ColumnKeys, other.RowKeys);
            kcv.ThrowIfInvalid();
            ////TODO If the keys match but are in the wrong order, copy the other matrix and swap its rows and row keys so they match exactly
            
            Matrix<double> result = ((Matrix<double>)this).Multiply((Matrix<double>)other);
            return new KeyedMatrix<TKey>(result, this.RowKeys, other.ColumnKeys);
        }
        
        /// <summary>
        /// Clones this matrix
        /// </summary>
        /// <returns>A shallow clone of this matrix</returns>
        public override Matrix<double> Clone()
        {
            return new KeyedMatrix<TKey>(this);
        }
        
        public new KeyedMatrix<TKey> Inverse()
        {
            Matrix<double> result = ((Matrix<double>)this).Inverse();
            return new KeyedMatrix<TKey>(result, this.ColumnKeys, this.RowKeys);
        }
        
        public new KeyedMatrix<TKey> Transpose()
        {
            Matrix<double> result = ((Matrix<double>)this).Transpose();
            return new KeyedMatrix<TKey>(result, this.ColumnKeys, this.RowKeys);
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
