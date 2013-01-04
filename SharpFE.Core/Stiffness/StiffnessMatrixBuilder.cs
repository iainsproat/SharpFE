namespace SharpFE.Stiffness
{
	using System;
	using System.Collections.Generic;
	using MathNet.Numerics.LinearAlgebra.Double;
	using MathNet.Numerics.LinearAlgebra.Generic;
	
	public abstract class StiffnessMatrixBuilder : IStiffnessMatrixBuilder
	{
		public StiffnessMatrixBuilder()
		{
			// empty
		}
		
		public void Initialize(FiniteElement finiteElement)
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
		
		protected bool HasBeenInitialized
		{
			get;
			private set;
		}
		
		public abstract KeyedVector<NodalDegreeOfFreedom> GetStrainDisplacementMatrix();
		public abstract ElementStiffnessMatrix GetStiffnessMatrix();
		
		/// <summary>
		/// Prepares and generates the stiffness matrix.
		/// It creates an entirely new matrix from the current set of nodes and the supported degrees of freedom of this element.
		/// It calls the GenerateStiffnessMatrix method which inheriting classes are expected to implement.
		/// It sets the stiffnessMatrixHasBeenGenerated flag to true.
		/// </summary>
		public ElementStiffnessMatrix BuildGlobalStiffnessMatrix() 
		{
			this.ThrowIfNotInitialized();
			
			Matrix k = this.GetStiffnessMatrix();
			Matrix t = this.BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates();
			
			Matrix kt = (Matrix)k.Multiply(t); // K*T
			Matrix ttransposed = (Matrix)t.Transpose(); // T^
			Matrix ttransposedkt = (Matrix)ttransposed.Multiply(kt); // (T^)*K*T
			ElementStiffnessMatrix result = new ElementStiffnessMatrix(ttransposedkt, this.Element.SupportedNodalDegreeOfFreedoms, this.Element.SupportedNodalDegreeOfFreedoms);
			return result;
			
			
		}
		
		/// <summary>
		/// Builds the rotational matrix from local coordinates to global coordinates.
		/// </summary>
		private Matrix BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates()
		{
			this.ThrowIfNotInitialized();
			
			Matrix rotationMatrix = this.CalculateElementRotationMatrix();
			
			Matrix elementRotationMatrixFromLocalToGlobalCoordinates = new ElementStiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);

			int numberOfRowsInRotationMatrix = rotationMatrix.RowCount;
			int numberOfColumnsInRotationMatrix = rotationMatrix.ColumnCount;
			elementRotationMatrixFromLocalToGlobalCoordinates.SetSubMatrix(0, numberOfRowsInRotationMatrix, 0, numberOfColumnsInRotationMatrix, rotationMatrix);
			elementRotationMatrixFromLocalToGlobalCoordinates.SetSubMatrix(6, numberOfRowsInRotationMatrix, 6, numberOfColumnsInRotationMatrix, rotationMatrix);
			return elementRotationMatrixFromLocalToGlobalCoordinates;
		}
		
		public Matrix CalculateElementRotationMatrix()
		{
			this.ThrowIfNotInitialized();
			
			Matrix rotationMatrix = (Matrix)DenseMatrix.CreateFromRows(new List<Vector<double>>(3) { this.Element.LocalXAxis, this.Element.LocalYAxis, this.Element.LocalZAxis });
			rotationMatrix = (Matrix)rotationMatrix.NormalizeRows(2);
			return rotationMatrix;
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
