//-----------------------------------------------------------------------
// <copyright file="LinearTrussElementTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE;

namespace SharpFE.Core.Tests.Elements
{
	[TestFixture]
	public class LinearTrussElementTest
	{
		protected NodeFactory nodeFactory;
		protected ElementFactory elementFactory;
		protected FiniteElementNode start;
		protected FiniteElementNode end;
		protected LinearTruss SUT;
		
		[SetUp]
		public void SetUp()
		{
			nodeFactory = new NodeFactory(ModelType.Truss1D);
			start = nodeFactory.Create(0);
			end = nodeFactory.Create(1);
			elementFactory = new ElementFactory();
			GenericElasticMaterial material = new GenericElasticMaterial(0, 0.1, 0, 0);
			SolidRectangle section = new SolidRectangle(0.1, 1);
			SUT = elementFactory.CreateLinearTruss(start, end, material, section);
		}
		
		[Test]
		public void It_can_be_constructed()
		{
			Assert.IsNotNull(SUT);
		}
	}
}
