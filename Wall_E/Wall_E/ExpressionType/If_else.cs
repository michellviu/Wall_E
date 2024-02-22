using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walle;

namespace Walle;
public class If_else : IExpressionType
{
    public ExpressionType expressiontype { get; }
    public List<Token> condicional;
    public List<Token> instructionThen;
    public List<Token> instructionElse;

    public If_else(List<Token> instruction)
    {
        expressiontype = ExpressionType.Conditional;
        condicional = new List<Token>();
        instructionThen = new List<Token>();
        instructionElse = new List<Token>();

        int inicio = 0;
        int fin = 0;

        for (int i = 0; i < instruction.Count; i++)
        {
            if (instruction[i].Value == "then")
            {
                fin = i - 1;
                condicional.AddRange(new ArraySegment<Token>(instruction.ToArray(), 1, fin));
                inicio = i + 1;
                break;
            }

        }
        for (int i = instruction.Count - 1; i > inicio; i--)
        {
            if (instruction[i].Value == "else")
            {
                fin = i;
                instructionThen.AddRange(new ArraySegment<Token>(instruction.ToArray(), inicio, fin - inicio));
                instructionElse.AddRange(new ArraySegment<Token>(instruction.ToArray(), i + 1, instruction.Count - i - 1));
                break;
            }

        }

        if (condicional.Count == 0 || instructionThen.Count == 0 || instructionElse.Count == 0)
            throw new Exception("! SYNTAX ERROR: \n Expresión condicional incompleta. Línea: " + condicional[0].Location.Line);

    }


    public dynamic Evaluate()
    {
        IType condicion = Parser.Parse(condicional).Evaluate();

        if(condicion is Secuencia)
        {
            Secuencia secuencia = (Secuencia)condicion;

            if(secuencia.IsFinite && secuencia.Count != 0)
                return Parser.Parse(instructionThen).Evaluate();
            else
                return Parser.Parse(instructionElse).Evaluate();
        }
        else if(condicion is Number)
        {
            Number number = (Number)condicion;
            
            if(number.value != 0)
                return Parser.Parse(instructionThen).Evaluate();
            else
                return Parser.Parse(instructionElse).Evaluate();

        }
        else if(condicion is Undefined)
            return Parser.Parse(instructionElse).Evaluate();
        else if(condicion is Booleano)
        {
            Booleano booleano = (Booleano)condicion;
            
            if(booleano.valor)
                return Parser.Parse(instructionThen).Evaluate();
            else
                return Parser.Parse(instructionElse).Evaluate();
        }
        else
            return Parser.Parse(instructionThen).Evaluate();


    }
}

