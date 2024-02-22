using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walle;

public class Measure: IType
{
    public double valor { get; private set; }
    public string identificador { get; set; }

    public Measure(double valor, string identificador = "")
    {
        this.identificador = identificador;
        this.valor = Math.Abs(valor);
    }


    public Measure(Point p1, Point p2, string identificador="")
    {
        this.identificador = identificador;
        valor = (double)Math.Sqrt(Math.Pow((p1.x - p2.x), 2) + Math.Pow((p1.y - p2.y), 2));
       // return new Measure(distancia);
    }
    public static Measure DistanciaPuntoRecta(Point p, Linea l)
    {
        double A = -l.pendiente;
        double B = 1;
        double C = l.pendiente * l.p1.x - l.p1.y;

        double result = (double)( Math.Abs(A * p.x + B * p.y + C) / Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2)));
        return new Measure(result);

    }
}

