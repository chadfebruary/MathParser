﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace dab.Library.UnitTests.MathParserUnitTest
{
    [TestClass]
    public class ExpressionTests
    {
        MathParser.MathParser mp = new MathParser.MathParser("", "");

        [TestMethod]
        public void EmptyExpression()
        {
            Assert.AreEqual(0, mp.Evaluate("").Value);
        }


        [TestMethod]
        public void GetInterpretationExpression()
        {
            var eval = mp.GetEvaluation("Sin(ASin(Tan(Atan(Cos(acos((sqrt(4)^2)/4))))))");

            Assert.AreEqual("Sin(ASin(Tan(ATan(Cos(ACos((Sqrt(4) ^ 2) / 4))))))", eval.GetInterpretation());
        }

        [TestMethod]
        public void OnePlusOne()
        {
            Assert.AreEqual(2, mp.Evaluate("1 + 1").Value);
        }

        [TestMethod]
        public void SingleNumber()
        {
            Assert.AreEqual(45, mp.Evaluate("45").Value);

            Assert.AreEqual("45", mp.GetEvaluation("45 as decimal").GetInterpretation());
        }
        [TestMethod]
        public void SingleNumberCommas()
        {
            Assert.AreEqual(4500, mp.Evaluate("4,500").Value);
            Assert.AreEqual("4,500", mp.GetInterpretation("4,500"));
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidMathExpressionException))]
        public void InvalidSymbol()
        {
            try
            {
                Assert.AreEqual(45, mp.Evaluate("a").Value);
            }
            catch (InvalidMathExpressionException ex)
            {
                Assert.AreEqual("The following math expression is invalid: a", ex.Message);
                Assert.AreEqual("a", ex.Expression);
                throw;
            }
            
        }

        [TestMethod]
        public void PI()
        {
            Assert.AreEqual((decimal)Math.PI, mp.Evaluate("PI").Value);
        }

        [TestMethod]
        public void E()
        {
            Assert.AreEqual((decimal)Math.E, mp.Evaluate("E").Value);
        }

        [TestMethod]
        public void OnePlusOneInParenthesis()
        {
            Assert.AreEqual(2, mp.Evaluate("(((1) + (1)))").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidMathExpressionException))]
        public void OnePlusOneInNonMatchingParenthesisLeft()
        {
            Assert.AreEqual(2, mp.Evaluate("((1) + (1)))").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidMathExpressionException))]
        public void OnePlusOneInNonMatchingParenthesisRight()
        {
            Assert.AreEqual(2, mp.Evaluate("((1) + (1)").Value);
        }

        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(10, mp.Evaluate("2 * 5").Value);
        }

        [TestMethod]
        public void SubtractionAndNegative()
        {
            Assert.AreEqual(-3, mp.Evaluate("2 - 5").Value);
            Assert.AreEqual(3, mp.Evaluate("5 -2").Value);

            Assert.AreEqual("5 - 2", mp.GetInterpretation("5 -2"));
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(2, mp.Evaluate("10 / 5").Value);
            Assert.AreEqual(10M / 3M, mp.Evaluate("10 / 3").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(MathParser.DivideByZeroException))]
        public void DivisionByZero()
        {
            mp.Evaluate("1/0");
        }

        [TestMethod]
        public void BitwiseOperators()
        {
            Assert.AreEqual(-2, mp.Evaluate("~1").Value);
            Assert.AreEqual(3, mp.Evaluate("1 | 2").Value);
            Assert.AreEqual(0, mp.Evaluate("1 & 2").Value);
            Assert.AreEqual(4, mp.Evaluate("1 << 2").Value);
            Assert.AreEqual(1, mp.Evaluate("2 >> 1").Value);
        }

        [TestMethod]
        public void BitwiseOperatorsDisplay()
        {
            Assert.AreEqual("-2", mp.Evaluate("~1").ToString());
            Assert.AreEqual("3", mp.Evaluate("1 | 2").ToString());
            Assert.AreEqual("0", mp.Evaluate("1 & 2").ToString());
            Assert.AreEqual("4", mp.Evaluate("1 << 2").ToString());
            Assert.AreEqual("1", mp.Evaluate("2 >> 1").ToString());
        }

        [TestMethod]
        public void BitwiseOperatorsInterpretationDisplay()
        {
            Assert.AreEqual("~(1)", mp.GetInterpretation("~1"));
            Assert.AreEqual("1 | 2", mp.GetInterpretation("1 | 2"));
            Assert.AreEqual("1 & 2", mp.GetInterpretation("1 & 2"));
            Assert.AreEqual("1 << 2", mp.GetInterpretation("1 << 2"));
            Assert.AreEqual("2 >> 1", mp.GetInterpretation("2 >> 1"));
        }

        [TestMethod]
        public void ModulusOperator()
        {
            Assert.AreEqual(1, mp.Evaluate("4 % 3").Value);
        }

        [TestMethod]
        public void OrderOfOperation()
        {
            Assert.AreEqual(14, mp.Evaluate("4 + 5 * 2").Value);
            Assert.AreEqual(18, mp.Evaluate("(4 + 5) * 2").Value);
            Assert.AreEqual(36, mp.Evaluate("(4 + 5) * 2^2").Value);

            // Test for left to right operation of similiar valued operations
            Assert.AreEqual(116, mp.Evaluate("123-4-8+5").Value);
            Assert.AreEqual(106, mp.Evaluate("123-4-8-5").Value);
            Assert.AreEqual(124, mp.Evaluate("123+4-8+5").Value);
            Assert.AreEqual(((decimal)49.20), mp.Evaluate("123/4*8/5").Value);
            Assert.AreEqual(((decimal)307.5), mp.Evaluate("123*4/8*5").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidOperatorException))]
        public void InvalidExpressionOperatorBefore()
        {
            mp.Evaluate("+ 1 1");
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidOperatorException))]
        public void InvalidExpressionOperatorAfter()
        {
            mp.Evaluate("1 1 +");
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void InvalidExpressionOperatorAfter2()
        {
            mp.Evaluate("1 + 1 -");
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidOperatorException))]
        public void InvalidExpressionDoubleOperator()
        {
            mp.Evaluate("1 + * 2");
        }

        [TestMethod]
        public void UsingNegativeSign()
        {
            Assert.AreEqual(0, mp.Evaluate("1 + - 1").Value);
        }

        [TestMethod]
        public void UsingNegativeSignNoSpace()
        {
            Assert.AreEqual(0, mp.Evaluate("1 + -1").Value);
        }

        [TestMethod]
        public void ValidFunctionParen()
        {
            Assert.AreEqual(4, mp.Evaluate("sqrt(16)").Value);
            Assert.IsTrue(mp.GetInterpretation("sqrt(16)").StartsWith("Sqrt(16)"));
        }
        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidMathExpressionException))]
        public void InvalidFunctionSpace()
        {
            Assert.AreEqual(4, mp.Evaluate("sqrt 16").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidMathExpressionException))]
        public void InvalidFunctionNoParamSeparator()
        {
            mp.Evaluate("sqrt16"); // not valid
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidMathExpressionException))]
        public void InvalidFunction()
        {
            Assert.AreEqual(4, mp.Evaluate("sqrtt(16)").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidFunctionArgument))]
        public void InvalidFunctionNoParams()
        {
            Assert.AreEqual(4, mp.Evaluate("sqrtt()").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidMathExpressionException))]
        public void InvalidFunctionBrackets()
        {
            Assert.AreEqual(4, mp.Evaluate("sqrtt[16]").Value);
        }

        [TestMethod]
        public void GetInterpretations()
        {
            Assert.AreEqual("Sin(16)", mp.GetInterpretation("Sin(16)"));

            Assert.AreEqual("(1 Foot) as Inch", mp.GetInterpretation("1 foot as inches"));
        }

        [TestMethod]
        public void GetLargeNumber()
        {
            Assert.AreEqual("1.100000E+016", mp.Evaluate("11000000000000000").ToString());

        }

        [TestMethod]
        public void UsingInForConversion()
        {
            Assert.AreEqual("3,600 Seconds", mp.Evaluate("1 hour in seconds").ToString());
        }

        [TestMethod]
        public void CheckInConversionAndInchDontCollid()
        {
            Assert.AreEqual("12 Inches", mp.Evaluate("1 foot in in").ToString());
        }

        [TestMethod]
        public void RegressionTestForInchesAndInConversion()
        {
            Assert.AreEqual("12 Inches", mp.Evaluate("1 foot to in").ToString());
        }
        [TestMethod]
        public void TestIntegerOverUnderFlowBitShift()
        {
            Assert.AreEqual("4,294,967,296", mp.Evaluate("1 << 32").ToString());
            Assert.AreEqual("0", mp.Evaluate("1 >> 32").ToString());
            Assert.AreEqual("-9,223,372,036,854,775,808", mp.Evaluate("1 << 2147483647").ToString());
            Assert.AreEqual("0", mp.Evaluate("1 >> 2147483647").ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUnitTypeException))]
        public void TestInvalidUnits()
        {
           mp.Evaluate("1 BadUnit to OtherBadUnit");
        }
    }
}
