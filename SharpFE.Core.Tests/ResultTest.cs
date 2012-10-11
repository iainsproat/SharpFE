/*
 * Created by Iain Sproat
 * Date: 25/09/2012
 * Time: 20:49
 * 
 */
using System;
using NUnit.Framework;

namespace SharpFE.Tests
{
    [TestFixture]
    public class ResultTest
    {
        [Test]
        public void ResultsHaveDateTimeOfWhenCreated()
        {
            FiniteElementResults SUT = new FiniteElementResults(ModelType.Truss1D);
            Assert.IsNotNull(SUT.ResultsCreatedAt);
        }
        
        [Test]
        public void CanGetDisplacementOfNode()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss1D);
            FiniteElementNode node = model.NodeFactory.Create(0);
            FiniteElementResults SUT = new FiniteElementResults(ModelType.Truss1D);
            SUT.AddDisplacement(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), 0.002);
            
            Assert.AreEqual(0.002, SUT.GetDisplacement(node).X);
        }
        
        [Test]
        public void CanGetForceAtNode()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss1D);
            FiniteElementNode node = model.NodeFactory.Create(0);
            FiniteElementResults SUT = new FiniteElementResults(ModelType.Truss1D);
            SUT.AddReaction(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), 22);
            
            Assert.AreEqual(22, SUT.GetReaction(node).X);
        }
    }
}
