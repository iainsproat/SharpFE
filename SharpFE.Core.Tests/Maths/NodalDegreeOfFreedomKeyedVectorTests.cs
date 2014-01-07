//-----------------------------------------------------------------------
// <copyright file="NodalDegreeOfFreedomKeyedVectorTest.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Core.Tests.Maths
{
	using System;
	using System.Collections.Generic;
	using NUnit.Framework;
	using SharpFE.Maths;

	[TestFixture]
	public class NodalDegreeOfFreedomKeyedVectorTest
	{
		IList<IFiniteElementNode> nodeKeys;
		IList<DegreeOfFreedom> dofKeys;

		[SetUp]
		public void SetUp()
		{
			nodeKeys = new List<IFiniteElementNode>(3){
				new FiniteElementNode(1), 
				new FiniteElementNode(4),
				new FiniteElementNode(9)
			};

			dofKeys = new List<DegreeOfFreedom>(1) {
				DegreeOfFreedom.X
			};
		}

		[Test]
		public void Constructor_StoresKeysAndSetsAllValuesToZero()
		{
			var SUT = new NodalDegreeOfFreedomKeyedVector(nodeKeys, dofKeys);
			Assert.IsNotNull(SUT);
			var storedNodeKeys = SUT.Nodes;
			Assert.IsTrue(storedNodeKeys.Contains(nodeKeys[0]));
			Assert.IsTrue(storedNodeKeys.Contains(nodeKeys[1]));
			Assert.IsTrue(SUT.SupportedDegreesOfFreedom.Contains(DegreeOfFreedom.X));
			Assert.IsFalse(SUT.SupportedDegreesOfFreedom.Contains(DegreeOfFreedom.Y));

			Assert.AreEqual(0, SUT[nodeKeys[0], DegreeOfFreedom.X]);
			Assert.AreEqual(0, SUT[nodeKeys[1], DegreeOfFreedom.X]);
		}

		[Test]
		public void Constructor_WithInitialValueParameter_SetsAllValuesToTheInitialValue()
		{
			NodalDegreeOfFreedomKeyedVector SUT = new NodalDegreeOfFreedomKeyedVector(nodeKeys, dofKeys, 22);

			Assert.IsTrue(SUT.Nodes.Contains(nodeKeys[0]));

			Assert.AreEqual(22, SUT[nodeKeys[0],DegreeOfFreedom.X]);
			Assert.AreEqual(22, SUT[nodeKeys[1],DegreeOfFreedom.X]);
		}

		[TestCase(20, 1, 2, 3)]
		[TestCase(29, 2, 3, 4)]
		public void DotProduct_WithOtherKeyedVector_IsComputed(double expected, params double[] otherVectorValues)
		{
			NodalDegreeOfFreedomKeyedVector otherVector = new NodalDegreeOfFreedomKeyedVector(nodeKeys, dofKeys, otherVectorValues);
			NodalDegreeOfFreedomKeyedVector SUT = new NodalDegreeOfFreedomKeyedVector(nodeKeys, dofKeys, 2, 3, 4);
			double result = SUT.DotProduct(otherVector);
			Assert.AreEqual(expected, result);
		}
	}
}
