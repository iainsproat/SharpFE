// <copyright file="KeyedVectorStorage.Validation.cs" company="Iain Sproat">
// Copyright Iain Sproat, 2013.
//
// Parts of this file are copyright and licensed under the following terms:
//
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// Copyright (c) 2009-2010 Math.NET
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

using System;
using MathNet.Numerics.Properties;

namespace MathNet.Numerics.LinearAlgebra.Keyed.Storage
{
    // ReSharper disable UnusedParameter.Global
    public partial class KeyedVectorStorage<T>
    {
        protected void ValidateRange(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        protected void ValidateSubVectorRange(KeyedVectorStorage<T> target,
            int sourceIndex, int targetIndex, int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count", Resources.ArgumentMustBePositive);
            }

            // Verify Source

            if (sourceIndex >= Length || sourceIndex < 0)
            {
                throw new ArgumentOutOfRangeException("sourceIndex");
            }

            var sourceMax = sourceIndex + count;

            if (sourceMax > Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            // Verify Target

            if (targetIndex >= target.Length || targetIndex < 0)
            {
                throw new ArgumentOutOfRangeException("targetIndex");
            }

            var targetMax = targetIndex + count;

            if (targetMax > target.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
        }
    }
    // ReSharper restore UnusedParameter.Global
}