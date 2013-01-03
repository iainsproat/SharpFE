namespace SharpFE
{
	using System;

	public class SolidRectangle : ICrossSection
	{
		
		public SolidRectangle(double height, double width)
		{
			if (height <= 0)
			{
				throw new ArgumentException("height has to be a positive, non-zero number");
			}
			
			if (width <= 0)
			{
				throw new ArgumentException("width has to be a positive, non-zero number");
			}
			
			this.MaximumDepth = height;
			this.MaximumWidth = width;
		}
		
		public double MaximumDepth
		{
			get;
			private set;
		}
		
		public double MaximumWidth
		{
			get;
			private set;
		}
		
		public double Area
		{
			get
			{
				return this.MaximumDepth * this.MaximumWidth;
			}
		}
		
		public double SecondMomentOfAreaAroundXX
		{
			get
			{
				return this.MaximumWidth * this.MaximumDepth * this.MaximumDepth * this.MaximumDepth / 12.0;
			}
		}
		
		public double SecondMomentOfAreaAroundYY
		{
			get
			{
				return this.MaximumDepth * this.MaximumWidth * this.MaximumWidth * this.MaximumWidth / 12.0;
			}
		}
		
		public double ExternalPerimeterLength
		{
			get
			{
				return this.MaximumDepth * 2 + this.MaximumWidth * 2;
			}
		}
		
		public double MomentOfInertiaInTorsion
		{
			get
			{
				throw new NotImplementedException("MomentOfInertiaInTorsion");
			}
		}
		
		public Coordinate2D GeometricCentroid
		{
			get
			{
				return new Coordinate2D(0, 0);
			}
		}
	}
}
