/*
 * Created by Iain Sproat
 * Date: 25/09/2012
 * Time: 20:49
 * 
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpFE.Core.Tests
{
	[TestFixture]
	public class ResultTest
	{
		FiniteElementModel model;
		FiniteElementNode node;
		
		FiniteElementResults SUT;
			
		[SetUp]
		public void SetUp()
		{
			model = new FiniteElementModel(ModelType.Truss1D);
			node = model.NodeFactory.Create(0);
			
			SUT = new FiniteElementResults(ModelType.Truss1D);
		}
		
		[Test]
		public void ResultsHaveDateTimeOfWhenCreated()
		{
			Assert.IsNotNull(SUT.ResultsCreatedAt);
		}
		
		[Test]
		public void CanGetDisplacementOfNode()
		{
			SUT.AddDisplacement(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), 0.002);
			
			Assert.AreEqual(0.002, SUT.GetDisplacement(node).X);
		}
		
		[Test]
		public void CanGetForceAtNode()
		{
			SUT.AddReaction(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), 22);
			
			Assert.AreEqual(22, SUT.GetReaction(node).X);
		}
		
		[Test]
		public void CanAddMultipleDisplacements()
		{			
			FiniteElementNode node1 = model.NodeFactory.Create(1);
			IList<NodalDegreeOfFreedom> identifiers = new List<NodalDegreeOfFreedom>(3);
			identifiers.Add(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X));
			identifiers.Add(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Y));
			identifiers.Add(new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y));
			
			KeyedVector<NodalDegreeOfFreedom> displacements = new KeyedVector<NodalDegreeOfFreedom>(identifiers);
			displacements[0] = 10;
			displacements[1] = 12;
			displacements[2] = 13;
			
			SUT.AddMultipleDisplacements(displacements);
			
			Assert.AreEqual(10, SUT.GetDisplacement(node).X);
			Assert.AreEqual(12, SUT.GetDisplacement(node).Y);
			Assert.AreEqual(13, SUT.GetDisplacement(node1).Y);
		}
	}
}
