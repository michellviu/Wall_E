using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walle;

public class FunctionCall : IExpressionType
{
    public string identificador { get; }
    public ExpressionType expressiontype { get; }
    public List<IType> parametros { get; }
    public Dictionary<string, IType> parametrosDictionary { get; set; }



    public FunctionCall(List<Token> tokens)
    {
        parametros = new List<IType>();
        expressiontype = ExpressionType.FunctionCall;
        identificador = tokens[0].Value;
        parametrosDictionary = new Dictionary<string, IType>();


        if (tokens[1].Value == "(")
        {
            int cont = 0;
            List<Token> list = new List<Token>();

            for (int j = 2; j < tokens.Count - 1; j++)
            {
                if (tokens[j].Value == "(") cont++;
                if (tokens[j].Value == ")") cont--;

                if (tokens[j].Value == "," && cont == 0)
                {
                    IType parametro = Parser.Return(list);
                    parametros.Add(parametro);
                    list.Clear();
                }
                else
                    list.Add(tokens[j]);

                if (j == tokens.Count - 2)
                {
                    IType parametro = Parser.Return(list);
                    parametros.Add(parametro);
                }
            }

            if (Parser.funciones.ContainsKey(identificador))
            {
                for (int i = 0; i < parametros.Count; i++)
                {
                    //if (parametrosDictionary.ContainsKey(Parser.funciones[identificador].parámetros[i]))
                    parametrosDictionary.Add(Parser.funciones[identificador].parámetros[i], parametros[i]);
                }
                Parser.contexto.Push(parametrosDictionary);
            }
           
        }
    }



    public dynamic Evaluate()
    {
        if (identificador == "point")
        {
            if (parametros.Count == 2 && parametros[0] is Number && parametros[1] is Number)
            {
                Number x = (Number)parametros[0];
                Number y = (Number)parametros[1];
                Point point = new Point(x.value, y.value);
                return point;
            }
            else
                throw new Exception("! SEMANTIC ERROR: \n El constructor de 'print' esperaba un tipo 'Walle.Point'.");
        }


        if (identificador == "circle")
        {
            if (parametros.Count == 2 && parametros[0] is Point && parametros[1] is Measure)
            {
                Point centro = (Point)parametros[0];
                Measure radio = (Measure)parametros[1];

                Circle circle = new Circle(centro, radio);
                return circle;

            }
            else
                throw new Exception("! SEMANTIC ERROR: \n El constructor de 'circle' esperaba un tipo 'Walle.Point' en el primer argumento y un tipo 'Walle.Measure'.");
        }
        if (identificador == "line")
        {
            if (parametros.Count == 2 && parametros[0] is Point && parametros[1] is Point)
            {
                Linea linea = new Linea((Point)parametros[0], (Point)parametros[1]);
                return linea;

            }
            else
                throw new Exception("! SEMANTIC ERROR: \n El constructor de 'circle' esperaba un tipo 'Walle.Point' en el primer argumento y un tipo 'Walle.Measure' en el segundo argumento.");
        }
        if (identificador == "ray")
        {
            if (parametros.Count == 2 && parametros[0] is Point && parametros[1] is Point)
            {
                Ray rayo = new Ray((Point)parametros[0], (Point)parametros[1]);
                return rayo;
            }
            else
                throw new Exception("! SEMANTIC ERROR: \n El constructor de 'ray' esperaba un tipo 'Walle.Point' en el primer argumento y un tipo 'Walle.Point' en el segundo argumento.");

        }
        if (identificador == "segment")
        {
            if (parametros.Count == 2 && parametros[0] is Point && parametros[1] is Point)
            {
                Segment segment = new Segment((Point)parametros[0], (Point)parametros[1]);
                return segment;
            }
            else
                throw new Exception("! SEMANTIC ERROR: \n El constructor de 'segement' esperaba un tipo 'Walle.Point' en el primer argumento y un tipo 'Walle.Point' en el segundo argumento.");

        }
        if (identificador == "arc")
        {
            if (parametros.Count == 4 && parametros[0] is Point && parametros[1] is Point && parametros[2] is Point && parametros[3] is Measure)
            {
                Arc arc = new Arc((Point)parametros[0], (Point)parametros[1], (Point)parametros[2], (Measure)parametros[3]);
                return arc;
            }
            else
                throw new Exception("! SEMANTIC ERROR: \n El constructor de 'arc' esperaba un tipo 'Walle.Point' en el primer, segundo y tercer argumento y un tipo 'Walle.Measure' en el cuarto argumento.");

        }
        if (identificador == "samples")
        {
            Secuencia secuencia = Point.CreateSequence();
            secuencia.IsFinite = false;
            return secuencia;
        }
        if (identificador == "measure")
        {
            if (parametros.Count == 2 && parametros[0] is Point && parametros[1] is Point)
            {
                Measure measure = new Measure((Point)parametros[0], (Point)parametros[1]);
                return measure;
            }
            else if (parametros.Count == 1 && parametros[0] is Number)
            {
                Number distancia = (Number)parametros[0];
                Measure measure = new Measure(distancia.value);
                return measure;
            }
            else
                throw new Exception("! SEMANTIC ERROR: \n El constructor de 'measure' tiene una primera sobrecarga que esperaba un tipo 'Walle.Point' en el primer y segundo argumento. Y una segunda sobrecarga que esepra un tipo 'Number' como único argumento.");

        }
        if (identificador == "intersect")
        {
            if (parametros.Count == 2 && parametros[0] is IFigure && parametros[1] is IFigure)
            {
                Secuencia secuencia = Secuencia.Interseccion((IFigure)parametros[0], (IFigure)parametros[1]);
                return secuencia;
            }
            else
                throw new Exception("! SEMANTIC ERROR: \n El constructor de 'intersect' recibe dos argument 'Walle.Point' en el primer y segundo argumento. Y una segunda sobrecarga que esepra un tipo 'Number' como único argumento.");

        }


        if (Parser.funciones.ContainsKey(identificador))
        {
            if (Parser.funciones[identificador].parámetros.Count != parametros.Count)
                throw new Exception("! SEMANTIC ERROR: \n Ninguna sobrecarga de la función '" + identificador + "' recibe '" + parametros.Count + "' parámetros.");

            //for (int i = 0; i < parametros.Count; i++)
            //{
            //    if (!Parser.constantes.ContainsKey(Parser.funciones[identificador].parámetros[i]))
            //        Parser.constantes.Add(Parser.funciones[identificador].parámetros[i], parametros[i]);
            //    else
            //        Parser.constantes[Parser.funciones[identificador].parámetros[i]] = parametros[i];
            //}
            //for (int i = 0; i < parametros.Count; i++)
            //{
            //    //if (parametrosDictionary.ContainsKey(Parser.funciones[identificador].parámetros[i]))
            //        parametrosDictionary.Add(Parser.funciones[identificador].parámetros[i], parametros[i]);
            //}



            IExpressionType expresion = Parser.Parse(Parser.funciones[identificador].cuerpo);
            IType respuesta = expresion.Evaluate();

            //for (int i = 0; i < parametros.Count; i++)
            //{
            //    Parser.constantes.Remove(Parser.funciones[identificador].parámetros[i]);
            //}
            Parser.contexto.Pop();

            return respuesta;
        }
        else
        {
            throw new Exception("! SYNTAX ERROR: \n La funcion '" + identificador + "' no esta definida.");
        }
    }
}
