using System.Linq;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.UnitTests.LinearAlgebraTests.Keyed.Double
{
    using LinearAlgebra.Keyed.Double;
    using LinearAlgebra.Keyed.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class KeyedMatrixStructureTheory : KeyedMatrixStructureTheory<double>
    {
        [Datapoints]
        KeyedMatrix<double>[] _matrices = new KeyedMatrix<double>[]
            {
                new DenseKeyedMatrix(new[,] {{1d, 1d, 2d}, {1d, 1d, 2d}, {1d, 1d, 2d}}),
                new DenseKeyedMatrix(new[,] {{-1.1d, -2.2d, -3.3d}, {0d, 1.1d, 2.2d}, {-4.4d, 5.5d, 6.6d}}),
                new DenseKeyedMatrix(new[,] {{-1.1d, -2.2d, -3.3d, -4.4d}, {0d, 1.1d, 2.2d, 3.3d}, {1d, 2.1d, 6.2d, 4.3d}, {-4.4d, 5.5d, 6.6d, -7.7d}}),
                new DenseKeyedMatrix(new[,] {{-1.1d, -2.2d, -3.3d, -4.4d}, {-1.1d, -2.2d, -3.3d, -4.4d}, {-1.1d, -2.2d, -3.3d, -4.4d}, {-1.1d, -2.2d, -3.3d, -4.4d}}),
                new DenseKeyedMatrix(new[,] {{-1.1d, -2.2d}, {0d, 1.1d}, {-4.4d, 5.5d}}),
                new DenseKeyedMatrix(new[,] {{-1.1d, -2.2d, -3.3d}, {0d, 1.1d, 2.2d}}),
                new DenseKeyedMatrix(new[,] {{1d, 2d, 3d}, {2d, 2d, 0d}, {3d, 0d, 3d}}),

                new SparseKeyedMatrix(new[,] {{7d, 1d, 2d}, {1d, 1d, 2d}, {1d, 1d, 2d}}),
                new SparseKeyedMatrix(new[,] {{7d, 1d, 2d}, {1d, 0d, 0d}, {-2d, 0d, 0d}}),
                new SparseKeyedMatrix(new[,] {{-1.1d, 0d, 0d}, {0d, 1.1d, 2.2d}}),

                new DiagonalKeyedMatrix(3, 3, new[] {1d, -2d, 1.5d}),
                new DiagonalKeyedMatrix(3, 3, new[] {1d, 0d, -1.5d}),

                new UserDefinedKeyedMatrix(new[,] {{0d, 1d, 2d}, {-1d, 7.7d, 0d}, {-2d, 0d, 0d}})
            };

        [Datapoints]
        double[] _scalars = new[] {2d, -1.5d, 0d};

        protected override KeyedMatrix<double> CreateDenseZero(int rows, int columns)
        {
            return new DenseKeyedMatrix(rows, columns);
        }

        protected override KeyedMatrix<double> CreateDenseRandom(int rows, int columns, int seed)
        {
            var dist = new Normal {RandomSource = new MersenneTwister(seed)};
            return new DenseKeyedMatrix(rows, columns, dist.Samples().Take(rows*columns).ToArray());
        }

        protected override KeyedMatrix<double> CreateSparseZero(int rows, int columns)
        {
            return new SparseKeyedMatrix(rows, columns);
        }

        protected override KeyedVector<double> CreateVectorZero(int size)
        {
            return new DenseKeyedVector(size);
        }

        protected override KeyedVector<double> CreateVectorRandom(int size, int seed)
        {
            var dist = new Normal {RandomSource = new MersenneTwister(seed)};
            return new DenseKeyedVector(dist.Samples().Take(size).ToArray());
        }

        protected override double Zero
        {
            get { return 0d; }
        }
    }
}