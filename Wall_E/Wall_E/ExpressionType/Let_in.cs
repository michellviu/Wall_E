using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Walle;

namespace Walle;
public class LetIn : IExpressionType
{
    public ExpressionType expressiontype { get; }
    public List<Token> cuerpo;
    public List<Token> returnIn;

    public LetIn(List<Token> instruction)
    {
        expressiontype = ExpressionType.LetIn;
        cuerpo = new List<Token>();
        returnIn = new List<Token>();

        int contador = 0;
        for (int i = 0; i < instruction.Count; i++)
        {
            if (instruction[i].Value == "let")
                contador++;
            else if (instruction[i].Value == "in")
            {
                contador--;
                if (contador == 0)
                {
                    cuerpo.AddRange(new ArraySegment<Token>(instruction.ToArray(), 1, i - 1));
                    returnIn.AddRange(new ArraySegment<Token>(instruction.ToArray(), i + 1, instruction.Count - 1 - i));

                    break;
                }
            }
        }

        if (cuerpo.Count == 0 || returnIn.Count == 0)
            throw new Exception("Expresión let  incorrecta \n File: " + instruction[0].Location.File + " Linea: " + instruction[0].Location.Line);
    }


    public dynamic Evaluate()
    {
        List<string> variablesTemp = new List<string>();

        bool locree = false;
        foreach (List<Token> instruction in Parser.SeparaPorLinea(cuerpo))
        {
            if (instruction.Count == 1 && instruction[0].Value == "restore")
            {
                if (Parser.color.Count > 0)
                    Parser.color.Pop();
                continue;
            }
            IExpressionType expression = Parser.Parse(instruction);

            switch (expression.expressiontype)
            {
                //Declaración de tipos
                case ExpressionType.StatementShapes:
                    {
                        IFigure variable = expression.Evaluate();

                        if (Parser.contexto.Count > 0)
                        {
                            var dicc = Parser.contexto.Peek();
                            if (dicc.ContainsKey(variable.identificador))
                            {
                                dicc[variable.identificador] = variable;

                            }
                            else
                            {
                                dicc.Add(variable.identificador, variable);
                                // variablesTemp.Add(variable.identificador);
                            }

                        }
                        else
                        {
                            locree = true;
                            var dicc = new Dictionary<string, IType>();
                            dicc.Add(variable.identificador, variable);
                            Parser.contexto.Push(dicc);

                        }



                    }
                    break;
                //Instrucción draw
                case ExpressionType.Draw:
                    {
                        Draw draw = (Draw)expression;
                        IFigure type = draw.Evaluate();

                        if (draw.etiqueta != "")
                            type.etiqueta = draw.etiqueta;

                        if (Parser.color.Count > 0)
                            draw.color = Draw.GetColor(Parser.color.Peek());
                        else
                            draw.color = Brushes.Black;

                        type.color = draw.color;
                        Parser.draws.Add(type);
                    }
                    break;
                //Declaración de color
                case ExpressionType.Color:
                    { Parser.color.Push((string)expression.Evaluate()); }
                    break;
                //Instrucción print
                case ExpressionType.Print:
                    {
                        Print print = (Print)expression;
                        IType type = print.Evaluate();

                        if (type is Number)
                        {
                            Number number = (Number)type;
                            Parser.prints.Add(number.value.ToString());
                        }
                    }
                    break;
                //Declaración de constantes
                case ExpressionType.StatementConst:
                    {
                        List<IType> consts = expression.Evaluate();

                        foreach (IType variable in consts)
                        {
                            if (Parser.contexto.Count > 0)
                            {
                                var dicc = Parser.contexto.Peek();
                                if (dicc.ContainsKey(variable.identificador))
                                {
                                    dicc[variable.identificador] = variable;

                                }
                                else
                                {
                                    dicc.Add(variable.identificador, variable);
                                    // variablesTemp.Add(variable.identificador);
                                }

                            }
                            else
                            {
                                locree = true;
                                var dicc = new Dictionary<string, IType>();
                                dicc.Add(variable.identificador, variable);
                                Parser.contexto.Push(dicc);

                            }

                        }
                    }
                    break;
                default:
                    throw new Exception("Expresion inválida dentro de una estructura let_in");
            }
        }

        IType result = Parser.Parse(returnIn).Evaluate();


        if (locree)
            Parser.contexto.Pop();



        return result;
    }
}
