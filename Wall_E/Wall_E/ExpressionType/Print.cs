using System.Collections.Generic;

namespace Walle;
public class Print: IExpressionType
{
    public ExpressionType expressiontype { get; }
    public string etiqueta  { get; }
    public List<Token> imprimir { get; }

    public Print(List<Token> tokens)
    {
        etiqueta = "";
        tokens.RemoveAt(0);
        expressiontype = ExpressionType.Print;

        if(tokens[tokens.Count-1].Type == TokenType.Text)
        {
            etiqueta = tokens[tokens.Count - 1].Value;
            tokens.RemoveAt(tokens.Count - 1);
        }
        imprimir = tokens;
    }



    public dynamic Evaluate()
    {
        return Parser.Parse(imprimir).Evaluate();
    }
}
