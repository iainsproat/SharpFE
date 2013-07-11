//-----------------------------------------------------------------------
// <copyright file="KeyedRowColumnMatrix.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra.Generic;
    
    using SharpFE.Maths;

    /// <summary>
    /// </summary>
    /// <typeparam name="TRowKey"></typeparam>
    /// <typeparam name="TColumnKey"></typeparam>
    public class KeyedRowColumnMatrix<TRowKey, TColumnKey>
    {
        /// <summary>
        /// The keys which identify the rows of this keyed matrix
        /// </summary>
        private IDictionary<TRowKey, int> keysForRows;
        
        /// <summary>
        /// The keys which identify the columns of this keyed matrix
        /// </summary>
        private IDictionary<TColumnKey, int> keysForColumns;
        
        /// <summary>
        /// The underlying matrix which holds the data and is delegated to for the majority of operations on the data
        /// </summary>
        private Matrix<double> underlyingMatrix;
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public KeyedRowColumnMatrix(IList<TRowKey> keysForRows, IList<TColumnKey> keysForColumns)
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
        {
            this.CheckAndAddKeys(keysForRows, keysForColumns, initialValueOfAllElements);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds the keys and data to copy into this new matrix</param>
        protected KeyedRowColumnMatrix(IList<TRowKey> rowKeys, IList<TColumnKey> columnKeys, KeyedRowColumnMatrix<TRowKey, TColumnKey> matrix)
        {
            IEnumerable<TRowKey> unionOfRowKeys = rowKeys.Union(matrix.RowKeys);
            IEnumerable<TColumnKey> unionOfColumnKeys = columnKeys.Union(matrix.ColumnKeys);
            this.CheckAndAddKeys(unionOfRowKeys, unionOfColumnKeys);
            foreach (TRowKey rowKey in matrix.RowKeys)
            {
                foreach (TColumnKey columnKey in matrix.ColumnKeys)
                {
                    this.At(rowKey, columnKey, matrix.At(rowKey, columnKey));
                }
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds the keys and data to copy into this new matrix</param>
        protected KeyedRowColumnMatrix(KeyedRowColumnMatrix<TRowKey, TColumnKey> matrix)
            : this(matrix.keysForRows, matrix.keysForColumns, matrix.underlyingMatrix)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        /// <param name="initialValueOfAllElements">The value to which we assign to each element of the matrix</param>
        public KeyedRowColumnMatrix(IList<TRowKey> keysForRows, IList<TColumnKey> keysForColumns, Matrix<double> dataToCopy)
            : this(ListToDictionary(keysForRows), ListToDictionary(keysForColumns), dataToCopy)
        {
            // empty
        }
        
        protected static IDictionary<TKey, int> ListToDictionary<TKey>(IList<TKey> listToConvert)
        {
            Guard.AgainstNullArgument(listToConvert, "listToConvert");
            
            int numItems = listToConvert.Count;
            IDictionary<TKey, int> response = new Dictionary<TKey, int>(numItems);
            for (int i = 0; i < numItems; i++)
            {
                response.Add(listToConvert[i], i);
            }
            
            return response;
        }
        
        protected KeyedRowColumnMatrix(IDictionary<TRowKey, int> rows, IDictionary<TColumnKey, int> columns, Matrix<double> dataToCopy)
        {
            Guard.AgainstNullArgument(rows, "rows");
            Guard.AgainstNullArgument(columns, "columns");
            Guard.AgainstNullArgument(dataToCopy, "dataToCopy");
            
            Guard.AgainstBadArgument(
                "rows",
                () => {
                    return rows.Count != dataToCopy.RowCount;
                },
                "Number of row keys does not equal the number of rows in the underlying data");
            Guard.AgainstBadArgument(
                "columns",
                () => {
                    return columns.Count != dataToCopy.ColumnCount;
                },
                "Number of column keys does not equal the number of columns in the underlying data");
            
            this.keysForRows = new Dictionary<TRowKey, int>(rows);
            this.keysForColumns =  new Dictionary<TColumnKey, int>(columns);
            this.underlyingMatrix = dataToCopy.Clone();
        }
        
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets the keys for the rows
        /// </summary>
        /// <remarks>
        /// This is a copy of the keys.  Changes made to this list will not be reflected in the matrix.
        /// </remarks>
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
        /// <remarks>
        /// This is a copy of the keys.  Changes made to this list will not be reflected in the matrix.
        /// </remarks>
        public IList<TColumnKey> ColumnKeys
        {
            get
            {
                return new List<TColumnKey>(this.keysForColumns.Keys);
            }
        }
        
        public int RowCount
        {
            get
            {
                return this.RowKeys.Count;
            }
        }
        
        public int ColumnCount
        {
            get
            {
                return this.ColumnKeys.Count;
            }
        }
        #endregion
        
        
        public static explicit operator Matrix<double>(KeyedRowColumnMatrix<TRowKey, TColumnKey> matrix)
        {
            return matrix.underlyingMatrix;
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
        
        public double this[TRowKey rowKey, TColumnKey columnKey]
        {
            get
            {
                return this.At(rowKey, columnKey);
            }
            set
            {
                this.At(rowKey, columnKey, value);
            }
        }
        
        public KeyedRowColumnMatrix<TRowKey, TColumnKey> Add(KeyedRowColumnMatrix<TRowKey, TColumnKey> other)
        {
            Matrix<double> result = this.underlyingMatrix.Add(other.underlyingMatrix);
            return new KeyedRowColumnMatrix<TRowKey, TColumnKey>(this.keysForRows, this.keysForColumns, result);
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
            
            int rowIndex, colIndex;
            
            try
            {
                rowIndex = this.keysForRows[rowKey];
            }
            catch (KeyNotFoundException knfe)
            {
                throw new KeyNotFoundException(string.Format("Could not find a row with key of {0}", rowKey), knfe);
            }
            
            try
            {
                colIndex = this.keysForColumns[columnKey];
            }
            catch (KeyNotFoundException knfe)
            {
                throw new KeyNotFoundException(string.Format("Could not find a column with key of {0}", columnKey), knfe);
            }
            
            return this.underlyingMatrix[rowIndex, colIndex];
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
            
            Guard.AgainstInvalidState(() => { return !this.keysForRows.ContainsKey(rowKey);},
                                      "A value could not be added to a matrix.  The key provided for the row does not exist in the matrix.  The key provided for the row is : {0}. We have not checked the key for the column, but it was : {1}",
                                      rowKey,
                                      columnKey);

            Guard.AgainstInvalidState(() => { return !this.keysForColumns.ContainsKey(columnKey); },
                                      "A value could not be added to a matrix.  The key provided for the column does not exist in the matrix.  The key provided for the column is : {0}. However, the key for the row could be found, for reference it was : {1}",
                                      columnKey,
                                      rowKey);
            
            int rowIndex = this.keysForRows[rowKey];
            int colIndex = this.keysForColumns[columnKey];
            this.underlyingMatrix[rowIndex, colIndex] = value;
        }
        
        /// <summary>
        /// Sets all the values to zero.  The row and column keys remain unaffected.
        /// </summary>
        public void Clear()
        {
            this.underlyingMatrix.Clear();
        }
        
        /// <summary>
        /// Clones this matrix
        /// </summary>
        /// <returns>A shallow clone of this matrix</returns>
        public KeyedRowColumnMatrix<TRowKey, TColumnKey> Clone()
        {
            return new KeyedRowColumnMatrix<TRowKey, TColumnKey>(this);
        }
        
        public double Determinant()
        {
            return this.underlyingMatrix.Determinant();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public KeyedRowColumnMatrix<TColumnKey, TRowKey> Inverse()
        {
            Matrix<double> inverseUnderlyingMatrix = this.underlyingMatrix.Inverse();
            return new KeyedRowColumnMatrix<TColumnKey, TRowKey>(this.keysForColumns, this.keysForRows, inverseUnderlyingMatrix);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public KeyedRowColumnMatrix<TRowKey, TOtherColumnKey> Multiply<TOtherRowKey, TOtherColumnKey>(KeyedRowColumnMatrix<TOtherRowKey, TOtherColumnKey> other)
        {
            KeyCompatibilityValidator<TColumnKey, TOtherRowKey> kcv = new KeyCompatibilityValidator<TColumnKey, TOtherRowKey>(this.ColumnKeys, other.RowKeys);
            kcv.ThrowIfInvalid();
            
            ////FIXME If the column keys and other row keys are compatible but are stored or returned in the wrong order then this will return the wrong results.  Need to swap the vector items and keys to match exactly
            
            Matrix<double> multipliedUnderlyingMatrix = this.underlyingMatrix.Multiply(other.underlyingMatrix);
            return new KeyedRowColumnMatrix<TRowKey, TOtherColumnKey>(this.keysForRows, other.keysForColumns, multipliedUnderlyingMatrix);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public KeyedRowColumnMatrix<TRowKey, TColumnKey> Multiply(double scalar)
        {
            Matrix<double> scalarMultipliedUnderlyingMatrix = this.underlyingMatrix.Multiply(scalar);
            return new KeyedRowColumnMatrix<TRowKey, TColumnKey>(this.keysForRows, this.keysForColumns, scalarMultipliedUnderlyingMatrix);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rightSide"></param>
        /// <returns></returns>
        public KeyedVector<TRowKey> Multiply(KeyedVector<TColumnKey> rightSide)
        {
            KeyCompatibilityValidator<TColumnKey, TColumnKey> kcv = new KeyCompatibilityValidator<TColumnKey, TColumnKey>(this.ColumnKeys, rightSide.Keys);
            kcv.ThrowIfInvalid();
            
            ////FIXME If the column keys and vector keys are compatible but are stored or returned in the wrong order then this will return the wrong results.  Need to swap the vector items and keys to match exactly
            
            Vector<double> result = this.underlyingMatrix.Multiply(rightSide.ToVector());
            return new KeyedVector<TRowKey>(this.RowKeys, result);
        }
        
        public KeyedRowColumnMatrix<TRowKey, TColumnKey> NormalizeRows(int p)
        {
            Matrix<double> rowNormalizedUnderlyingMatrix = this.underlyingMatrix.NormalizeRows(p);
            return new KeyedRowColumnMatrix<TRowKey, TColumnKey>(this.keysForRows, this.keysForColumns, rowNormalizedUnderlyingMatrix);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public KeyedRowColumnMatrix<TColumnKey, TRowKey> Transpose()
        {
            Matrix<double> transposedUnderlyingMatrix = this.underlyingMatrix.Transpose();
            return new KeyedRowColumnMatrix<TColumnKey, TRowKey>(this.keysForColumns, this.keysForRows, transposedUnderlyingMatrix);
        }
        
        public Matrix<double> ToMatrix()
        {
            return this.underlyingMatrix.Clone();
        }
        
        public KeyedRowColumnMatrix<TColumnKey, TOtherColumnKey> TransposeThisAndMultiply<TOtherColumnKey>(KeyedRowColumnMatrix<TRowKey, TOtherColumnKey> other)
        {
            Matrix<double> result = this.underlyingMatrix.TransposeThisAndMultiply(other.underlyingMatrix);
            return new KeyedRowColumnMatrix<TColumnKey, TOtherColumnKey>(this.keysForColumns, other.keysForColumns, result);
        }
        
        #region Key and Data initialization
        private void CheckAndAddKeys(IEnumerable<TRowKey> keysRows, IEnumerable<TColumnKey> keysColumns)
        {
            this.CheckAndAddKeys(keysRows, keysColumns, 0);
        }
        
        /// <summary>
        /// Replaces the keys with the provided lists.
        /// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
        /// </summary>
        /// <param name="keysRows">Keys to copy into the RowKeys property</param>
        /// <param name="keysColumns">Keys to copy into the RowColumns property</param>
        private void CheckAndAddKeys(IEnumerable<TRowKey> rows, IEnumerable<TColumnKey> columns, double initialValueOfAllData)
        {
            Guard.AgainstNullArgument(rows, "rows");
            Guard.AgainstNullArgument(columns, "columns");
            
            this.keysForRows = new Dictionary<TRowKey, int>();
            this.keysForColumns = new Dictionary<TColumnKey, int>();
            
            IEnumerator<TRowKey> rowEnumerator = rows.GetEnumerator();
            int i = 0;
            while (rowEnumerator.MoveNext())
            {
                this.keysForRows.Add(rowEnumerator.Current, i++);
            }
            
            IEnumerator<TColumnKey> columnEnumator = columns.GetEnumerator();
            int j = 0;
            while (columnEnumator.MoveNext())
            {
                this.keysForColumns.Add(columnEnumator.Current, j++);
            }
            
            this.underlyingMatrix = new DenseMatrix(i, j, initialValueOfAllData);
        }
        #endregion
        
        
        public override string ToString()
        {
            if (this.underlyingMatrix != null)
            {
                return this.underlyingMatrix.ToString();
            }
            
            return this.GetType().FullName;
        }
    }
}
