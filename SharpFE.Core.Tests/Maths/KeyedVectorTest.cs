//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
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
        public void Can_be_constructed_from_List_of_keys()
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
        public void Can_be_constructed_with_initial_value()
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
    }
}
