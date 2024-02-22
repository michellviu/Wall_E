using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Walle;
public class Draw : IExpressionType
{
    public ExpressionType expressiontype { get; }
    public string etiqueta;
    public Brush color;
    public List<Token> cuerpoDraw;

    public Draw(List<Token> tokens)
    {
        expressiontype = ExpressionType.Draw;
        cuerpoDraw = tokens;

        if (cuerpoDraw[cuerpoDraw.Count - 1].Type == TokenType.Text)
        {
            etiqueta = cuerpoDraw[cuerpoDraw.Count - 1].Value;
            cuerpoDraw.RemoveAt(cuerpoDraw.Count - 1);
        }
        else
            etiqueta = "";
       
        if (Parser.color.Count > 0)
            color = GetColor(Parser.color.Peek());
        else
            color = Brushes.Black;
    }

    public dynamic Evaluate()
    {
        IType objetos = Parser.Return(cuerpoDraw);

        if (objetos == null)
            throw new Exception("! LEXICAL ERROR: \n Token inválido. Se esperaba un objeto de tipo shape. \n Línea: " + cuerpoDraw[0].Location.Line);

        if (objetos is IFigure)
        {
            var figure = (IFigure)objetos;
            return figure;
        }
        else     
            throw new Exception("! SEMANTIC ERROR: \n No se puede definir en una instrucción Draw objetos de tipo '"+ objetos.GetType() + "' \n Línea: " + cuerpoDraw[0].Location.Line);        
    }



    public static Brush GetColor(string color)
    {
        switch (color)
        {
            case "red": return Brushes.Red;
            case "blue": return Brushes.Blue;
            case "yellow": return Brushes.Yellow;
            case "green": return Brushes.Green;
            case "cyan": return Brushes.Cyan;
            case "magenta": return Brushes.Magenta;
            case "white": return Brushes.White;
            case "gray": return Brushes.Gray;
            default: return Brushes.Black;
        }
    }


}
