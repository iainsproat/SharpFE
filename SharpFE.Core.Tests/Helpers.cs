/*
 * Created by Iain Sproat
 * Date: 28/09/2012
 * Time: 14:58
 * 
 */
using System;
using NUnit.Framework;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace SharpFE.Core.Tests
{
    /// <summary>
    /// Description of Helpers.
    /// </summary>
    public static class Helpers
    {
        public static void AssertMatrix(Matrix<double> actual, int expectedRowCount, int expectedColumnCount, params double[] expectedValues)
        {
            if(expectedValues == null)
            {
                throw new ArgumentNullException("expectedValues");
            }
            
            if(expectedRowCount * expectedColumnCount != expectedValues.Length)
            {
                throw new ArgumentException(String.Format("You have not provided the correct number of values to check for in the matrix.  " +
                                                          "You provided a expected Row count of {0} and an expected column count of {1}.  " +
                                                          "We would have expected {2} values, but you provided {3}",
                                                          expectedRowCount,
                                                          expectedColumnCount,
                                                          expectedRowCount * expectedColumnCount,
                                                          expectedValues.Length));
            }
            
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedRowCount, actual.RowCount, "RowCount is not correct");
            Assert.AreEqual(expectedColumnCount, actual.ColumnCount, "ColumnCount");
            
            int currentArrayIndex = 0;
            for (int i = 0; i < expectedRowCount; i++)
            {
                for (int j = 0; j < expectedColumnCount; j++)
                {
                    Assert.AreEqual(expectedValues[currentArrayIndex], actual.At(i, j), 0.0005, String.Format("Row : {0}; Column : {1},\r\n" +
                                                                                                              "Actual : \r\n" +
                                                                                                              "{2}", i, j, actual));
                    currentArrayIndex++;
                }
            }
        }
    }
}
