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
    public class KeyedMatrix<TKey> : DenseMatrix
    {
        /// <summary>
        /// The keys which identify the rows of this keyed matrix
        /// </summary>
        private List<TKey> keysForRows;
        
        /// <summary>
        /// The keys which identify the columns of this keyed matrix
        /// </summary>
        private List<TKey> keysForColumns;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keys">The keys which will be used to look up rows and columns of this square matrix. One unique key is expected per row.</param>
        public KeyedMatrix(IList<TKey> keys)
            : this(keys, keys)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public KeyedMatrix(IList<TKey> keysForRows, IList<TKey> keysForColumns)
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
        public KeyedMatrix(IList<TKey> keysForRows, IList<TKey> keysForColumns, double initialValueOfAllElements)
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
        public KeyedMatrix(Matrix<double> matrix, IList<TKey> keysForRows, IList<TKey> keysForColumns)
            : base(matrix)
        {
            this.CheckAndAddKeys(keysForRows, keysForColumns);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds the keys and data to copy into this new matrix</param>
        public KeyedMatrix(KeyedMatrix<TKey> matrix)
            : base(matrix)
        {
            this.CheckAndAddKeys(matrix.RowKeys, matrix.ColumnKeys);
        }
        
        /// <summary>
        /// Gets the keys for the rows
        /// </summary>
        public IList<TKey> RowKeys
        {
            get
            {
                return this.keysForRows.AsReadOnly();
            }
            
            private set
            {
                this.keysForRows = new List<TKey>(value);
            }
        }
        
        /// <summary>
        /// Gets the keys for the columns
        /// </summary>
        public IList<TKey> ColumnKeys
        {
            get
            {
                return this.keysForColumns.AsReadOnly();
            }
            
            private set
            {
                this.keysForColumns = new List<TKey>(value);
            }
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
        public KeyedMatrix<TKey> SubMatrix(TKey rowKey, int rowCount, TKey columnKey, int columnCount)
        {
            int rowIndex = this.RowIndex(rowKey);
            int columnIndex = this.ColumnIndex(columnKey);
            
            IList<TKey> subMatrixRowKeys = new List<TKey>()
            {
                rowKey
            };
            IList<TKey> subMatrixColumnKeys = new List<TKey>()
            {
                columnKey
            };
            for (int i = 1; i < rowCount; i++)
            {
                subMatrixRowKeys.Add(this.RowKeyFromIndex(rowIndex + i));
            }
            
            for (int i = 1; i < rowCount; i++)
            {
                subMatrixColumnKeys.Add(this.RowKeyFromIndex(rowIndex + i));
            }
            
            Matrix<double> subMatrix = this.SubMatrix(rowIndex, rowCount, columnIndex, columnCount);
            
            return new KeyedMatrix<TKey>(subMatrix, subMatrixRowKeys, subMatrixColumnKeys);
        }
        
        /// <summary>
        /// Creates a matrix which contains values from the requested sub-matrix
        /// </summary>
        /// <param name="rowsToInclude">A list of the keys of rows to include in the new matrix</param>
        /// <param name="columnsToInclude">A list of the keys of columns to include in the new matrix</param>
        /// <returns>A KeyedMatrix which contains values from the requested sub-matrix</returns>
        public KeyedMatrix<TKey> SubMatrix(IList<TKey> rowsToInclude, IList<TKey> columnsToInclude)
        {
            KeyedMatrix<TKey> subMatrix = new KeyedMatrix<TKey>(rowsToInclude, columnsToInclude);
            
            foreach (TKey rowKey in rowsToInclude)
            {
                foreach (TKey columnKey in columnsToInclude)
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
        public double At(TKey rowKey, TKey columnKey)
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
        public void At(TKey rowKey, TKey columnKey, double value)
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
        
        /// <summary>
        /// Replaces the keys with the provided lists.
        /// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
        /// </summary>
        /// <param name="keysForRows">The keys to replace the RowKeys property with</param>
        /// <param name="keysForColumns">The keys to replace the ColumnKeys property with</param>
        private void CheckAndAddKeys(IList<TKey> keysForRows, IList<TKey> keysForColumns)
        {
            if (keysForRows == null)
            {
                throw new ArgumentNullException("keysForRows");
            }
            
            if (keysForColumns == null)
            {
                throw new ArgumentNullException("keysForColumns");
            }
            
            if (this.RowCount != keysForRows.Count)
            {
                throw new ArgumentException("The number of items in the rowKeys list should match the number of rows of the underlying matrix", "keysForRows");
            }
            
            if (this.ColumnCount != keysForColumns.Count)
            {
                throw new ArgumentException("The number of items in the columnKeys list should match the number of rows of the underlying matrix", "keysForColumns");
            }
            
            // TODO check for duplicate keys?
            this.RowKeys = keysForRows;
            this.ColumnKeys = keysForColumns;
        }
        
        /// <summary>
        /// Determines the row index in the matrix
        /// </summary>
        /// <param name="rowKey">The key for which to find the index</param>
        /// <returns>An integer representing the row index in the matrix</returns>
        private int RowIndex(TKey rowKey)
        {
            return this.RowKeys.IndexOf(rowKey);
        }
        
        /// <summary>
        /// Determines the column index in the matrix
        /// </summary>
        /// <param name="columnKey">The key for which to find the index</param>
        /// <returns>An integer representing the column index in the matrix</returns>
        private int ColumnIndex(TKey columnKey)
        {
            return this.ColumnKeys.IndexOf(columnKey);
        }
        
        /// <summary>
        /// Determines the key which can be used to identify a row of the matrix
        /// </summary>
        /// <param name="rowIndex">The index of the row</param>
        /// <returns>The key of the row</returns>
        private TKey RowKeyFromIndex(int rowIndex)
        {
            if (rowIndex > this.RowCount)
            {
                throw new ArgumentException("rowIndex cannot be greater than the number of rows of the matrix", "rowIndex");
            }
            
            return this.RowKeys[rowIndex];
        }
        
        /// <summary>
        /// Determines the key which can be used to identify a column of the matrix
        /// </summary>
        /// <param name="columnIndex">The index of the column</param>
        /// <returns>The key of the column</returns>
        private TKey ColumnKeyFromIndex(int columnIndex)
        {
            if (columnIndex > this.ColumnCount)
            {
                throw new ArgumentException("columnIndex cannot be greater than the number of columns of the matrix", "columnIndex");
            }
            
            return this.ColumnKeys[columnIndex];
        }
    }
}
