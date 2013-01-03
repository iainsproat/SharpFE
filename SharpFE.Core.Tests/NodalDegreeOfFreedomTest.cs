//-----------------------------------------------------------------------
// <copyright file="FiniteElementNode.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using SharpFE;
using NUnit.Framework;

namespace SharpFE.Core.Tests
{
    [TestFixture]
    public class NodalDegreeOfFreedomTest
    {
        [Test]
        public void AreEqualIfPropertiesAreEqual()
        {
            NodeFactory factory = new NodeFactory(ModelType.Truss1D);
            FiniteElementNode node1 = factory.Create(0);
            NodalDegreeOfFreedom SUT = new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X);
            NodalDegreeOfFreedom equalNodalDegreeOfFreedom = new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X);
            
            Assert.IsTrue(SUT.Equals(equalNodalDegreeOfFreedom));
            Assert.IsTrue(SUT == equalNodalDegreeOfFreedom);
            Assert.IsFalse(SUT != equalNodalDegreeOfFreedom);
            Assert.AreEqual(SUT.GetHashCode(), equalNodalDegreeOfFreedom.GetHashCode());
            
            NodalDegreeOfFreedom notEqualDegreeOfFreedom = new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y);
            
            Assert.IsFalse(SUT.Equals(notEqualDegreeOfFreedom));
            Assert.IsFalse(SUT == notEqualDegreeOfFreedom);
            Assert.IsTrue(SUT != notEqualDegreeOfFreedom);
            Assert.AreNotEqual(SUT.GetHashCode(), notEqualDegreeOfFreedom.GetHashCode());
            
            FiniteElementNode node2 = factory.Create(1);
            NodalDegreeOfFreedom notEqualNode = new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X);
            
            Assert.IsFalse(SUT.Equals(notEqualNode));
            Assert.IsFalse(SUT == notEqualNode);
            Assert.IsTrue(SUT != notEqualNode);
            Assert.AreNotEqual(SUT.GetHashCode(), notEqualNode.GetHashCode());
        }
    }
}
