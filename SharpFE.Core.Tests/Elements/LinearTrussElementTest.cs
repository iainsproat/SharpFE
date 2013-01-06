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
		private NodeFactory nodeFactory;
		private ElementFactory elementFactory;
		private FiniteElementNode start;
		private FiniteElementNode end;
		private GenericElasticMaterial material;
		private SolidRectangle section;
		private LinearTruss SUT;
		
		[SetUp]
		public void SetUp()
		{
			nodeFactory = new NodeFactory(ModelType.Truss1D);
			start = nodeFactory.Create(0);
			end = nodeFactory.Create(1);
			elementFactory = new ElementFactory();
			material = new GenericElasticMaterial(0, 0.1, 0, 0);
			section = new SolidRectangle(0.1, 1);
			SUT = elementFactory.CreateLinearTruss(start, end, material, section);
		}
		
		[Test]
		public void It_can_be_constructed()
		{
			Assert.IsNotNull(SUT);
			Assert.AreEqual(this.material, SUT.Material);
			Assert.AreEqual(this.section, SUT.CrossSection);
		}
		
		[Test]
        public void Equality_depends_on_material_and_CrossSection()
        {
        	Assert.IsTrue(SUT.Equals(SUT));
        	
        	LinearTruss equal = elementFactory.CreateLinearTruss(start, end, material, section);
        	Assert.IsTrue(SUT.Equals(equal));
        	
        	GenericElasticMaterial material2 = new GenericElasticMaterial(0, 2, 0, 0);
        	LinearTruss unequalMaterial = elementFactory.CreateLinearTruss(start, end, material2, section);
        	Assert.IsFalse(SUT.Equals(unequalMaterial));
        	
        	SolidRectangle section2 = new SolidRectangle(2, 1);
        	LinearTruss unequalCrossSection = elementFactory.CreateLinearTruss(start, end, material, section2);
        	Assert.IsFalse(SUT.Equals(unequalCrossSection));
        }
        
        [Test]
        public void HashCode_changes_with_Material_and_CrossSection()
        {
        	int SUTOriginalHash = SUT.GetHashCode();
        	
        	LinearTruss equal = elementFactory.CreateLinearTruss(start, end, material, section);
        	Assert.AreEqual(SUTOriginalHash, equal.GetHashCode());
        	
        	GenericElasticMaterial material2 = new GenericElasticMaterial(0, 2, 0, 0);
        	LinearTruss unequalMaterial = elementFactory.CreateLinearTruss(start, end, material2, section);
        	Assert.AreNotEqual(SUTOriginalHash, unequalMaterial.GetHashCode());
        	
        	SolidRectangle section2 = new SolidRectangle(2, 1);
        	LinearTruss unequalCrossSection = elementFactory.CreateLinearTruss(start, end, material, section2);
        	Assert.AreNotEqual(SUTOriginalHash, unequalCrossSection.GetHashCode());
        }
	}
}
