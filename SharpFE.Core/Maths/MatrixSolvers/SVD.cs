//-----------------------------------------------------------------------
// <copyright file="SVD.cs" company="Iain Sproat">
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
using System;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace SharpFE.Maths.MatrixSolvers
{
    /// <summary>
    /// Description of SVD.
    /// </summary>
    public class Svd<TRowKey, TColumnKey> : IMatrixSolver<TRowKey, TColumnKey>
    {
        private KeyedRowColumnMatrix<TRowKey, TColumnKey> data;
        private Svd _underlyingSvd;
        
        public Svd(KeyedRowColumnMatrix<TRowKey, TColumnKey> matrix, bool computeVectors)
        {
            data = matrix.Clone();
            this._underlyingSvd = new DenseSvd((DenseMatrix)data.ToMatrix(), computeVectors);
        }
        
        public KeyedVector<TColumnKey> Solve(KeyedVector<TRowKey> b)
        {
            Vector<double> solution = this._underlyingSvd.Solve(b.ToVector());
            return new KeyedVector<TColumnKey>(solution.ToArray(), data.ColumnKeys);
        }
    }
}
