//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharpFE.Stiffness
{
    /// <summary>
    /// Description of ElementStiffnessMatrixFactory.
    /// </summary>
    public class ElementStiffnessMatrixBuilderFactory
    {
        private IDictionary<Type, Type> lookup = new Dictionary<Type, Type>();
        
        public ElementStiffnessMatrixBuilderFactory()
        {
            this.RegisterTypes();
        }
        
        private void RegisterTypes()
        {
            this.lookup.Add(typeof(LinearConstantSpring), typeof(LinearTrussStiffnessMatrixBuilder));
            this.lookup.Add(typeof(LinearTruss), typeof(LinearTrussStiffnessMatrixBuilder));
            this.lookup.Add(typeof(Linear1DBeam), typeof(Linear1DBeamStiffnessMatrixBuilder));
            this.lookup.Add(typeof(Linear3DBeam), typeof(Linear3DBeamStiffnessMatrixBuilder));
            this.lookup.Add(typeof(LinearConstantStrainTriangle), typeof(LinearConstantStrainTriangleStiffnessMatrixBuilder));
        }
        
        public IStiffnessProvider Create<T>(T element)
            where T : FiniteElement
        {
            Guard.AgainstNullArgument(element, "element");
            
            if (!this.lookup.ContainsKey(element.GetType()))
            {
                throw new ArgumentException(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Factory has not registered a builder for the element type {0}",
                    element.GetType().FullName));
            }
            
            Type builderType = this.lookup[element.GetType()];
            object[] parameters = new object[] {
                element
            };
            object builder = Activator.CreateInstance(builderType, parameters);
            if (builder == null)
            {
                throw new InvalidOperationException(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Failed to create an instance of type {0}. Most likely as the type does not implement a constructor expecting a single parameter of type FiniteElement",
                    builderType.FullName));
            }
            
            IStiffnessProvider castBuilder = builder as IStiffnessProvider;
            if (castBuilder == null)
            {
                throw new InvalidOperationException(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Tried to cast to IStiffnessMatrixBuilder but it is null.  Most likely because the registered builder, {0}, for element type {1} does not inherit from IStiffnessMatrixBuilder",
                    builderType.FullName,
                    element.GetType().FullName));
            }
            
            return castBuilder;
        }
    }
}
