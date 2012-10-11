/*
 * Created by Iain Sproat
 * Date: 28/09/2012
 * Time: 20:31
 * 
 */
using System;
using NUnit.Framework;

using SharpFE;

namespace SharpFE.Tests.Measures
{
    /// <summary>
    /// Description of ForceTest.
    /// </summary>
    [TestFixture]
    public class ForceTest
    {
        ForceFactory forceFactory;
        ForceVector SUT;
        
        [SetUp]
        public void Setup()
        {
            forceFactory = new ForceFactory(ModelType.Truss1D);
            SUT = forceFactory.Create(12);
        }
        
        [Test]
        public void CanQueryForceValue()
        {
            Assert.AreEqual(12, SUT.X);
        }
        
        [Test]
        public void CanGetComponent()
        {
            Assert.AreEqual(12, SUT.GetValue(DegreeOfFreedom.X));
            Assert.AreEqual(0, SUT.GetValue(DegreeOfFreedom.ZZ));
        }
    }
}
