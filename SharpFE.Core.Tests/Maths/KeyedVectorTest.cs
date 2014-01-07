//-----------------------------------------------------------------------
// <copyright file="KeyedVectorTest.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpFE.Maths;

namespace SharpFE.Core.Tests.Maths
{
    [TestFixture]
    public class KeyedVectorTest
    {
        IList<int> keys;
        
        [SetUp]
        public void SetUp()
        {
            keys = new List<int>(3){ 1, 4, 9 };
        }
        
        [Test]
        public void Constructor_StoresKeysAndSetsAllValuesToZero()
        {
            KeyedVector<int> SUT = new KeyedVector<int>(keys);
            Assert.IsNotNull(SUT);
            IList<int> storedKeys = SUT.Keys;
            Assert.IsTrue(storedKeys.Contains(1));
            Assert.IsTrue(storedKeys.Contains(4));
            Assert.IsTrue(storedKeys.Contains(9));
            
            Assert.AreEqual(0, SUT[1]);
            Assert.AreEqual(0, SUT[4]);
            Assert.AreEqual(0, SUT[9]);
        }
        
        [Test]
        public void Constructor_WithInitialValueParameter_SetsAllValuesToTheInitialValue()
        {
            KeyedVector<int> SUT = new KeyedVector<int>(keys, 22);
            
            IList<int> storedKeys = SUT.Keys;
            Assert.IsTrue(storedKeys.Contains(1));
            Assert.IsTrue(storedKeys.Contains(4));
            Assert.IsTrue(storedKeys.Contains(9));
            
            Assert.AreEqual(22, SUT[1]);
            Assert.AreEqual(22, SUT[4]);
            Assert.AreEqual(22, SUT[9]);
        }
        
        [TestCase(20, 1, 2, 3)]
        [TestCase(29, 2, 3, 4)]
        public void DotProduct_WithOtherKeyedVector_IsComputed(double expected, params double[] otherVectorValues)
        {
            KeyedVector<int> otherVector = new KeyedVector<int>(keys, otherVectorValues);
            KeyedVector<int> SUT = new KeyedVector<int>(keys, 2, 3, 4);
            double result = SUT.DotProduct(otherVector);
            Assert.AreEqual(expected, result);
        }
    }
}
