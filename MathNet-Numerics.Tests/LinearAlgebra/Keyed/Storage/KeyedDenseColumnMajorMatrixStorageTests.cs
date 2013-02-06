//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

using MathNet.Numerics.LinearAlgebra.Keyed.Storage;

namespace MathNet.Numerics.Tests.LinearAlgebra.Keyed.Storage
{
    [TestFixture]
    public class KeyedDenseColumnMajorMatrixStorageTests
    {
        [Test]
        public void TestMethod()
        {
            KeyedDenseColumnMajorMatrixStorage<double> SUT = new KeyedDenseColumnMajorMatrixStorage<double>(1, 1);
        }
    }
}
