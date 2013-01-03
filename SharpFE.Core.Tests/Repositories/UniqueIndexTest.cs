//-----------------------------------------------------------------------
// <copyright file="IndexTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE;

namespace SharpFE.Core.Tests.Repositories
{
    [TestFixture]
    public class UniqueIndexTest
    {
        UniqueIndex<string, double> SUT;
        
        [SetUp]
        public void SetUp()
        {
            SUT = new UniqueIndex<string, double>();
        }
        
        [Test]
        public void CanAddItems()
        {
            SUT.Add("hello", 40);
            Assert.AreEqual(1, SUT["hello"].Count);
            SUT.Add("hello", 50);
            Assert.AreEqual(2, SUT["hello"].Count);
            SUT.Add("foo", 60);
            Assert.AreEqual(2, SUT["hello"].Count);
            Assert.AreEqual(1, SUT["foo"].Count);
        }
        
        [Test]
        public void AddingAnExistingItemToAnotherKeyWillRemoveItFromTheExistingKey()
        {
            SUT.Add("hello", 40);
            SUT.Add("hello", 50);
            SUT.Add("foo", 60);
            Assert.AreEqual(2, SUT["hello"].Count);
            Assert.AreEqual(1, SUT["foo"].Count);
            
            SUT.Add("foo", 50);
            Assert.AreEqual(1, SUT["hello"].Count);
            Assert.AreEqual(2, SUT["foo"].Count);
        }
        
        [Test]
        public void CanFindTheCorrectKeyForAValue()
        {
            SUT.Add("hello", 40);
            SUT.Add("hello", 50);
            SUT.Add("foo", 60);
            Assert.AreEqual("hello", SUT.KeyOfValue(40));
            Assert.AreEqual("foo", SUT.KeyOfValue(60));
            
            // now move a value to another key
            SUT.Add("foo", 40);
            Assert.AreEqual("foo", SUT.KeyOfValue(40));
        }
    }
}
