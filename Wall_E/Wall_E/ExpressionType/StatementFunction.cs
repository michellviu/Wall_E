using System;
using System.Collections.Generic;

namespace Walle;
public class StatementFunction: IExpressionType
{   
    public ExpressionType expressiontype { get;}
    public string identificador { get; }
    public List<string> parámetros { get; set; }
    public List<Token> cuerpo { get; }

    public StatementFunction(List<Token> instruccion)
    {
        identificador = instruccion[0].Value;
        parámetros = new List<string>();
        cuerpo = new List<Token>();
        expressiontype = ExpressionType.StatementFunction;
    
        int i = 0;

        if (instruccion[1].Value == "(")
        {
            i = 2;
            while (instruccion[i].Value != ")")
            {
                
                if (instruccion[i].Type == TokenType.Identifier)
                    parámetros.Add(instruccion[i].Value);
                else if (instruccion[i].Value == ",") { }
                else
                     throw new Exception("! LEXICAL ERROR: \n Token inválido en la declaración de función.");  
                
                i++;
            }
        }
        

        if(instruccion[i+1].Value == "=")
        {
            cuerpo.AddRange(new ArraySegment<Token>(instruccion.ToArray(), i + 2, instruccion.Count - i -2));
        }
        else
            throw new Exception("! LEXICAL ERROR: \n Token inválido en la declaración de función.");

    }

    public object Evaluate()
    {   

        return null;
    }
}
