// <copyright file="ISolver.cs" company="Math.NET">
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

namespace MathNet.Numerics.LinearAlgebra.Keyed.Generic
{
    using System;
    using System.Numerics;

    /// <summary>
    /// Classes that solves a system of linear equations, <c>AX = B</c>.
    /// </summary>
    /// <typeparam name="T">Supported data types are double, single, <see cref="Complex"/>, and <see cref="Complex32"/>.</typeparam>
    public interface IKeyedSolver<T> where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Solves a system of linear equations, <c>AX = B</c>.
        /// </summary>
        /// <param name="input">The right hand side Matrix, <c>B</c>.</param>
        /// <returns>The left hand side Matrix, <c>X</c>.</returns>
        KeyedMatrix<T> Solve(KeyedMatrix<T> input);

        /// <summary>
        /// Solves a system of linear equations, <c>AX = B</c>.
        /// </summary>
        /// <param name="input">The right hand side Matrix, <c>B</c>.</param>
        /// <param name="result">The left hand side Matrix, <c>X</c>.</param>
        void Solve(KeyedMatrix<T> input, KeyedMatrix<T> result);

        /// <summary>
        /// Solves a system of linear equations, <c>Ax = b</c>
        /// </summary>
        /// <param name="input">The right hand side vector, <c>b</c>.</param>
        /// <returns>The left hand side Vector, <c>x</c>.</returns>
        KeyedVector<T> Solve(KeyedVector<T> input);

        /// <summary>
        /// Solves a system of linear equations, <c>Ax = b</c>.
        /// </summary>
        /// <param name="input">The right hand side vector, <c>b</c>.</param>
        /// <param name="result">The left hand side Matrix>, <c>x</c>.</param>
        void Solve(KeyedVector<T> input, KeyedVector<T> result);
    }
}