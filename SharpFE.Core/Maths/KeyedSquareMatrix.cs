//-----------------------------------------------------------------------
// <copyright file="KeyedMatrix.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
//
// Based in part on:
//
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2010 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using MathNet.Numerics.LinearAlgebra.Generic;

    /// <summary>
    /// A KeyedMatrix is a matrix whose elements can be accessed by Keys, rather than just index integers.
    /// This is roughly analagous to what a Dictionary is to a List.
    /// </summary>
    /// <typeparam name="TKey">The type of the instances which form the keys to this KeyedMatrix</typeparam>
    public class KeyedSquareMatrix<TKey> : KeyedRowColumnMatrix<TKey, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keys">The keys which will be used to look up rows and columns of this square matrix. One unique key is expected per row.</param>
        public KeyedSquareMatrix(IList<TKey> keys)
            : base(keys, keys)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        public KeyedSquareMatrix(IList<TKey> keysForRows, IList<TKey> keysForColumns)
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
        public KeyedSquareMatrix(IList<TKey> keysForRows, IList<TKey> keysForColumns, double initialValueOfAllElements)
            : base(keysForRows, keysForColumns, initialValueOfAllElements)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        /// <param name="matrix">The value to which we assign to each element of the matrix</param>
        public KeyedSquareMatrix(IList<TKey> keysForRows, IList<TKey> keysForColumns, KeyedSquareMatrix<TKey> matrix)
            : base(keysForRows, keysForColumns, matrix)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="keysForRows">The keys which will be used to look up rows of this matrix. One unique key is expected per row.</param>
        /// <param name="keysForColumns">The keys which will be used to look up columns of this matrix. One unique key is expected per column.</param>
        /// <param name="matrix">The value to which we assign to each element of the matrix</param>
        public KeyedSquareMatrix(IList<TKey> keysForRows, IList<TKey> keysForColumns, Matrix<double> matrix)
            : base(keysForRows, keysForColumns, matrix)
        {
            // empty
        }
        
        public KeyedSquareMatrix(KeyedRowColumnMatrix<TKey, TKey> matrix)
            : base(matrix)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedMatrix{TKey}" /> class.
        /// </summary>
        /// <param name="matrix">The matrix which holds the keys and data to copy into this new matrix</param>
        public KeyedSquareMatrix(KeyedSquareMatrix<TKey> matrix)
            : base(matrix)
        {
            // empty
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods", Justification = "hiding base method avoids the need for calling members to cast")]
        public KeyedSquareMatrix<TKey> Multiply(KeyedRowColumnMatrix<TKey, TKey> other)
        {
            KeyedRowColumnMatrix<TKey, TKey> result = base.Multiply(other);
            return new KeyedSquareMatrix<TKey>(result);
        }
        
        public new KeyedSquareMatrix<TKey> Multiply(double scalar)
        {
            KeyedRowColumnMatrix<TKey, TKey> result = base.Multiply(scalar);
            return new KeyedSquareMatrix<TKey>(result);
        }
        
        public KeyedSquareMatrix<TKey> Add(KeyedSquareMatrix<TKey> other)
        {
            KeyedRowColumnMatrix<TKey, TKey> result = base.Add(other);
            return new KeyedSquareMatrix<TKey>(result);
        }
        
        /// <summary>
        /// Clones this matrix
        /// </summary>
        /// <returns>A shallow clone of this matrix</returns>
        public new KeyedSquareMatrix<TKey> Clone()
        {
            return new KeyedSquareMatrix<TKey>(this);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new KeyedSquareMatrix<TKey> Inverse()
        {
            KeyedRowColumnMatrix<TKey, TKey> result = base.Inverse();
            return new KeyedSquareMatrix<TKey>(result);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new KeyedSquareMatrix<TKey> Transpose()
        {
            KeyedRowColumnMatrix<TKey, TKey> result = base.Transpose();
            return new KeyedSquareMatrix<TKey>(result);
        }
        
        public KeyedSquareMatrix<TKey> TransposeThisAndMultiply(KeyedSquareMatrix<TKey> other)
        {
            KeyedRowColumnMatrix<TKey, TKey> result = base.TransposeThisAndMultiply(other);
            return new KeyedSquareMatrix<TKey>(result);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p", Justification = "Following Math.net library convention")]
        public new KeyedSquareMatrix<TKey> NormalizeRows(int p)
        {
            KeyedRowColumnMatrix<TKey, TKey> result = base.NormalizeRows(p);
            return new KeyedSquareMatrix<TKey>(result);
        }
        
        /// <summary>
        /// Creates a matrix which contains values from the requested sub-matrix
        /// </summary>
        /// <param name="rowsToInclude">A list of the keys of rows to include in the new matrix</param>
        /// <param name="columnsToInclude">A list of the keys of columns to include in the new matrix</param>
        /// <returns>A KeyedMatrix which contains values from the requested sub-matrix</returns>
        public KeyedSquareMatrix<TKey> SubMatrix(IList<TKey> keysToInclude)
        {
            KeyedSquareMatrix<TKey> subMatrix = new KeyedSquareMatrix<TKey>(keysToInclude);
            
            foreach (TKey rowKey in keysToInclude) //rows
            {
                foreach (TKey columnKey in keysToInclude) //columns
                {
                    subMatrix.At(rowKey, columnKey, this.At(rowKey, columnKey));
                }
            }
            
            return subMatrix;
        }
        
        public override string ToString()
		{
			return base.ToString();
		}
    }
}
