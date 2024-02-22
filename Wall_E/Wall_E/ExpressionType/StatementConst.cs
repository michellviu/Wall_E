using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walle;
class StatementConst : IExpressionType
{
    public ExpressionType expressiontype => ExpressionType.StatementConst;
    public List<string> identificadores = new List<string>();
    public List<Token> cuerpo = new List<Token>();

    public StatementConst(List<Token> tokens)
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type == TokenType.Identifier)
            {
                identificadores.Add(tokens[i].Value);
            }
            else if (tokens[i].Value == ",")
            {

            }
            else if (tokens[i].Value == "=")
            {
                cuerpo.AddRange(new ArraySegment<Token>(tokens.ToArray(), i + 1, tokens.Count - 1 - i));
                break;
            }
            else
                throw new Exception("! LEXICAL ERROR: \n Token inválido en la declaracion.");


        }
    }

    public dynamic Evaluate()
    {
        IExpressionType expression = Parser.Parse(cuerpo);
        IType tokens = expression.Evaluate();


        List<IType> returns = new List<IType>();

        if (identificadores.Count > 1)
        {
            if (tokens is Secuencia)
            {
                Secuencia secuencia = (Secuencia)tokens;

                if (identificadores[0] == "_" || identificadores[0] == "rest")
                {
                    for (int i = identificadores.Count; i > 0; i--)
                    {
                        if (identificadores[i - 1] != "_" && identificadores[i - 1] != "rest")
                        {
                            if (secuencia.IsFinite)
                            {
                                IType variable = secuencia.GetElement(secuencia.Count - 1 - (identificadores.Count - i));
                                variable.identificador = identificadores[i - 1];
                                returns.Add(variable);
                            }
                            else
                            {
                                IType variable = new Undefined();
                                variable.identificador = identificadores[i - 1];
                                returns.Add(variable);
                            }

                        }
                    }

                }
                else
                    for (int i = 0; i < identificadores.Count; i++)
                    {
                        if (identificadores[i] != "_" && identificadores[i] != "rest")
                        {
                            IType variable = secuencia.GetElement(i);
                            variable.identificador = identificadores[i];
                            returns.Add(variable);
                        }
                    }
                return returns;
            }
            else
                throw new Exception("! SEMANTIC ERROR: \n Se esperaba una secuencia. \n Línea: " + cuerpo[0].Location.Line);


        }
        else
        {
            tokens.identificador = identificadores[0];
            returns.Add(tokens);

            return returns;
        }


    }
}

