namespace Walle;
public interface IExpressionType
{
    public dynamic Evaluate();
    public ExpressionType expressiontype { get; }
}
