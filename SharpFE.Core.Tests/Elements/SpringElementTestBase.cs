//-----------------------------------------------------------------------
// <copyright file="SpringElementTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE.Tests.Elements
{
    /// <summary>
    /// Description of SpringElementTestBase.
    /// </summary>
    public class SpringElementTestBase
    {
        protected NodeFactory nodeFactory;
        protected ElementFactory elementFactory;
        protected FiniteElementNode start;
        protected FiniteElementNode end;
        protected Spring SUT;
        
        public void SetUp()
        {
            nodeFactory = new NodeFactory(ModelType.Truss1D);
            start = nodeFactory.Create(0);
            end = nodeFactory.Create(1);
            elementFactory = new ElementFactory();
            SUT = elementFactory.CreateSpring(start, end, 2);
        }
        
        protected Spring CreateSpringFromOriginTo(double x, double y)
        {
            nodeFactory = new NodeFactory(ModelType.Truss2D);
            start = nodeFactory.Create(0, 0);
            end = nodeFactory.Create(x, y);
            
            elementFactory = new ElementFactory();
            return elementFactory.CreateSpring(start, end, 1);
        }
        
        protected Spring CreateSpringFromOriginTo(double x, double y, double z)
        {
            nodeFactory = new NodeFactory(ModelType.Truss3D);
            start = nodeFactory.Create(0, 0, 0);
            end = nodeFactory.Create(x, y, z);
            
            elementFactory = new ElementFactory();
            return elementFactory.CreateSpring(start, end, 1);
        }
        
        protected void Assert12x12StiffnessMatrix(params double[] expectedValues)
        {
            Helpers.AssertMatrix(SUT.GlobalStiffnessMatrix, 12, 12, expectedValues);
        }
        
        protected void Assert3x3RotationMatrix(params double[] expectedValues)
        {
            Helpers.AssertMatrix(SUT.RotationMatrixFromLocalToGlobal, 3, 3, expectedValues);
        }
    }
}
