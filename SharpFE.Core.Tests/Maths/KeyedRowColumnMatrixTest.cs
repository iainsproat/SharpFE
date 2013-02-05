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
            colKeys = new List<string>{"x", "y", "z"};
        }
        
        [Test]
        public void Can_be_constructed_with_keys()
        {
            KeyedRowColumnMatrix<string, string> SUT = new KeyedRowColumnMatrix<string, string>(rowKeys, colKeys);
        }
    }
}
