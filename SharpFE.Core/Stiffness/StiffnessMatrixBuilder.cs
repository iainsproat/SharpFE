namespace SharpFE.Stiffness
{
	using System;
	using System.Collections.Generic;
	using MathNet.Numerics.LinearAlgebra.Double;
	using MathNet.Numerics.LinearAlgebra.Generic;
	using SharpFE;
	
	public abstract class StiffnessMatrixBuilder : IStiffnessMatrixBuilder
	{
		/// <summary>
		/// The global stiffness matrix of this element
		/// </summary>
		private StiffnessMatrix stiffnessMatrix;
		
		private int hashAtWhichElementStiffnessMatrixWasLastBuilt;
		
		public StiffnessMatrixBuilder(FiniteElement finiteElement)
		{
			Guard.AgainstNullArgument(finiteElement, "finiteElement");
			this.Element = finiteElement;
			
			this.HasBeenInitialized = true;
		}
		
		public FiniteElement Element
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the stiffness matrix of this element.
		/// </summary>
		public StiffnessMatrix GlobalStiffnessMatrix
		{
			get
			{
				if (this.Element.IsDirty(this.hashAtWhichElementStiffnessMatrixWasLastBuilt))
				{
					this.BuildGlobalStiffnessMatrix();
				}
				
				return this.stiffnessMatrix;
			}
			
			private set
			{
				this.stiffnessMatrix = value;
			}
		}
		
		protected bool HasBeenInitialized
		{
			get;
			private set;
		}
		
		public abstract KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> GetShapeFunctionVector(FiniteElementNode location);
		public abstract KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> GetStrainDisplacementMatrix();
		public abstract StiffnessMatrix GetLocalStiffnessMatrix();
		
		/// <summary>
		/// Gets the exact stiffness value for a given node and degree of freedom combinations.
		/// </summary>
		/// <param name="rowNode">The node defining the row (force equations)</param>
		/// <param name="rowDegreeOfFreedom">The degree of freedom defining the row (force equations)</param>
		/// <param name="columnNode">The node defining the column (displacement equations)</param>
		/// <param name="columnDegreeOfFreedom">the degree of freedom defining the column (displacement equations)</param>
		/// <returns>A value representing the stiffness at the given locations</returns>
		/// <exception cref="ArgumentException">Thrown if either of the nodes is not part of this element, or either of the degrees of freedom are not supported by this element.</exception>
		public double GetGlobalStiffnessAt(FiniteElementNode rowNode, DegreeOfFreedom rowDegreeOfFreedom, FiniteElementNode columnNode, DegreeOfFreedom columnDegreeOfFreedom)
		{
			if (rowNode == null)
			{
				throw new ArgumentNullException("rowNode");
			}
			
			if (columnNode == null)
			{
				throw new ArgumentNullException("columnNode");
			}
			
			if (this.Element.IsDirty(this.hashAtWhichElementStiffnessMatrixWasLastBuilt))
			{
				this.BuildGlobalStiffnessMatrix();
			}
			
			return this.GlobalStiffnessMatrix.At(rowNode, rowDegreeOfFreedom, columnNode, columnDegreeOfFreedom);
		}
		
		/// <summary>
		/// Prepares and generates the stiffness matrix.
		/// It creates an entirely new matrix from the current set of nodes and the supported degrees of freedom of this element.
		/// It calls the GenerateStiffnessMatrix method which inheriting classes are expected to implement.
		/// It sets the stiffnessMatrixHasBeenGenerated flag to true.
		/// </summary>
		public void BuildGlobalStiffnessMatrix()
		{
			this.ThrowIfNotInitialized();
			
			Matrix k = this.GetLocalStiffnessMatrix();
			Matrix t = this.BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates();
			
			//FIXME these multiplications assume the keys of both matrices are ordered and identical
			Matrix kt = (Matrix)k.Multiply(t); // K*T
			Matrix ttransposed = (Matrix)t.Transpose(); // T^
			Matrix ttransposedkt = (Matrix)ttransposed.Multiply(kt); // (T^)*K*T
			this.stiffnessMatrix = new StiffnessMatrix(ttransposedkt, this.Element.SupportedNodalDegreeOfFreedoms, this.Element.SupportedNodalDegreeOfFreedoms);
			
			this.hashAtWhichElementStiffnessMatrixWasLastBuilt = this.Element.GetHashCode();
		}
		
		/// <summary>
		/// Builds the rotational matrix from local coordinates to global coordinates.
		/// </summary>
		private Matrix BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates()
		{
			this.ThrowIfNotInitialized();
			
			Matrix rotationMatrix = this.CalculateElementRotationMatrix();
			Matrix identityMatrix = DenseMatrix.Identity(3);
			
			Matrix elementRotationMatrixFromLocalToGlobalCoordinates = new KeyedMatrix<NodalDegreeOfFreedom>(this.Element.SupportedNodalDegreeOfFreedoms);

			int numberOfNodes = this.Element.Nodes.Count; //assumes no duplicate nodes
			
			for (int i = 0; i < numberOfNodes; i++)
			{
				elementRotationMatrixFromLocalToGlobalCoordinates.SetSubMatrix(i*6, 3, i*6, 3, rotationMatrix);
				elementRotationMatrixFromLocalToGlobalCoordinates.SetSubMatrix(i*6+3, 3, i*6+3, 3, identityMatrix);
			}
			
			return elementRotationMatrixFromLocalToGlobalCoordinates;
		}
		
		public Matrix CalculateElementRotationMatrix()
		{
			this.ThrowIfNotInitialized();
			
			Matrix rotationMatrix = (Matrix)DenseMatrix.CreateFromRows(new List<Vector<double>>(3) { this.Element.LocalXAxis, this.Element.LocalYAxis, this.Element.LocalZAxis });
			rotationMatrix = (Matrix)rotationMatrix.NormalizeRows(2);
			return rotationMatrix;
		}
		
		protected T CastElementTo<T>() where T : FiniteElement
		{
			T convertedElement = this.Element as T;
			if (convertedElement == null)
			{
				throw new InvalidCastException(String.Format(
					"We expected the element of type {0} to be convertable to a type {1}, but it didn't work.",
					this.Element.GetType().FullName,
					typeof(T).FullName));
			}
			
			return convertedElement;
		}
		
		protected IMaterial GetMaterial()
		{
			IHasMaterial hasMaterial = this.Element as IHasMaterial;
			if (hasMaterial == null)
			{
				throw new NotImplementedException("StiffnessMatrixBuilder expects finite elements to implement IHasMaterial");
			}
			
			return hasMaterial.Material;
		}
		
		protected ICrossSection GetCrossSection()
		{
			IHasConstantCrossSection hasCrossSection = this.Element as IHasConstantCrossSection;
			if (hasCrossSection == null)
			{
				throw new NotImplementedException("StiffnessMatrixBuilder expects finite elements to implement IHasConstantCrossSection");
			}
			
			return hasCrossSection.CrossSection;
		}
		
		protected void ThrowIfNotInitialized()
		{
			if(!this.HasBeenInitialized)
			{
				throw new InvalidOperationException("This StiffnessMatrixBuilder has not been initialized correctly.  The Initialize(FiniteElement) method should be called first.");
			}
		}
	}
}
