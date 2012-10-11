//-----------------------------------------------------------------------
// <copyright file="Index.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE;
using System.Collections.Generic;

namespace SharpFE.Tests.Repositories
{
    [TestFixture]
    public class IndexTest
    {
        Index<string, double> SUT;
        
        [SetUp]
        public void SetUp()
        {
            SUT = new Index<string, double>();
        }
        
        [Test]
        public void CanAddMultipleItemsToAKey()
        {
            SUT.Add("hello", 40);
            Assert.AreEqual(1, SUT["hello"].Count);
            Assert.AreEqual(40, SUT["hello"][0]);
            
            SUT.Add("hello", 50);
            Assert.AreEqual(2, SUT["hello"].Count);
            Assert.AreEqual(50, SUT["hello"][1]);
        }
        
        [Test]
        public void CanAddAnItemToMultipleKeys()
        {
            SUT.Add("hello", 30);
            SUT.Add("hello", 40);
            SUT.Add("foo", 30);
            Assert.AreEqual(2, SUT["hello"].Count);
            Assert.AreEqual(1, SUT["foo"].Count);
        }
        
        [Test]
        public void CanRemoveAnItemFromMultipleKeys()
        {
            SUT.Add("hello", 30);
            SUT.Add("hello", 40);
            SUT.Add("foo", 30);
            
            SUT.RemoveValue(30);
            Assert.AreEqual(1, SUT["hello"].Count);
            Assert.AreEqual(0, SUT["foo"].Count);
        }
        
        [Test]
        public void WillNotAddADuplicateItem()
        {
            SUT.Add("hello", 40);
            Assert.AreEqual(1, SUT["hello"].Count);
            
            SUT.Add("hello", 40);
            Assert.AreEqual(1, SUT["hello"].Count);
        }
        
        [Test]
        public void CanAddDuplicatesIfInitializedWithAllowDuplicateFlag()
        {
            SUT = new Index<string, double>(true);
            SUT.Add("hello", 40);
            SUT.Add("hello", 40);
            Assert.AreEqual(2, SUT["hello"].Count);
        }
        
        [Test]
        public void CanAddAListOfValues()
        {
            IList<double> values = new List<double>(4){
                10,
                20,
                30,
                40
            };
            SUT.Add("hello", values);
            Assert.AreEqual(4, SUT["hello"].Count);
        }
        
        [Test]
        public void WillNotAddDuplicateValuesFromAList()
        {
            IList<double> values = new List<double>(4){
                10,
                20,
                30,
                40,
                30 //duplicate
            };
            SUT.Add("hello", values);
            Assert.AreEqual(4, SUT["hello"].Count);
        }
    }
}
