using System;

namespace Walle;
public class StatementShape: IExpressionType
{
    public ExpressionType expressiontype { get; }
    public string tipo { get; }
    public string identificador { get; }



    public StatementShape( string tipo, string identificador)
    {
        expressiontype = ExpressionType.StatementShapes;
        this.tipo = tipo;
        this.identificador = identificador;
    }



    public dynamic Evaluate()
    {
  
        if (tipo == "point")
        {
            Point point = new Point(identificador);
            return point;
        }
        else if (tipo == "circle")
        {
            Circle circle = new Circle(identificador);
            return circle;
        }
        else if (tipo == "line")
        {
            Linea line = new Linea(identificador);
            return line;
        }
        else if (tipo == "segment")
        {
            Segment segment = new Segment(identificador);
            return segment;
        }
        else if (tipo == "ray")
        {
            Ray ray = new Ray(identificador);
            return ray;
        }
        else if (tipo == "arc")
        {
            Arc arc = new Arc(identificador);
            return arc;
        }
        else if (tipo == "point sequence")
        {
            Secuencia secuencia = Point.CreateSequence();
            secuencia.identificador = identificador;

            return secuencia;
        }
        else if (tipo == "circle sequence")
        {
            Secuencia secuencia = Circle.CreateSequence();
            secuencia.identificador = identificador;

            return secuencia;
        }
        else if (tipo == "line sequence")
        {
            Secuencia secuencia = Linea.CreateSequence();
            secuencia.identificador = identificador;

            return secuencia;
        }
        else if (tipo == "ray sequence")
        {
            Secuencia secuencia = Ray.CreateSequence();
            secuencia.identificador = identificador;

            return secuencia;
        }
        else if (tipo == "segment sequence")
        {
            Secuencia secuencia = Segment.CreateSequence();
            secuencia.identificador = identificador;

            return secuencia;
        }
        else if (tipo == "arc sequence")
        {
            Secuencia secuencia = Arc.CreateSequence();
            secuencia.identificador = identificador;
           
            return secuencia;
        }
        else
            return null;

    }


}
