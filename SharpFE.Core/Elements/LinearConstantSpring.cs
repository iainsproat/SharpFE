//-----------------------------------------------------------------------
// <copyright file="LinearConstantSpring.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using SharpFE.Elements;
    using SharpFE.Stiffness;

    /// <summary>
    /// A spring is a 1D linear element which has a constant stiffness along the local x-axis.
    /// </summary>
    public class LinearConstantSpring : FiniteElement1D, IEquatable<LinearConstantSpring>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantLinearSpring" /> class.
        /// </summary>
        /// <param name="node1">The node at the start of the spring.</param>
        /// <param name="node2">The node at the end of the spring.</param>
        /// <param name="springConstant">The value which defines the constant stiffness of the spring.</param>
        internal LinearConstantSpring(IFiniteElementNode node1, IFiniteElementNode node2, double springConstant)
            : base(node1, node2)
        {
            this.SpringConstant = springConstant;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double SpringConstant
        {
            get;
            private set;
        }
        
        #region Equals and GetHashCode implementation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHandSide"></param>
        /// <param name="rightHandSide"></param>
        /// <returns></returns>
        public static bool operator ==(LinearConstantSpring leftHandSide, LinearConstantSpring rightHandSide)
        {
            if (object.ReferenceEquals(leftHandSide, rightHandSide))
            {
                return true;
            }
            
            if (object.ReferenceEquals(leftHandSide, null) || object.ReferenceEquals(rightHandSide, null))
            {
                return false;
            }
            
            return leftHandSide.Equals(rightHandSide);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHandSide"></param>
        /// <param name="rightHandSide"></param>
        /// <returns></returns>
        public static bool operator !=(LinearConstantSpring leftHandSide, LinearConstantSpring rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as LinearConstantSpring);            
        }
        
        public bool Equals(LinearConstantSpring other)
        {
            if (other == null)
            {
                return false;
            }
            
            return base.Equals(other) && object.Equals(this.SpringConstant, other.SpringConstant);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += base.GetHashCode();
                hashCode += 1000000007 * this.SpringConstant.GetHashCode();
            }
            
            return hashCode;
        }
        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="degreeOfFreedom"></param>
        /// <returns></returns>
        public override bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom)
        {
            switch (degreeOfFreedom)
            {
                case DegreeOfFreedom.X:
                    return true;
                case DegreeOfFreedom.Y:
                case DegreeOfFreedom.Z:
                case DegreeOfFreedom.XX:
                case DegreeOfFreedom.YY:
                case DegreeOfFreedom.ZZ:
                default:
                    return false;
            }
        }
        
        public static bool IsASupportedModelType(ModelType modelType)
        {
            switch(modelType)
            {
               case ModelType.Truss1D:
                case ModelType.Beam1D:
                case ModelType.Truss2D:
                case ModelType.Frame2D:
                case ModelType.Slab2D:
                case ModelType.Membrane2D:
                case ModelType.Truss3D:
                case ModelType.Membrane3D:
                case ModelType.MultiStorey2DSlab:
                case ModelType.Full3D:
                    return true;
                default:
                    throw new NotImplementedException(string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "LinearConstantSpring.IsSupportedModelType(ModelType) has not been defined for a model type of {0}",
                        modelType));
            }
        }
    }
}
