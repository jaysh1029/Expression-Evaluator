﻿using NUnit.Framework;
using Vanderbilt.Biostatistics.Wfccm2;

namespace ExpressionEvaluatorTests
{
    [TestFixture]
    public class PrecedanceTests
    {
        private Expression _func;

        [SetUp]
        public void Init() { _func = new Expression(); }

        [TearDown]
        public void Clear() { _func.Clear(); }

        [Test]
        public void Precedance_AdditionMultiplication_IsCorrect()
        {
            _func.Function = "2 * 3 - 2";
            Assert.AreEqual(4, _func.EvaluateNumeric());
        }

        [Test]
        public void Precedance_AdditionSubtraction_IsCorrect()
        {
            _func.Function = "2 + 3 - 2";
            Assert.AreEqual(3, _func.EvaluateNumeric());
        }

        [Test]
        public void Precedance_MultiplicationAddition_IsCorrect()
        {
            _func.Function = "8 - 2 * 3";
            Assert.AreEqual(2, _func.EvaluateNumeric());
        }

        [Test]
        public void Precedance_SubtractionAddition_IsCorrect()
        {
            _func.Function = "3 - 2 + 2";
            Assert.AreEqual(3, _func.EvaluateNumeric());
        }
    }
}
