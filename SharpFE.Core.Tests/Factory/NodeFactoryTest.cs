//-----------------------------------------------------------------------
// <copyright file="NodeRepositoryTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Tests.Factory
{
    [TestFixture]
    public class NodeFactoryTest
    {
        NodeFactory SUT;
        
        [SetUp]
        public void SetUp()
        {
            this.SUT = new NodeFactory(ModelType.Truss1D);
        }
        
        [Test]
        public void NodesCanBeCreated()
        {
            FiniteElementNode result = SUT.Create(3);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.OriginalX);
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCreateInUnsupportedDimension()
        {
            SUT.Create(0, 0);
        }
        
        [Test]
        public void CanCreateAndAddtoTheRepository()
        {
            NodeRepository repository = new NodeRepository(ModelType.Truss1D);
            this.SUT = new NodeFactory(ModelType.Truss1D, repository);
            Assert.AreEqual(0, repository.Count);
            
            this.SUT.Create(0);
            Assert.AreEqual(1, repository.Count);
        }
    }
}
