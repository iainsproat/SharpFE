//-----------------------------------------------------------------------
// <copyright file="NodalDegreeOfFreedomKeyedMatrix.cs" company="Iain Sproat">
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
	public class NodalDegreeOfFreedomKeyedMatrix
	{
		/// <summary>
		/// The keys which identify the rows of this keyed matrix
		/// </summary>
		private IList<IFiniteElementNode> nodesForRows;
		private IList<DegreeOfFreedom> dofForRows;


		/// <summary>
		/// The keys which identify the columns of this keyed matrix
		/// </summary>
		private IList<IFiniteElementNode> nodesForColumns;
		private IList<DegreeOfFreedom> dofForColumns;


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
		public NodalDegreeOfFreedomKeyedMatrix(IList<IFiniteElementNode> keysForRows, IList<DegreeOfFreedom> allowedDegreeOfFreedomsForRows, IList<IFiniteElementNode> keysForColumns, IList<DegreeOfFreedom> allowedDegreeOfFreedomsForColumns)
		{
			this.CheckAndAddKeys(keysForRows, allowedDegreeOfFreedomsForRows, keysForColumns, allowedDegreeOfFreedomsForColumns);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
		/// </summary>
		/// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
		/// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
		/// <param name="initialValueOfAllElements">The value to which we assign to each element of the matrix</param>
		public NodalDegreeOfFreedomKeyedMatrix(IList<IFiniteElementNode> keysForRows, IList<DegreeOfFreedom> allowedDegreeOfFreedomsForRows, IList<IFiniteElementNode> keysForColumns, IList<DegreeOfFreedom> allowedDegreeOfFreedomsForColumns, double initialValueOfAllElements)
		{
			this.CheckAndAddKeys(keysForRows, allowedDegreeOfFreedomsForRows, keysForColumns, allowedDegreeOfFreedomsForColumns, initialValueOfAllElements);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
		/// </summary>
		/// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
		/// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
		/// <param name="initialValueOfAllElements">The value to which we assign to each element of the matrix</param>
		public NodalDegreeOfFreedomKeyedMatrix(IList<IFiniteElementNode> keysForRows, IList<DegreeOfFreedom> allowedDegreeOfFreedomsForRows, IList<IFiniteElementNode> keysForColumns, IList<DegreeOfFreedom> allowedDegreeOfFreedomsForColumns, Matrix<double> dataToCopy)
		{
			Guard.AgainstNullOrEmptyListArgument(keysForRows, "keysForRows");
			Guard.AgainstNullOrEmptyListArgument(allowedDegreeOfFreedomsForRows, "allowedDegreeOfFreedomsForRows");
			Guard.AgainstNullOrEmptyListArgument(keysForColumns, "keysForColumns");
			Guard.AgainstNullOrEmptyListArgument(allowedDegreeOfFreedomsForColumns, "allowedDegreeOfFreedomsForColumns");
			Guard.AgainstNullArgument(dataToCopy, "dataToCopy");

			Guard.AgainstBadArgument(
				"rows",
				() => {
					return (keysForRows.Count * allowedDegreeOfFreedomsForRows.Count) != dataToCopy.RowCount;
				},
				"Number of row keys does not equal the number of rows in the underlying data");
			Guard.AgainstBadArgument(
				"columns",
				() => {
					return (keysForColumns.Count * allowedDegreeOfFreedomsForColumns.Count) != dataToCopy.ColumnCount;
				},
				"Number of column keys does not equal the number of columns in the underlying data");

			this.nodesForRows = new List<IFiniteElementNode>(keysForRows);
			this.dofForRows = new List<DegreeOfFreedom>(allowedDegreeOfFreedomsForRows);
			this.nodesForColumns = new List<IFiniteElementNode>(keysForColumns);
			this.dofForColumns = new List<DegreeOfFreedom>(allowedDegreeOfFreedomsForColumns);
			this.underlyingMatrix = dataToCopy.Clone();
		}

		protected NodalDegreeOfFreedomKeyedMatrix(NodalDegreeOfFreedomKeyedMatrix matrixToClone)
			: this(matrixToClone.nodesForRows, matrixToClone.dofForRows, matrixToClone.nodesForColumns, matrixToClone.dofForColumns, matrixToClone.underlyingMatrix)
		{
			// empty
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets the keys for the rows
		/// </summary>
		/// <remarks>
		/// This is a copy of the keys.  Changes made to this list will not be reflected in the matrix.
		/// </remarks>
		public IList<IFiniteElementNode> RowKeys
		{
			get
			{
				return new List<IFiniteElementNode>(this.nodesForRows);
			}
		}

		/// <summary>
		/// Gets the keys for the columns
		/// </summary>
		/// <remarks>
		/// This is a copy of the keys.  Changes made to this list will not be reflected in the matrix.
		/// </remarks>
		public IList<IFiniteElementNode> ColumnKeys
		{
			get
			{
				return new List<IFiniteElementNode>(this.nodesForColumns);
			}
		}

		public int RowCount
		{
			get
			{
				return this.nodesForRows.Count * this.dofForRows.Count;
			}
		}

		public int ColumnCount
		{
			get
			{
				return this.nodesForColumns.Count * this.dofForColumns.Count;
			}
		}
		#endregion


		public static explicit operator Matrix<double>(NodalDegreeOfFreedomKeyedMatrix matrix)
		{
			return matrix.underlyingMatrix;
		}

		/// <summary>
		/// Creates a matrix which contains values from the requested sub-matrix
		/// </summary>
		/// <param name="rowsToInclude">A list of the keys of rows to include in the new matrix</param>
		/// <param name="columnsToInclude">A list of the keys of columns to include in the new matrix</param>
		/// <returns>A KeyedMatrix which contains values from the requested sub-matrix</returns>
		public NodalDegreeOfFreedomKeyedMatrix SubMatrix(IList<IFiniteElementNode> rowsToInclude, IList<DegreeOfFreedom> rowDofToInclude, IList<IFiniteElementNode> columnsToInclude, IList<DegreeOfFreedom> columnDofToInclude)
		{
			NodalDegreeOfFreedomKeyedMatrix subMatrix = new NodalDegreeOfFreedomKeyedMatrix(rowsToInclude, rowDofToInclude, columnsToInclude, columnDofToInclude);

			foreach (IFiniteElementNode rowKey in rowsToInclude)
			{
				foreach (DegreeOfFreedom rowDof in rowDofToInclude)
				{
					foreach (IFiniteElementNode columnKey in columnsToInclude)
					{
						foreach(DegreeOfFreedom colDof in columnDofToInclude)
						{
							subMatrix.At(rowKey, rowDof, columnKey, colDof, this.At(rowKey, rowDof, columnKey, colDof));
						}
					}
				}
			}

			return subMatrix;
		}

		public double this[IFiniteElementNode rowKey, DegreeOfFreedom rowDof, IFiniteElementNode columnKey, DegreeOfFreedom columnDof]
		{
			get
			{
				return this.At(rowKey, rowDof, columnKey, columnDof);
			}
			set
			{
				this.At(rowKey, rowDof, columnKey, columnDof, value);
			}
		}

		public NodalDegreeOfFreedomKeyedMatrix Add(NodalDegreeOfFreedomKeyedMatrix other)
		{
			NodalDegreeOfFreedomKeyedMatrix result = this.Clone();
			result.underlyingMatrix = result.underlyingMatrix.Add(other.underlyingMatrix);
			return result;
		}

		/// <summary>
		/// Retrieves the requested element.
		/// </summary>
		/// <param name="rowKey">The row of the element</param>
		/// <param name="columnKey">The column of the element</param>
		/// <returns>The requested element</returns>
		public double At(IFiniteElementNode rowKey, DegreeOfFreedom rowDof, IFiniteElementNode columnKey, DegreeOfFreedom columnDof)
		{
			Tuple<int, int> indices = this.CalculateMatrixIndices(rowKey, rowDof, columnKey, columnDof);

			return this.underlyingMatrix[indices.Item1, indices.Item2];
		}

		private Tuple<int, int> CalculateMatrixIndices(IFiniteElementNode rowKey, DegreeOfFreedom rowDof, IFiniteElementNode columnKey, DegreeOfFreedom columnDof)
		{
			Guard.AgainstNullArgument(rowKey, "rowKey");
			Guard.AgainstNullArgument(rowDof, "rowDof");
			Guard.AgainstNullArgument(columnKey, "columnKey");
			Guard.AgainstNullArgument(columnDof, "columnDof");

			int rowNodeIndex, rowDofIndex, colNodeIndex, colDofIndex;

			try
			{
				rowNodeIndex = this.nodesForRows.IndexOf(rowKey);
			}
			catch (KeyNotFoundException knfe)
			{
				throw new KeyNotFoundException(string.Format("Could not find a row for node {0}", rowKey), knfe);
			}

			try
			{
				rowDofIndex = this.dofForRows.IndexOf(rowDof);
			}
			catch (KeyNotFoundException knfe)
			{
				throw new KeyNotFoundException(string.Format("Could not find a row with a degree of freedom of {0}", rowDof), knfe);
			}

			try
			{
				colNodeIndex = this.nodesForColumns.IndexOf(columnKey);
			}
			catch (KeyNotFoundException knfe)
			{
				throw new KeyNotFoundException(string.Format("Could not find a column with key of {0}", columnKey), knfe);
			}

			try
			{
				colDofIndex = this.dofForColumns.IndexOf(columnDof);
			}
			catch (KeyNotFoundException knfe)
			{
				throw new KeyNotFoundException(string.Format("Could not find a column with a degree of freedom of {0}", columnDof), knfe);
			}

			int rowIndex = rowNodeIndex * this.dofForRows.Count + rowDofIndex;
			int colIndex = colNodeIndex * this.dofForColumns.Count + colDofIndex;
			return new Tuple<int, int>(rowIndex, colIndex);
		}

		/// <summary>
		/// Sets the value of the given element
		/// </summary>
		/// <param name="rowKey">the row of the element</param>
		/// <param name="columnKey">the column of the element</param>
		/// <param name="value">The value to set the element to</param>
		public void At(IFiniteElementNode rowKey, DegreeOfFreedom rowDof, IFiniteElementNode columnKey, DegreeOfFreedom columnDof, double value)
		{
			Tuple<int, int> indices = this.CalculateMatrixIndices(rowKey, rowDof, columnKey, columnDof);

			this.underlyingMatrix[indices.Item1, indices.Item2] = value;
		}

		/// <summary>
		/// Sets all the values to zero.  The row and column keys remain unaffected.
		/// </summary>
		public void Clear()
		{
			//FIXME do we need to clear the keys??
			this.underlyingMatrix.Clear();
		}

		/// <summary>
		/// Clones this matrix
		/// </summary>
		/// <returns>A shallow clone of this matrix</returns>
		public NodalDegreeOfFreedomKeyedMatrix Clone()
		{
			return new NodalDegreeOfFreedomKeyedMatrix(this);
		}

		public double Determinant()
		{
			return this.underlyingMatrix.Determinant();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public NodalDegreeOfFreedomKeyedMatrix Inverse()
		{
			Matrix<double> inverseUnderlyingMatrix = this.underlyingMatrix.Inverse();
			return new NodalDegreeOfFreedomKeyedMatrix(this.nodesForColumns, this.dofForRows, this.nodesForRows, this.dofForColumns, inverseUnderlyingMatrix);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public NodalDegreeOfFreedomKeyedMatrix Multiply(NodalDegreeOfFreedomKeyedMatrix other)
		{
			NodalDegreeOfFreedomKeyCompatibilityValidator kcv = new NodalDegreeOfFreedomKeyCompatibilityValidator(this.nodesForRows, this.dofForRows, this.nodesForColumns, this.dofForColumns);
			kcv.ThrowIfInvalid();

			Matrix<double> multipliedUnderlyingMatrix = this.underlyingMatrix.Multiply(other.underlyingMatrix);
			return new NodalDegreeOfFreedomKeyedMatrix(this.nodesForRows, this.dofForRows, other.nodesForColumns, other.dofForColumns, multipliedUnderlyingMatrix);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scalar"></param>
		/// <returns></returns>
		public NodalDegreeOfFreedomKeyedMatrix Multiply(double scalar)
		{
			NodalDegreeOfFreedomKeyedMatrix result = this.Clone();
			result.underlyingMatrix = result.underlyingMatrix.Multiply(scalar);
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rightSide"></param>
		/// <returns></returns>
		public NodalDegreeOfFreedomKeyedVector Multiply(NodalDegreeOfFreedomKeyedVector rightSide)
		{
			NodalDegreeOfFreedomKeyCompatibilityValidator kcv = new NodalDegreeOfFreedomKeyCompatibilityValidator(this.nodesForColumns, this.dofForColumns, rightSide.Nodes, rightSide.SupportedDegreesOfFreedom);
			kcv.ThrowIfInvalid();

			Vector<double> result = this.underlyingMatrix.Multiply(rightSide.ToVector());
			return new NodalDegreeOfFreedomKeyedVector(this.nodesForRows, this.dofForRows, result);
		}

		public NodalDegreeOfFreedomKeyedMatrix NormalizeRows(int p)
		{
			NodalDegreeOfFreedomKeyedMatrix result = this.Clone();
			result.underlyingMatrix = result.underlyingMatrix.NormalizeRows(p);
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public NodalDegreeOfFreedomKeyedMatrix Transpose()
		{
			var result = this.Clone();
			result.underlyingMatrix = result.underlyingMatrix.Transpose();
			SwapRowsAndColumns(result);

			return result;
		}

		/// <summary>
		/// Swaps the rows and column keys, but does not alter the underlying matrix.
		/// </summary>
		/// <returns>A matrix with swapped rows and column keys</returns>
		/// <param name="matrixToSwap">Matrix to swap.</param>
		protected static void SwapRowsAndColumns(NodalDegreeOfFreedomKeyedMatrix result)
		{
			var tempNodes = new List<IFiniteElementNode>(result.nodesForRows);
			var tempDof = new List<DegreeOfFreedom>(result.dofForRows);
			result.nodesForRows = new List<IFiniteElementNode>(result.nodesForColumns);
			result.dofForRows = new List<DegreeOfFreedom>(result.dofForColumns);
			result.nodesForColumns = tempNodes;
			result.dofForColumns = tempDof;
		}

		public Matrix<double> ToMatrix()
		{
			return this.underlyingMatrix.Clone();
		}

		public NodalDegreeOfFreedomKeyedMatrix TransposeThisAndMultiply(NodalDegreeOfFreedomKeyedMatrix other)
		{
			//TODO check key compatibility
			var result = this.Clone();
			result.underlyingMatrix = result.underlyingMatrix.TransposeThisAndMultiply(other.underlyingMatrix);
			result.nodesForRows = new List<IFiniteElementNode>(result.nodesForColumns);
			result.nodesForColumns = new List<IFiniteElementNode>(other.nodesForColumns);
			return result;
		}

		#region Key and Data initialization
		/// <summary>
		/// Replaces the keys with the provided lists.
		/// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
		/// </summary>
		/// <param name="keysRows">Keys to copy into the RowKeys property</param>
		/// <param name="keysColumns">Keys to copy into the RowColumns property</param>
		private void CheckAndAddKeys(IEnumerable<IFiniteElementNode> rowNodes, IEnumerable<DegreeOfFreedom> rowDofs, IEnumerable<IFiniteElementNode> columnNodes, IEnumerable<DegreeOfFreedom> columnDofs, double initialValueOfAllData = 0)
		{
			Guard.AgainstNullArgument(rowNodes, "rowNodes");
			Guard.AgainstNullArgument(rowDofs, "rowDofs");
			Guard.AgainstNullArgument(columnNodes, "columnNodes");
			Guard.AgainstNullArgument(columnDofs, "columnDofs");

			this.nodesForRows = new List<IFiniteElementNode>(rowNodes);
			this.dofForRows = new List<DegreeOfFreedom>(rowDofs);
			this.nodesForColumns = new List<IFiniteElementNode>(columnNodes);
			this.dofForColumns = new List<DegreeOfFreedom>(columnDofs);

			this.underlyingMatrix = new DenseMatrix(this.RowCount, this.ColumnCount, initialValueOfAllData);
		}
		#endregion


		public override string ToString()
		{
			if (this.underlyingMatrix != null)
			{
				return this.underlyingMatrix.ToString();
			}

			//TODO publish the keys of rows and columns

			return this.GetType().FullName;
		}
	}
}

