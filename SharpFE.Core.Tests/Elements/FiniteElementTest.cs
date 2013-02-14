//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Core.Tests.Elements
{
	/// <summary>
	/// Tests methods on the FiniteElement abstract class
	/// </summary>
	[TestFixture]
	public class FiniteElementTest
	{
		private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode start;
        private FiniteElementNode end;
        private FiniteElement SUT;
        
        [SetUp]
        public void SetUp()
        {
            nodeFactory = new NodeFactory(ModelType.Truss1D);
            start = nodeFactory.Create(0);
            end = nodeFactory.Create(1);
            elementFactory = new ElementFactory();
            SUT = elementFactory.CreateLinearConstantSpring(start, end, 0);
        }
        
        [Test]
        public void It_can_be_constructed()
        {
        	Assert.IsNotNull(SUT);
        }
        
        [Test]
        public void Nodes_have_been_added_correctly()
        {
        	Assert.IsNotNull(SUT.Nodes);
        	Assert.AreEqual(2, SUT.Nodes.Count);
        	Assert.IsTrue(SUT.Nodes.Contains(start));
        	Assert.IsTrue(SUT.Nodes.Contains(end));
        }
        
        [Test]
        public void Can_determine_if_equal()
        {
        	Assert.IsTrue(SUT.Equals(SUT));
        	
        	FiniteElement equal = elementFactory.CreateLinearConstantSpring(start, end, 0);
        	Assert.IsTrue(SUT.Equals(equal));
        	
        	FiniteElementNode otherNode = this.nodeFactory.Create(3);
        	FiniteElement notEqual = elementFactory.CreateLinearConstantSpring(start, otherNode, 0);
        	Assert.IsFalse(SUT.Equals(notEqual));
        }
        
        [Test]
        public void HashCode_depends_only_on_connected_nodes()
        {
        	int SUTOriginalHash = SUT.GetHashCode();
        	
        	FiniteElement equal = elementFactory.CreateLinearConstantSpring(start, end, 0);
        	Assert.AreEqual(SUTOriginalHash, equal.GetHashCode());
        	Assert.IsFalse(SUT.IsDirty(SUTOriginalHash));
        	Assert.IsFalse(SUT.IsDirty(equal.GetHashCode()));
        }
        
        [Test]
        public void HashCode_changes_if_nodes_are_changed()
        {
        	int SUTOriginalHash = SUT.GetHashCode();
        	SUT.RemoveNode(start);
        	Assert.AreEqual(1, SUT.Nodes.Count);
        	Assert.AreNotEqual(SUTOriginalHash, SUT.GetHashCode());
        	Assert.IsTrue(SUT.IsDirty(SUTOriginalHash));
        }
        
        [Test]
        public void ToString_returnsNodeList()
        {
            string result = SUT.ToString();
            Assert.AreEqual("{SharpFE.LinearConstantSpring, [[0, 0, 0], [1, 0, 0]]}", result);
        }
	}
}
