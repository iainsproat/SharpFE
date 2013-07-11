/*
 * Created by Iain Sproat
 * Date: 28/09/2012
 * Time: 14:58
 * 
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpFE.Core.Tests
{
    /// <summary>
    /// Description of Helpers.
    /// </summary>
    public static class Helpers
    {
        public static void AssertMatrix<TRowKey, TColumnKey>(KeyedRowColumnMatrix<TRowKey, TColumnKey> actual, int expectedRowCount, int expectedColumnCount, params double[] expectedValues)
        {
        	
            if(expectedValues == null)
            {
                throw new ArgumentNullException("expectedValues");
            }
            
            if(expectedRowCount * expectedColumnCount != expectedValues.Length)
            {
                throw new ArgumentException(String.Format("You have not provided the correct number of values to check for in the matrix.  " +
                                                          "You provided a expected Row count of {0} and an expected column count of {1}.  " +
                                                          "We would have expected {2} values, but you provided {3}.  The actual matrix has {4} values.\n\r{5}",
                                                          expectedRowCount,
                                                          expectedColumnCount,
                                                          expectedRowCount * expectedColumnCount,
                                                          expectedValues.Length,
                                                          actual.ColumnCount * actual.RowCount,
                                                          actual));
            }
            
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedRowCount, actual.RowCount, "RowCount is not correct");
            Assert.AreEqual(expectedColumnCount, actual.ColumnCount, "ColumnCount");
            
            int currentArrayIndex = 0;
            foreach (TRowKey rowKey in actual.RowKeys)
            {
                foreach (TColumnKey columnKey in actual.ColumnKeys)
                {
                    Assert.AreEqual(expectedValues[currentArrayIndex], actual.At(rowKey, columnKey), 0.0005, String.Format("Row : {0}; Column : {1},\r\n" +
                                                                                                              "Actual : \r\n" +
                                                                                                              "{2}", rowKey, columnKey, PrettyPrintKeyedRowColumnMatrix(actual)));
                    currentArrayIndex++;
                }
            }
        }
        
        public static string PrettyPrintKeyedRowColumnMatrix<TRowKey, TColumnKey>(KeyedRowColumnMatrix<TRowKey, TColumnKey> matrix)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (matrix == null)
            {
                sb.Append("KeyedRowColumnMatrix<");
                sb.Append(typeof(TRowKey).Name);
                sb.Append(", ");
                sb.Append(typeof(TColumnKey).Name);
                sb.Append(">");
                sb.Append(" is null");
            }
            else
            {
                PrettyPrintKeyedRowColumnMatrix(sb, matrix);
            }
            return sb.ToString();
        }
        
        public static void PrettyPrintKeyedRowColumnMatrix<TRowKey, TColumnKey>(System.Text.StringBuilder sb, KeyedRowColumnMatrix<TRowKey, TColumnKey> matrix)
        {
            sb.AppendLine(string.Format("KeyedRowColumnMatrix<{0}, {1}>", typeof(TRowKey).Name, typeof(TColumnKey).Name));
            
            sb.Append("Column Keys : ");
            IList<TColumnKey> columnKeys = matrix.ColumnKeys;
            PrettyPrintList<TColumnKey>(sb, columnKeys);
            
            IList<TRowKey> rowKeys = matrix.RowKeys;
            sb.AppendLine();
            sb.Append("Row Keys : ");
            PrettyPrintList<TRowKey>(sb, rowKeys);
            
            sb.AppendLine();            
            sb.Append(matrix);
        }
        
        public static void PrettyPrintList<T>(System.Text.StringBuilder sb, IList<T> list)
        {
            sb.Append("[");
            int numItems = list.Count;
            for (int i = 0; i < numItems; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                
                sb.AppendLine();
                sb.Append("    ");
                sb.Append(list[i]);
            }
            if (numItems > 0)
            {
                sb.AppendLine();
            }
            sb.Append("]");
        }
    }
}
