//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace SharpFE
{
	/// <summary>
	/// Description of KeyedRowColumnMatrix.
	/// </summary>
	public class KeyedRowColumnMatrix<TRowKey, TColumnKey> : DenseMatrix
	{
		/// <summary>
        /// The keys which identify the rows of this keyed matrix
        /// </summary>
        private IList<TRowKey> keysForRows;
        
        /// <summary>
        /// The keys which identify the columns of this keyed matrix
        /// </summary>
        private IList<TColumnKey> keysForColumns;
        
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
            	return ((List<TRowKey>)this.keysForRows).AsReadOnly();
            }
            
            private set
            {
                this.keysForRows = new List<TRowKey>(value);
            }
        }
        
        /// <summary>
        /// Gets the keys for the columns
        /// </summary>
        public IList<TColumnKey> ColumnKeys
        {
            get
            {
            	return ((List<TColumnKey>)this.keysForColumns).AsReadOnly();
            }
            
            private set
            {
                this.keysForColumns = new List<TColumnKey>(value);
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
        /// Creates a matrix which contains the values of the requested submatrix
        /// </summary>
        /// <param name="rowKey">The key which defines the first row to start copying from</param>
        /// <param name="rowCount">The number of rows to copy</param>
        /// <param name="columnKey">The key which defines the first column to start copying from</param>
        /// <param name="columnCount">The number of columns to copy</param>
        /// <returns>A matrix which is a submatrix of this KeyedMatrix</returns>
        public KeyedRowColumnMatrix<TRowKey, TColumnKey> SubMatrix(TRowKey rowKey, int rowCount, TColumnKey columnKey, int columnCount)
        {
            int rowIndex = this.RowIndex(rowKey);
            int columnIndex = this.ColumnIndex(columnKey);
            
            IList<TRowKey> subMatrixRowKeys = new List<TRowKey>()
            {
                rowKey
            };
            IList<TColumnKey> subMatrixColumnKeys = new List<TColumnKey>()
            {
                columnKey
            };
            for (int i = 1; i < rowCount; i++)
            {
                subMatrixRowKeys.Add(this.RowKeyFromIndex(rowIndex + i));
            }
            
            for (int i = 1; i < rowCount; i++)
            {
                subMatrixColumnKeys.Add(this.ColumnKeyFromIndex(rowIndex + i));
            }
            
            Matrix<double> subMatrix = this.SubMatrix(rowIndex, rowCount, columnIndex, columnCount);
            
            return new KeyedRowColumnMatrix<TRowKey, TColumnKey>(subMatrix, subMatrixRowKeys, subMatrixColumnKeys);
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
            int rowIndex = this.RowIndex(rowKey);
            if (rowIndex == -1)
            {
                return;
            }
            
            int columnIndex = this.ColumnIndex(columnKey);
            if (columnIndex == -1)
            {
                return;
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
        	//TODO check that this column keys and the other row keys match exactly, including order.
            //If the keys match but are in the wrong order, copy the other matrix and swap its rows and row keys so they match exactly
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
            //TODO check that column keys of the matrix and the keys of the vector match exactly.
            //If the keys match but are in the wrong order, swap the vector items and keys to match exactly
            Vector<double> result = base.Multiply(rightSide);
            return new KeyedVector<TRowKey>(result, this.RowKeys);
        }
        
        /// <summary>
        /// Replaces the keys with the provided lists.
        /// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
        /// </summary>
        /// <param name="keysForRows">The keys to replace the RowKeys property with</param>
        /// <param name="keysForColumns">The keys to replace the ColumnKeys property with</param>
        private void CheckAndAddKeys(IList<TRowKey> keysForRows, IList<TColumnKey> keysForColumns)
        {
            Guard.AgainstNullArgument(keysForRows, "keysForRows");
            Guard.AgainstNullArgument(keysForColumns, "keysForColumns");
            Guard.AgainstBadArgument(
                () => { return this.RowCount != keysForRows.Count; },
                "The number of items in the rowKeys list should match the number of rows of the underlying matrix",
                "keysForRows");
            Guard.AgainstBadArgument(
                () => { return this.ColumnCount != keysForColumns.Count; },
                "The number of items in the columnKeys list should match the number of rows of the underlying matrix",
                "keysForColumns");
            
            // TODO check for duplicate keys?
            this.RowKeys = keysForRows;
            this.ColumnKeys = keysForColumns;
        }
        
        /// <summary>
        /// Determines the row index in the matrix
        /// </summary>
        /// <param name="rowKey">The key for which to find the index</param>
        /// <returns>An integer representing the row index in the matrix</returns>
        private int RowIndex(TRowKey rowKey)
        {
            return this.RowKeys.IndexOf(rowKey);
        }
        
        /// <summary>
        /// Determines the column index in the matrix
        /// </summary>
        /// <param name="columnKey">The key for which to find the index</param>
        /// <returns>An integer representing the column index in the matrix</returns>
        private int ColumnIndex(TColumnKey columnKey)
        {
            return this.ColumnKeys.IndexOf(columnKey);
        }
        
        /// <summary>
        /// Determines the key which can be used to identify a row of the matrix
        /// </summary>
        /// <param name="rowIndex">The index of the row</param>
        /// <returns>The key of the row</returns>
        private TRowKey RowKeyFromIndex(int rowIndex)
        {
            Guard.AgainstBadArgument(
                () => { return rowIndex > this.RowCount; },
                "rowIndex cannot be greater than the number of rows of the matrix",
                "rowIndex");
            
            return this.RowKeys[rowIndex];
        }
        
        /// <summary>
        /// Determines the key which can be used to identify a column of the matrix
        /// </summary>
        /// <param name="columnIndex">The index of the column</param>
        /// <returns>The key of the column</returns>
        private TColumnKey ColumnKeyFromIndex(int columnIndex)
        {
            Guard.AgainstBadArgument(
                () => { return columnIndex > this.ColumnCount; },
                "columnIndex cannot be greater than the number of columns of the matrix",
                "columnIndex");
            
            return this.ColumnKeys[columnIndex];
        }
	}
}
