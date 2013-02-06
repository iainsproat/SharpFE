//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpFE.Core.Tests.Maths
{
    [TestFixture]
    public class KeyedRowColumnMatrixTest
    {
        IList<string> rowKeys;
        IList<string> colKeys;
        
        [SetUp]
        public void SetUp()
        {
            rowKeys = new List<string>{"a", "b", "c"};
            colKeys = new List<string>{"x", "y"};
        }
        
        [Test]
        public void Can_be_constructed_with_keys_alone()
        {
            KeyedRowColumnMatrix<string, string> SUT = new KeyedRowColumnMatrix<string, string>(rowKeys, colKeys);
            this.AssertProperties(SUT, 0, 0, 0, 0, 0, 0);
        }
        
        [Test]
        public void Can_be_constructed_with_initial_value()
        {
            KeyedRowColumnMatrix<string, string> SUT = new KeyedRowColumnMatrix<string, string>(rowKeys, colKeys, 33);
            AssertProperties(SUT, 33, 33, 33, 33, 33, 33);
        }
        
        [Test]
        public void It_can_generate_a_sub_matrix()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_get_via_index()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_set_via_index()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_get_data_At()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_set_data_At()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_cleared()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_cloned()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_calculate_determinant()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_inverted()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_scalar_multiplied()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_vector_multiplied()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_matrix_multiplied()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_normalized_by_rows()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_transposed()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void It_can_be_converted_to_a_matrix()
        {
            Assert.Ignore();
        }
        
        private void AssertProperties(KeyedRowColumnMatrix<string, string> SUT, double ax, double ay, double bx, double by, double cx, double cy)
        {
            Assert.IsNotNull(SUT);
            
            Assert.AreEqual(3, SUT.RowCount);
            Assert.AreEqual(2, SUT.ColumnCount);
            
            IList<string> returnedRowKeys = SUT.RowKeys;
            Assert.IsNotNull(returnedRowKeys);
            Assert.IsTrue(returnedRowKeys.Contains("a"));
            Assert.IsTrue(returnedRowKeys.Contains("b"));
            Assert.IsTrue(returnedRowKeys.Contains("c"));
            
            IList<string> returnedColumnKeys = SUT.ColumnKeys;
            Assert.IsNotNull(returnedColumnKeys);
            Assert.IsTrue(returnedColumnKeys.Contains("x"));
            Assert.IsTrue(returnedColumnKeys.Contains("y"));
            
            Assert.AreEqual(ax, SUT["a", "x"], "ax");
            Assert.AreEqual(ay, SUT["a", "y"], "ay");
            Assert.AreEqual(bx, SUT["b", "x"], "bx");
            Assert.AreEqual(by, SUT["b", "y"], "by");
            Assert.AreEqual(cx, SUT["c", "x"], "cx");
            Assert.AreEqual(cy, SUT["c", "y"], "cy");
        }
    }
}
