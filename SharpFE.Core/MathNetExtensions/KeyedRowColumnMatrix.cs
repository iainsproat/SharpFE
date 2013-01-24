//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
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
    /// Description of KeyedRowColumnMatrix.
    /// </summary>
    public class KeyedRowColumnMatrix<TRowKey, TColumnKey> : DenseMatrix
    {
        /// <summary>
        /// The keys which identify the rows of this keyed matrix
        /// </summary>
        private IDictionary<TRowKey, int> keysForRows = new Dictionary<TRowKey, int>();
        
        /// <summary>
        /// The keys which identify the columns of this keyed matrix
        /// </summary>
        private IDictionary<TColumnKey, int> keysForColumns = new Dictionary<TColumnKey, int>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public KeyedRowColumnMatrix(IList<TRowKey> keysForRows, IList<TColumnKey> keysForColumns)
            : base(keysForRows.Count, keysForColumns.Count)
        {
            this.CheckAndAddKeys(keysForRows, keysForColumns);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        /// <param name="initialValueOfAllElements">The value to which we assign to each element of the matrix</param>
        public KeyedRowColumnMatrix(IList<TRowKey> keysForRows, IList<TColumnKey> keysForColumns, double initialValueOfAllElements)
            : base(keysForRows.Count, keysForColumns.Count, initialValueOfAllElements)
        {
            this.CheckAndAddKeys(keysForRows, keysForColumns);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds data to copy into this new matrix</param>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public KeyedRowColumnMatrix(Matrix<double> matrix, IList<TRowKey> keysForRows, IList<TColumnKey> keysForColumns)
            : base(matrix)
        {
            this.CheckAndAddKeys(keysForRows, keysForColumns);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds the keys and data to copy into this new matrix</param>
        public KeyedRowColumnMatrix(KeyedRowColumnMatrix<TRowKey, TColumnKey> matrix)
            : base(matrix)
        {
            this.CheckAndAddKeys(matrix.RowKeys, matrix.ColumnKeys);
        }
        
        /// <summary>
        /// Gets the keys for the rows
        /// </summary>
        public IList<TRowKey> RowKeys
        {
            get
            {
                return new List<TRowKey>(this.keysForRows.Keys);
            }
        }
        
        /// <summary>
        /// Gets the keys for the columns
        /// </summary>
        public IList<TColumnKey> ColumnKeys
        {
            get
            {
                return new List<TColumnKey>(this.keysForColumns.Keys);
            }
        }
        
        /// <summary>
        /// Clones this matrix
        /// </summary>
        /// <returns>A shallow clone of this matrix</returns>
        public override Matrix<double> Clone()
        {
            return new KeyedRowColumnMatrix<TRowKey, TColumnKey>(this);
        }
        
        /// <summary>
        /// Creates a matrix which contains values from the requested sub-matrix
        /// </summary>
        /// <param name="rowsToInclude">A list of the keys of rows to include in the new matrix</param>
        /// <param name="columnsToInclude">A list of the keys of columns to include in the new matrix</param>
        /// <returns>A KeyedMatrix which contains values from the requested sub-matrix</returns>
        public KeyedRowColumnMatrix<TRowKey, TColumnKey> SubMatrix(IList<TRowKey> rowsToInclude, IList<TColumnKey> columnsToInclude)
        {
            KeyedRowColumnMatrix<TRowKey, TColumnKey> subMatrix = new KeyedRowColumnMatrix<TRowKey, TColumnKey>(rowsToInclude, columnsToInclude);
            
            foreach (TRowKey rowKey in rowsToInclude)
            {
                foreach (TColumnKey columnKey in columnsToInclude)
                {
                    subMatrix.At(rowKey, columnKey, this.At(rowKey, columnKey));
                }
            }
            
            return subMatrix;
        }
        
        /// <summary>
        /// Retrieves the requested element.
        /// </summary>
        /// <param name="rowKey">The row of the element</param>
        /// <param name="columnKey">The column of the element</param>
        /// <returns>The requested element</returns>
        public double At(TRowKey rowKey, TColumnKey columnKey)
        {
            Guard.AgainstNullArgument(rowKey, "rowKey");
            Guard.AgainstNullArgument(columnKey, "columnKey");
            
            int rowIndex = this.RowIndex(rowKey);
            int columnIndex = this.ColumnIndex(columnKey);
            return this.At(rowIndex, columnIndex);
        }
        
        /// <summary>
        /// Sets the value of the given element
        /// </summary>
        /// <param name="rowKey">the row of the element</param>
        /// <param name="columnKey">the column of the element</param>
        /// <param name="value">The value to set the element to</param>
        public void At(TRowKey rowKey, TColumnKey columnKey, double value)
        {
            Guard.AgainstNullArgument(rowKey, "rowKey");
            Guard.AgainstNullArgument(columnKey, "columnKey");
            
            int rowIndex;
            int columnIndex;
            
            try
            {
                rowIndex = this.RowIndex(rowKey);
            }
            catch (KeyNotFoundException knfe)
            {
                throw new InvalidOperationException(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "A value could not be added to a matrix.  The key provided for the row does not exist in the matrix.  The key provided for the row is : {0}. We have not checked the key for the column, but it was : {1}",
                        rowKey,
                        columnKey),
                    knfe);
            }
            
            try
            {
                columnIndex = this.ColumnIndex(columnKey);
            }
            catch (KeyNotFoundException knfe)
            {
                throw new InvalidOperationException(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "A value could not be added to a matrix.  The key provided for the column does not exist in the matrix.  The key provided for the column is : {0}. However, the key for the row could be found, for reference it was : {1}",
                        columnKey,
                        rowKey),
                    knfe);
            }
            
            this.At(rowIndex, columnIndex, value);
        }
        
        public new KeyedRowColumnMatrix<TColumnKey, TRowKey> Transpose()
        {
            return new KeyedRowColumnMatrix<TColumnKey, TRowKey>(base.Transpose(), this.ColumnKeys, this.RowKeys);
        }
        
        public new KeyedRowColumnMatrix<TColumnKey, TRowKey> Inverse()
        {
            Matrix<double> result = ((Matrix<double>)this).Inverse();
            return new KeyedRowColumnMatrix<TColumnKey, TRowKey>(result, this.ColumnKeys, this.RowKeys);
        }
        
        public KeyedRowColumnMatrix<TRowKey, TOtherColumnKey> Multiply<TOtherRowKey, TOtherColumnKey>(KeyedRowColumnMatrix<TOtherRowKey, TOtherColumnKey> other)
        {
            KeyCompatibilityValidator<TColumnKey, TOtherRowKey> kcv = new KeyCompatibilityValidator<TColumnKey, TOtherRowKey>(this.ColumnKeys, other.RowKeys);
            kcv.ThrowIfInvalid();
            
            ////TODO If the keys match but are in the wrong order, copy the other matrix and swap its rows and row keys so they match exactly
            Matrix<double> result = base.Multiply(other);
            return new KeyedRowColumnMatrix<TRowKey, TOtherColumnKey>(result, this.RowKeys, other.ColumnKeys);
        }
        
        public new KeyedRowColumnMatrix<TRowKey, TColumnKey> Multiply(double scalar)
        {
            Matrix<double> result = base.Multiply(scalar);
            return new KeyedRowColumnMatrix<TRowKey, TColumnKey>(result, this.RowKeys, this.ColumnKeys);
        }
        
        public KeyedVector<TRowKey> Multiply(KeyedVector<TColumnKey> rightSide)
        {
            KeyCompatibilityValidator<TColumnKey, TColumnKey> kcv = new KeyCompatibilityValidator<TColumnKey, TColumnKey>(this.ColumnKeys, rightSide.Keys);
            kcv.ThrowIfInvalid();
            
            ////TODO If the keys match but are in the wrong order, swap the vector items and keys to match exactly
            Vector<double> result = base.Multiply(rightSide);
            return new KeyedVector<TRowKey>(result, this.RowKeys);
        }
        
        /// <summary>
        /// Replaces the keys with the provided lists.
        /// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
        /// </summary>
        /// <param name="keysForRows">The keys to replace the RowKeys property with</param>
        /// <param name="keysForColumns">The keys to replace the ColumnKeys property with</param>
        private void CheckAndAddKeys(IList<TRowKey> keysRows, IList<TColumnKey> keysColumns)
        {
            Guard.AgainstNullArgument(keysRows, "keysRows");
            Guard.AgainstNullArgument(keysColumns, "keysColumns");
            Guard.AgainstBadArgument(
                () => { return this.RowCount != keysRows.Count; },
                "The number of items in the rowKeys list should match the number of rows of the underlying matrix",
                "keysForRows");
            Guard.AgainstBadArgument(
                () => { return this.ColumnCount != keysColumns.Count; },
                "The number of items in the columnKeys list should match the number of rows of the underlying matrix",
                "keysForColumns");
            
            this.keysForRows.Clear();
            this.keysForColumns.Clear();
            
            int numRowKeys = keysRows.Count;
            int numColKeys = keysColumns.Count;
            for (int i = 0; i < numRowKeys; i++)
            {
                this.keysForRows.Add(keysRows[i], i);
            }
            
            for (int j = 0; j < numColKeys; j++)
            {
                this.keysForColumns.Add(keysColumns[j], j);
            }
        }
        
        /// <summary>
        /// Determines the row index in the matrix
        /// </summary>
        /// <param name="rowKey">The key for which to find the index</param>
        /// <returns>An integer representing the row index in the matrix</returns>
        private int RowIndex(TRowKey rowKey)
        {
            try
            {
                return this.keysForRows[rowKey];
            }
            catch (KeyNotFoundException knfe)
            {
                throw new InvalidOperationException(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "We could not find the requested row in this matrix.  The key provided for the row was : {0}.",
                        rowKey),
                    knfe);
            }
        }
        
        /// <summary>
        /// Determines the column index in the matrix
        /// </summary>
        /// <param name="columnKey">The key for which to find the index</param>
        /// <returns>An integer representing the column index in the matrix</returns>
        private int ColumnIndex(TColumnKey columnKey)
        {
            try
            {
                return this.keysForColumns[columnKey];
            }
            catch (KeyNotFoundException knfe)
            {
                throw new InvalidOperationException(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "We could not find the requested column within this matrix.  The key provided for the column was : {0}.",
                        columnKey),
                    knfe);
            }
        }
    }
}
