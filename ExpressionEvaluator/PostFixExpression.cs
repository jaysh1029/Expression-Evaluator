﻿using System.Collections.Generic;

namespace Vanderbilt.Biostatistics.Wfccm2
{
    public class PostFixExpression
    {
        public PostFixExpression(InfixExpression expression)
        {
            _expression = Infix2Postfix(expression.Expression);
            _tokens = _expression.Split(new [] { ' ' });
            CheckPostVectorForEvaluability(Tokens);

        }

        private string _expression;
        private string[] _tokens;

        public string[] Tokens
        {
            get { return _tokens; }
        }

        public string Expression
        {
            get { return _expression; }
        }

        /// <summary>
        /// Convecs an infix string to a post fix string.
        /// </summary>
        /// <remarks><pre>
        /// 2004-07-19 - Jeremy Roberts
        /// </pre></remarks>
        /// <param name="func">The function to convert</param>
        /// <returns>A post fix string.</returns>
        protected string Infix2Postfix(string func)
        {
            func = new InfixExpression(func).Expression;

            string[] inFix = func.Split(new[] { ' ' });

            var postFix = new Stack<string>();
            var operators = new Stack<string>();

            string currOperator;

            foreach (string token in inFix)
            {
                if (ExpressionKeywords.IsOperand(token))
                {
                    postFix.Push(token);
                }
                else if (ExpressionKeywords.OpenGroupOperators.Contains(token))
                {
                    operators.Push(token);
                }
                else if (ExpressionKeywords.ClosingGroupOperators.Contains(token))
                {
                    Grouping g = ExpressionKeywords.GetGroupingFromClose(token);
                    currOperator = operators.Pop();

                    while (currOperator != g.Name)
                    {
                        postFix.Push(currOperator);
                        currOperator = operators.Pop();
                    }
                }
                else if (ExpressionKeywords.IsOperator(token))
                {
                    // while precedence of the operator is <= precedence of the token 
                    while (operators.Count > 0)
                    {
                        if (ExpressionKeywords.GetPrecedence(token) <= ExpressionKeywords.GetPrecedence(operators.Peek()))
                        {
                            currOperator = operators.Pop();
                            postFix.Push(currOperator);
                        }
                        else
                        {
                            break;
                        }
                    }

                    operators.Push(token);
                }
            }
            while (operators.Count > 0)
            {
                currOperator = operators.Pop();
                postFix.Push(currOperator);
            }

            // Build the post fix string.
            string psString = string.Empty;
            foreach (string item in postFix)
            {
                psString = item + " " + psString;
            }
            psString = psString.Trim();

            return psString;
        }


        //private void CheckPostVectorForEvaluability(string inFix, string[] tokens)
        private void CheckPostVectorForEvaluability(string[] tokens)
        {
            var workstack = new Stack<string>();

            for (int index = 0; index < tokens.Length; index++)
            {
                string token = tokens[index];

                if (ExpressionKeywords.IsOperator(token))
                {
                    if (ExpressionKeywords.Functions.Contains(token) ||
                        token == "else")
                    {
                        try
                        {
                            workstack.Pop();
                            workstack.Push("0");
                        }
                        catch
                        {
                            //throw new ExpressionException("Operator error! \"" + token + "\". " + inFix);
                            throw new ExpressionException("Operator error! \"" + token + "\". ");
                        }
                    }
                    else if (token == "if" || token == "elseif")
                    {
                        try
                        {
                            workstack.Pop();
                            workstack.Pop();
                        }
                        catch
                        {
                            //throw new ExpressionException("Operator error! \"" + token + "\". " + inFix);
                            throw new ExpressionException("Operator error! \"" + token + "\". ");
                        }
                    }
                    else
                    {
                        try
                        {
                            workstack.Pop();
                            workstack.Pop();
                            workstack.Push("0");
                        }
                        catch
                        {
                            //throw new ExpressionException("Operator error! \"" + token + "\". " + inFix);
                            throw new ExpressionException("Operator error! \"" + token + "\". ");
                        }
                    }
                }
                else
                {
                    workstack.Push(token);
                }
            }

            if (workstack.Count != 1)
            {
                //throw new ExpressionException("Expression formatted incorrecty! " + inFix);
                throw new ExpressionException("Expression formatted incorrecty! ");
            }
        }


    }
}