/*
 * Copyright Iain Sproat, 2013
 * User: ispro
 * Date: 03/07/2013
 * 
 */


namespace SharpFE.Core.Tests.Maths
{
    using System;
    using NUnit.Framework;
    using SharpFE;

    [TestFixture]
    public class DoubleExtensionTest
    {
        [Test]
        public void IsApproximatelyEqualTo()
        {
            Assert.IsTrue(23.0.IsApproximatelyEqualTo(23.5, 1.0));
            Assert.IsTrue(23.0.IsApproximatelyEqualTo(22.5, 1.0));
            Assert.IsTrue((-23.0).IsApproximatelyEqualTo(-22.5, 1.0));
            Assert.IsTrue((-23.0).IsApproximatelyEqualTo(-23.5, 1.0));
            Assert.IsFalse(23.0.IsApproximatelyEqualTo(24.0, 1.0));
            Assert.IsFalse(0.0.IsApproximatelyEqualTo(0.0, 0.0));
            Assert.IsTrue(23.0.IsApproximatelyEqualTo(23.5, -1.0));
            Assert.IsTrue((-23.0).IsApproximatelyEqualTo(-22.5, -1.0));
            Assert.IsTrue(double.MaxValue.IsApproximatelyEqualTo(double.MaxValue - 1, 2));
        }
    }
}
