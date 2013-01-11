//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpFE.MathNetExtensions;

namespace SharpFE.Core.Tests.Maths
{
    [TestFixture]
    public class KeyCompatibilityValidatorTest
    {
        string item0 = "a";
        string item1 = "b";
        string item2 = "c";
        string item3 = "d";
        
        [Test]
        public void It_will_not_throw_with_equal_ordered_lists()
        {
            IList<string> lhs = new List<string>(3){ item0, item1, item2 };
            IList<string> rhs = new List<string>(3){ item0, item1, item2 };
            KeyCompatibilityValidator<string, string> SUT = new KeyCompatibilityValidator<string, string>(lhs, rhs);
            SUT.ThrowIfInvalid();
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void It_will_throw_with_null_item_in_rhs()
        {
            IList<string> lhs = new List<string>(3){ item0, item1, item2 };
            IList<string> rhs = new List<string>(3){ item0, null, item2 };
            KeyCompatibilityValidator<string, string> SUT = new KeyCompatibilityValidator<string, string>(lhs, rhs);
            try
            {
                SUT.ThrowIfInvalid();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void It_will_throw_with_null_item_in_lhs()
        {
            IList<string> lhs = new List<string>(3){ item0, item1, null };
            IList<string> rhs = new List<string>(3){ item0, item1, item2 };
            KeyCompatibilityValidator<string, string> SUT = new KeyCompatibilityValidator<string, string>(lhs, rhs);
            try
            {
                SUT.ThrowIfInvalid();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void It_will_throw_if_lists_are_of_the_wrong_length()
        {
            IList<string> lhs = new List<string>(3){ item0, item1, item2, item3 };
            IList<string> rhs = new List<string>(3){ item0, item1, item2 };
            KeyCompatibilityValidator<string, string> SUT = new KeyCompatibilityValidator<string, string>(lhs, rhs);
            try
            {
                SUT.ThrowIfInvalid();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void It_will_throw_with_swapped_items()
        {
            IList<string> lhs = new List<string>(3){ item0, item2, item1 };
            IList<string> rhs = new List<string>(3){ item0, item1, item2 };
            KeyCompatibilityValidator<string, string> SUT = new KeyCompatibilityValidator<string, string>(lhs, rhs);
            try
            {
                SUT.ThrowIfInvalid();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void It_will_throw_with_missing_item_lhs()
        {
            IList<string> lhs = new List<string>(3){ item0, item1, item2 };
            IList<string> rhs = new List<string>(3){ item0, item1, item3 };
            KeyCompatibilityValidator<string, string> SUT = new KeyCompatibilityValidator<string, string>(lhs, rhs);
            try
            {
                SUT.ThrowIfInvalid();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void It_will_throw_if_items_shifted()
        {
            IList<string> lhs = new List<string>(3){ item0, item1, item2 };
            IList<string> rhs = new List<string>(3){ item3, item0, item1 };
            KeyCompatibilityValidator<string, string> SUT = new KeyCompatibilityValidator<string, string>(lhs, rhs);
            try
            {
                SUT.ThrowIfInvalid();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
