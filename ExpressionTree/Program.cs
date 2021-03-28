using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<int, int, int>> addition = (a, b) => (((a + 1) * b - 1) > 10) ? (a / b - 1) : (a + ((b.Equals(0) == true) ? 1 : 2));
            addition.Visitor();
            Console.ReadKey();
        }
    }
    public static class Helper
    {
        public static void Visitor(this Expression node)
        {
            Visitor(node, "");
        }
        public static void Visitor(this Expression node, string prefix)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Constant:
                    Console.WriteLine($"{prefix}{((ConstantExpression)node).NodeType} {((ConstantExpression)node).Type} = {((ConstantExpression)node).Value}");
                    break;
                case ExpressionType.Lambda:
                    Visitor(((LambdaExpression)node).Body, prefix);
                    break;
                case ExpressionType.Parameter:
                    Console.WriteLine($"{prefix}{((ParameterExpression)node).NodeType} {((ParameterExpression)node).Type} {((ParameterExpression)node).Name}");
                    break;
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    Console.WriteLine($"{prefix}{((BinaryExpression)node).NodeType}");
                    Visitor(((BinaryExpression)node).Left, prefix + "\t");
                    Visitor(((BinaryExpression)node).Right, prefix + "\t");
                    break;
                case ExpressionType.Conditional:
                    Console.WriteLine($"{prefix}{((ConditionalExpression)node).NodeType}");
                    Visitor(((ConditionalExpression)node).Test, prefix + "\t");
                    Visitor(((ConditionalExpression)node).IfTrue, prefix + "\t");
                    Visitor(((ConditionalExpression)node).IfFalse, prefix + "\t");
                    break;
                case ExpressionType.Call:
                    
                    if (((MethodCallExpression)node).Object == null)
                    {
                        Console.WriteLine($"{prefix}{((MethodCallExpression)node).NodeType} static");
                    }
                    else
                    {
                        Console.WriteLine($"{prefix}{((MethodCallExpression)node).NodeType}");
                        Visitor(((MethodCallExpression)node).Object, prefix + "\t");
                    }
                    Console.WriteLine($"{prefix}{((MethodCallExpression)node).NodeType} {((MethodCallExpression)node).Method.DeclaringType}.{((MethodCallExpression)node).Method.Name}");
                    foreach (var argument in ((MethodCallExpression)node).Arguments)
                    {
                        Visitor(argument, prefix + "\t");
                    }
                    break;
                default: Console.Error.WriteLine($"Node not processed yet: {node.NodeType}"); break;
            }
        }
    }
}
