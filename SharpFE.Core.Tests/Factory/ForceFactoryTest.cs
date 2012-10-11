//-----------------------------------------------------------------------
// <copyright file="ForceRepositoryTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Tests.Factory
{
    [TestFixture]
    public class ForceFactoryTest
    {
        ForceFactory SUT;
        
        [SetUp]
        public void SetUp()
        {
            this.SUT = new ForceFactory(ModelType.Truss1D);
        }
        
        [Test]
        public void CanCreateAForce()
        {
            ForceVector result = SUT.Create(0);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.X);
            Assert.AreEqual(0, result.Y);
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCreateAForceInAnUnsupportedDimensions()
        {
            SUT.Create(0, 0);
        }
        
        [Test]
        public void CanCreateAForceAndAddToTheRepository()
        {
            ForceRepository repository = new ForceRepository();
            this.SUT = new ForceFactory(ModelType.Truss1D, repository);
            Assert.AreEqual(0, repository.Count);
            
            this.SUT.Create(0);
            Assert.AreEqual(1, repository.Count);
        }
    }
}
