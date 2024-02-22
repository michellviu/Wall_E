using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace Walle;

public static class Parser
{

    public static Stack<string> color = new Stack<string>();
    public static Dictionary<string, IFigure> variables = new Dictionary<string, IFigure>();
    public static HashSet<string> coloresvalidos = new HashSet<string> { "blue", "red", "yellow", "green", "black", "white", "magenta", "gray", "cyan" };
    public static Dictionary<string, IType> constantes = new Dictionary<string, IType>();
    public static Dictionary<string, StatementFunction> funciones = new Dictionary<string, StatementFunction>();
    public static List<IFigure> draws = new List<IFigure>();
    public static List<string> prints = new List<string>();
    public static Stack<Dictionary<string, IType>> contexto = new Stack<Dictionary<string, IType>>();

    public static IEnumerable<List<Token>> SeparaPorLinea(List<Token> tokens)
    {
        //comprobar si estamos dentro de una expresion let in
        bool flag = false;
        int cont = 0;
        // List<List<string>> lineas = new List<List<string>>();
        List<Token> linea = new List<Token>();
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Value == "let")
            {
                cont++;
                flag = true;
            }
            else if (tokens[i].Value == "in")
            {
                cont--;
                if (cont == 0)
                {
                    flag = false;
                }
            }

            if (tokens[i].Value == ";" && !flag)
            {
                yield return linea;
                linea.Clear();
            }
            else
                linea.Add(tokens[i]);
        }

    }
    public static void Reset()
    {
        color.Clear();
        variables.Clear();
        constantes.Clear();
        funciones.Clear();
        draws.Clear();
        prints.Clear();
        contexto.Clear();

    }


    public static IExpressionType Parse(List<Token> instruction)
    {
        //Declaración de figuras
        if (instruction[0].Type == TokenType.Shape && instruction[1].Value != "(")
        {
            if (instruction.Count == 2)
            {
                if (instruction[1].Type == TokenType.Identifier)
                    return new StatementShape(instruction[0].Value, instruction[1].Value);
                else
                    throw new Exception("! LEXICAL ERROR: \n Se esperaba un identifivador. \n Línea: " + instruction[0].Location.Line);
            }
            else if (instruction.Count == 3)
            {
                if (instruction[1].Value == TokenValues.sequence && instruction[2].Type == TokenType.Identifier)
                {
                    return new StatementShape(instruction[0].Value + " " + instruction[1].Value, instruction[2].Value);
                }
                else
                    throw new Exception("! LEXICAL ERROR: \n Se esperaba un identificador. \n Línea: " + instruction[0].Location.Line);
            }
        }
        //Un llamado de función, una declaración de función o un identificador solo que se pasa por la recursividad.
        else if (instruction[0].Type == TokenType.Identifier && (instruction.Count == 1 || instruction[1].Value == "("))
        {
            foreach (Token token in instruction)
                if (token.Value == "=")
                    return new StatementFunction(instruction);

            return new Type(Return(instruction));
        }
        //Declaraciones de constantes
        else if (instruction[0].Type == TokenType.Identifier && (instruction[1].Value == "," || instruction[1].Value == "="))
        {
            return new StatementConst(instruction);
        }
        else if (instruction[0].Value == TokenValues.color)
        {
            if (instruction.Count != 2 || !coloresvalidos.Contains(instruction[1].Value))
                throw new Exception("Declaración de color incorrecta. \n File: " + instruction[0].Location.File + " Linea: " + instruction[0].Location.Line);

            return new StatementColor(instruction[1].Value);

        }
        //Instrucción import
        else if (instruction[0].Value == TokenValues.import)
        {
            if (instruction[0].Location.Line != 1)
                throw new Exception("! SEMANTIC ERROR: \n Uso incorrecto de bibliotecas. \n Línea: " + instruction[0].Location.Line);
            if (instruction.Count != 2 || instruction[1].Type != TokenType.Text)
                throw new Exception("! SYNTAX ERROR: \n  Línea: " + instruction[0].Location.Line);

            return new Import(instruction[1].Value);
        }
        //Instrucción draw
        else if (instruction[0].Value == TokenValues.draw)
        {
            if (instruction.Count == 1)
                throw new Exception("! SYNTAX ERROR: \n Expresión inválida. Se experaba un toke de tipo shapes. \n Línea: " + instruction[0].Location.Line);

            instruction.RemoveAt(0);
            return new Draw(instruction);
        }
        //Instrucción print
        else if (instruction[0].Value == TokenValues.print)
        {
            return new Print(instruction);
        }
        else if (instruction[0].Value == TokenValues.let)
        {
            return new LetIn(instruction);
        }
        //Instrucción condicional
        else if (instruction[0].Value == TokenValues.if_)
        {
            return new If_else(instruction);
        }
        else
        {

            return new Type(Return(instruction));
        }


        return null;

    }


    public static bool EstáIdentificadorDeclarado(string identificador, out IType valor)
    {
        //Se pregunta si el identificador es clase del diccionario con los tipos declarados
        if (contexto.Count > 0)
        {
            var dicc = contexto.Peek();
            if (dicc.ContainsKey(identificador))
            {
                valor = dicc[identificador];
                return true;
            }
            else
            {
                valor = null;
                return false;
            }
        }

        else if (constantes.ContainsKey(identificador))
        {
            valor = constantes[identificador];
            return true;
        }
        else if (variables.ContainsKey(identificador))
        {
            valor = variables[identificador];
            return true;
        }
        else
        {
            valor = null;
            return false;
        }

    }


    public static Brush ColorActual()
    {
        if (Parser.color.Count > 0)
            return GetColor(Parser.color.Peek());
        else
            return Brushes.Black;
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


    public static IType Return(List<Token> tokens)
    {
        Stack<IType> valores = new Stack<IType>();
        Stack<string> symbols = new Stack<string>();

        List<Token> list = new List<Token>();
        int i = 0;

        while (i < tokens.Count)
        {
            if ((tokens[i].Type == TokenType.Identifier && i == tokens.Count - 1) || (tokens[i].Type == TokenType.Identifier && tokens[i + 1].Value != "("))
            {
                if (EstáIdentificadorDeclarado(tokens[i].Value, out IType valor))
                    valores.Push(valor);
                else
                    throw new Exception("! SYNTAX ERROR: \n El identificador " + tokens[i].Value + " no existe en el contexto actual. \n Línea: " + tokens[i].Location.Line);
            }
            else if (tokens[i].Value == "{")
            {
                List<IType> elemtSec = new List<IType>();

                i++;
                int contador = 0;
                while (i < tokens.Count)
                {
                    if (tokens[i].Value == "(")
                    {
                        list.Add(tokens[i]);
                        contador++;
                    }
                    else if (tokens[i].Value == ")")
                    {
                        list.Add(tokens[i]);
                        contador--;
                    }
                    else if (tokens[i].Value == ",")
                    {
                        if (contador == 0)
                        {
                            elemtSec.Add(Return(list));
                            list.Clear();
                        }
                        else
                            list.Add(tokens[i]);
                    }
                    else if (tokens[i].Value == "}")
                    {
                        if (list.Count == 0)
                            break;

                        elemtSec.Add(Return(list));
                        list.Clear();
                        break;
                    }
                    else
                        list.Add(tokens[i]);

                    i++;
                }
                valores.Push(new Secuencia(elemtSec));

            }
            else if ((tokens[i].Type == TokenType.Identifier || tokens[i].Type == TokenType.Shape) && tokens[i + 1].Value == "(")
            {
                int contador = 0;
                while (i < tokens.Count)
                {
                    list.Add(tokens[i]);

                    if (tokens[i].Value == "(")
                    { contador++; }
                    else if (tokens[i].Value == ")")
                    {
                        contador--;
                        if (contador == 0)
                        {
                            FunctionCall llamado = new FunctionCall(list);
                            valores.Push(llamado.Evaluate());
                            list.Clear();


                            break;
                        }
                    }
                    i++;

                }

            }
            else if (tokens[i].Type == TokenType.Number)
            {
                valores.Push(new Number(Convert.ToDouble(tokens[i].Value)));
            }
            else if (tokens[i].Type == TokenType.Symbol)
            {
                if (tokens[i].Value == "<" || tokens[i].Value == ">" || tokens[i].Value == "<=" || tokens[i].Value == ">=" || tokens[i].Value == "==" || tokens[i].Value == "!=")
                {
                    while (symbols.Count > 0 && symbols.Peek() != "(")
                        Operation(symbols.Pop(), valores);

                    symbols.Push(tokens[i].Value);
                }
                else if (tokens[i].Value != ")" && (symbols.Count == 0 || symbols.Peek() == "(" || symbols.Peek() == "<" || symbols.Peek() == ">" || symbols.Peek() == "<=" || symbols.Peek() == ">=" || symbols.Peek() == "==" || symbols.Peek() == "!="))
                {
                    symbols.Push(tokens[i].Value);
                }
                else if (tokens[i].Value == "(")
                {
                    symbols.Push(tokens[i].Value);
                }
                else if (tokens[i].Value == "^")
                {
                    if (symbols.Peek() == "^")
                    { Operation("^", valores); }
                    else
                    { symbols.Push(tokens[i].Value); }
                }
                else if (tokens[i].Value == "*" || tokens[i].Value == "/")
                {
                    if (symbols.Peek() == "+" || symbols.Peek() == "-")
                    { symbols.Push(tokens[i].Value); }
                    else
                    {
                        Operation(symbols.Pop(), valores);
                        i--;
                    }
                }
                else if (tokens[i].Value == "+" || tokens[i].Value == "-")
                {
                    Operation(symbols.Pop(), valores);
                    i--;
                }
                else if (tokens[i].Value == ")")
                {
                    bool closed = false;
                    while (symbols.Count > 0)
                    {
                        if (symbols.Peek() == "(")
                        {
                            closed = true;
                            _ = symbols.Pop();
                            break;
                        }
                        else
                            Operation(symbols.Pop(), valores);
                    }

                    if(!closed)
                        throw new Exception("! SYNTAX ERROR: Se esperaba un '('. \n Línea: " + tokens[0].Location.Line);
                }
                else
                { throw new Exception("! LEXICAL ERROR: \n El token '" + tokens[i].Value + "' no es válido. \n Línea: " + tokens[0].Location.Line); }

            }
            else
                throw new Exception("! LEXICAL ERROR: \n El token '" + tokens[i].Value + "' no es válido. \n Línea: " + tokens[0].Location.Line);

            i++;
        }


        while (symbols.Count > 0)
        {
            if(symbols.Peek() == "(")
                    throw new Exception("! SYNTAX ERROR: Se esperaba un ')'. \n Línea: " + tokens[0].Location.Line);

            Operation(symbols.Pop(), valores);
        }
           


        if (valores.Count > 1)
            return new Secuencia(new List<IType>(valores));
        else
            return valores.Peek();

    }


    private static void Operation(string operador, Stack<IType> elements)
    {
        if (elements.Count < 2)
            throw new Exception("! SYNTAX ERROR: \n Expresión iválida.");

        try
        {
            if (operador.Equals("+"))
            {
                if (elements.Peek() is Secuencia)
                {
                    Secuencia x = (Secuencia)elements.Pop();
                    Secuencia y = (Secuencia)elements.Pop();

                    List<IType> elemtSec = new List<IType>();
                    elemtSec.AddRange(x.valores);
                    elemtSec.AddRange(y.valores);
                    Secuencia result = new Secuencia(elemtSec);

                    elements.Push(result);
                    return;
                }
                else if (elements.Peek() is Measure)
                {
                    Measure x = (Measure)elements.Pop();
                    Measure y = (Measure)elements.Pop();

                    Measure result = new Measure(x.valor + y.valor);

                    elements.Push(result);
                    return;
                }
                else if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Number result = new Number(x.value + y.value);
                    elements.Push(result);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '+' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());
            }
            else if (operador.Equals("-"))
            {
                if (elements.Peek() is Measure)
                {
                    Measure x = (Measure)elements.Pop();
                    Measure y = (Measure)elements.Pop();

                    Measure result = new Measure(y.valor - x.valor);

                    elements.Push(result);
                    return;
                }
                else if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Number result = new Number(y.value - x.value);
                    elements.Push(result);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '-' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());
            }
            else if (operador.Equals("/"))
            {
                if (elements.Peek() is Measure)
                {
                    Measure x = (Measure)elements.Pop();
                    Measure y = (Measure)elements.Pop();

                    Number result = new Number((double)(y.valor / x.valor));
                    elements.Push(result);
                    return;
                }
                else if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Number result = new Number((double)(y.value / x.value));
                    elements.Push(result);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '/' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());
            }
            else if (operador.Equals("*"))
            {
                if (elements.Peek() is Measure)
                {
                    Measure x = (Measure)elements.Pop();
                    if (elements.Peek() is Number)
                    {
                        Number y = (Number)elements.Pop();

                        Measure result = new Measure((double)(x.valor * Math.Abs(y.value)));

                        elements.Push(result);
                        return;
                    }
                    else
                        throw new Exception("! SEMANTIC ERROR: \n El operador '*' no puede ser utilizado por tokens de tipo: Measure y " + elements.Peek().GetType());

                }
                else if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    if (elements.Peek() is Measure)
                    {
                        Measure y = (Measure)elements.Pop();

                        Measure result = new Measure((double)(y.valor * Math.Abs(x.value)));

                        elements.Push(result);
                        return;
                    }
                    else if (elements.Peek() is Number)
                    {
                        Number y = (Number)elements.Pop();

                        Number result = new Number(y.value * x.value);
                        elements.Push(result);
                        return;
                    }
                    else
                        throw new Exception("! SEMANTIC ERROR: \n El operador '*' no puede ser utilizado por tokens de tipo: Number y " + elements.Peek().GetType());
                }

            }
            else if (operador.Equals("^"))
            {
                if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Number result = new Number(Math.Pow(y.value, x.value));
                    elements.Push(result);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '^' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());
            }
            else if (operador.Equals("<"))
            {
                if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Booleano booleano = new Booleano(y.value < x.value);
                    elements.Push(booleano);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '<' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());

            }
            else if (operador.Equals(">"))
            {
                if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Booleano booleano = new Booleano(y.value > x.value);
                    elements.Push(booleano);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '>' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());

            }
            else if (operador.Equals("<="))
            {
                if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Booleano booleano = new Booleano(y.value <= x.value);
                    elements.Push(booleano);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '<=' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());
            }
            else if (operador.Equals(">="))
            {
                if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Booleano booleano = new Booleano(y.value >= x.value);
                    elements.Push(booleano);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '>=' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());

            }
            else if (operador.Equals("=="))
            {
                if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Booleano booleano = new Booleano(y.value == x.value);
                    elements.Push(booleano);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '==' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());
            }
            else if (operador.Equals("!="))
            {
                if (elements.Peek() is Number)
                {
                    Number x = (Number)elements.Pop();
                    Number y = (Number)elements.Pop();

                    Booleano booleano = new Booleano(y.value != x.value);
                    elements.Push(booleano);
                    return;
                }
                else
                    throw new Exception("! SEMANTIC ERROR: \n El operador '!+' no puede ser utilizado por tokens de tipo: " + elements.Peek().GetType());
            }
            else
                throw new Exception("! SYNTAX ERROR: \n Token iválido. Se esperaba un operador");
        }
        catch (Exception)
        {
            throw new Exception("! SEMANTIC ERROR: \n El operador '" + operador + "' no es válido para los tipos definidos.");
        }
    }
}
