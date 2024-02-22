using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Walle;

public class Linea : IFigure
{
    public Point p1 { get; }
    public Point p2 { get; }
    public double pendiente { get; }
    public string identificador { get; set; }
    public Brush color { get; set; }
    public string etiqueta { get; set; }
    public double intercepto { get; }

    public Linea(Point p1, Point p2)
    {
        etiqueta = "";
        color = Brushes.Black;
        identificador = "";
        this.p1 = p1;
        this.p2 = p2;
        this.pendiente = (double)((p2.y - p1.y) / (p2.x - p1.x));
        intercepto = p1.y - pendiente * p1.x;

    }
    public Linea(string identificador = "")
    {
        etiqueta = "";
        color = Brushes.Black;
        this.identificador = identificador;
        p1 = new Point();
        p2 = new Point();
        pendiente = (p2.y - p1.y) / (p2.x - p1.x);
        intercepto = p1.y - pendiente * p1.x;
    }


    public virtual Point[] IntersectosPendienteNegativa(double m, Point p, Canvas lienzo)
    {
        Point[] result = new Point[2];
        double n = p.y - m * p.x;
        double x = (lienzo.ActualHeight - n) / m;
        //X
        result[0] = new Point(x, lienzo.ActualHeight);
        double y = (lienzo.ActualWidth * m) + n;
        //Y
        result[1] = new Point(lienzo.ActualWidth, y);
        return result;

    }
    public static double FindAngleBetweenLines(Point start1, Point end1)//, Point start2, Point end2)
    {
        // Calcular los vectores que representan las rectas
        //Linea vector1 = new Linea(start1, end1);
        //Linea vector2 = new Linea(start2, end2);

        ////θ = arctan((m_2 - m_1) / (1 + m_1 · m_2))
        //double angleInRadians = Math.Atan((vector2.pendiente - vector1.pendiente) / (1 + vector1.pendiente * vector2.pendiente));
        //double angleInDegrees = angleInRadians * 180 / Math.PI;
        // Convertir a grados
        // Calcular el ángulo entre los vectores utilizando el arcotangente
        // double angle = Math.Atan2(vector2.y, vector2.x) - Math.Atan2(vector1.y, vector1.x);
        //  angle = angle * 180 / Math.PI;  // Convertir de radianes a grados

        // Asegurarse de devolver un ángulo positivo (entre 0 y 360 grados)
        double d = (double)Math.Sqrt(Math.Pow(end1.x - start1.x, 2) + Math.Pow(end1.y - start1.y, 2));
        double r = d / 2;
        double theta = 2 * Math.Asin(d / (2 * r));
        double angle = theta * 180 / Math.PI;
        //if (angleInDegrees < 0)
        //{
        //    angleInDegrees = angleInDegrees + 180;
        //}
        return angle;
    }

    public Line ObtenerLinea(Canvas lienzo)
    {
        //Calculo de la pendiente
        double pendiente = this.pendiente;
        //Interseccion con el eje Y
        double n = this.p1.y - pendiente * this.p1.x;
        //Interseccion con el eje X
        double cero = -n / pendiente;
        //Interseccion con los ejes derechos del CANVAS
        Point[] points = IntersectosPendienteNegativa(pendiente, this.p1, lienzo);
        Line line = new Line
        {
            //Color
            Stroke = color
        };

        Point intersectoY = new Point(0, n);
        Point intersectoX = new Point(cero, 0);

        if (pendiente > 0)
        {
            if (n > cero)
            {
                line.X1 = intersectoY.x;
                line.Y1 = intersectoY.y;
                if (points[0].x < points[1].x)
                {
                    line.X2 = points[0].x;
                    line.Y2 = points[0].y;
                }
                else
                {
                    line.X2 = points[1].x;
                    line.Y2 = points[1].y;
                }
                //this.p1.Dibuja(lienzo);
                //this.p2.Dibuja(lienzo);
                //lienzo.Children.Add(line);
                return line;

            }
            else
            {
                line.X1 = intersectoX.x;
                line.Y1 = intersectoX.y;
                if (points[0].x < points[1].x)
                {
                    line.X2 = points[0].x;
                    line.Y2 = points[0].y;
                }
                else
                {
                    line.X2 = points[1].x;
                    line.Y2 = points[1].y;
                }
                //this.p1.Dibuja(lienzo);
                //this.p2.Dibuja(lienzo);
                //  lienzo.Children.Add(line);
                return line;
            }
        }
        else
        {
            if (points[1].x > intersectoX.x)
            {
                line.X1 = intersectoX.x;
                line.Y1 = intersectoX.y;
                if (points[0].x > intersectoY.x)
                {
                    line.X2 = points[0].x;
                    line.Y2 = points[0].y;

                }
                else
                {
                    line.X2 = intersectoY.x;
                    line.Y2 = intersectoY.y;
                }
                //this.p1.Dibuja(lienzo);
                //this.p2.Dibuja(lienzo);
                //lienzo.Children.Add(line);
                return line;

            }
            else
            {
                line.X1 = points[1].x;
                line.Y1 = points[1].y;
                if (points[0].x > intersectoY.x)
                {
                    line.X2 = points[0].x;
                    line.Y2 = points[0].y;

                }
                else
                {
                    line.X2 = intersectoY.x;
                    line.Y2 = intersectoY.y;
                }
                //this.p1.Dibuja(lienzo);
                //this.p2.Dibuja(lienzo);
                return line;

            }
        }
    }
    public virtual void Dibuja(Canvas lienzo)
    {

        Line line = ObtenerLinea(lienzo);

        TextBlock textBlock = new TextBlock();
        {
            textBlock.Text = etiqueta;
            textBlock.FontFamily = new FontFamily("Arial");
            textBlock.FontSize = 12;
        }

        Canvas.SetLeft(textBlock, p1.x + 2);
        Canvas.SetTop(textBlock, p1.y);

        lienzo.Children.Add(textBlock);

        lienzo.Children.Add(line);

    }

    public static Secuencia CreateSequence()
    {
        var random = new Random();
        var lineas = new List<IType>();
        var count = random.Next(10, 1000);

        for (int i = 0; i < count; i++)
        {
            var linea = new Linea();

            lineas.Add(linea);
        }
        return new Secuencia(lineas);
    }

    public Secuencia GetPoints()
    {
        List<IType> puntosAleatorios = new List<IType>();
        Random random = new Random();
        var count = random.Next(30, 100);
        for (int i = 0; i < count; i++)
        {
            double t = random.NextDouble(); // Generar un número aleatorio entre 0 y 1
            double x = p1.x + t * (p2.x - p1.x);
            double y = p1.y + t * (p2.y - p1.y);
            Point punto = new Point(x, y);
            puntosAleatorios.Add(punto);
        }
        Secuencia secuencia = new Secuencia(puntosAleatorios);
        secuencia.IsFinite = false;
        return secuencia;
    }

    public static Point InterseccionDosLineas(Linea l, Linea l1)
    {
        if (l.pendiente == l1.pendiente)
        {
            return null;
        }
        else if (double.IsInfinity(l.pendiente) || double.IsInfinity(l1.pendiente))
        {
            if (double.IsInfinity(l.pendiente))
            {
                double Intercepto = l1.p1.y - l1.pendiente * l1.p1.x;
                double x = l.p1.x; // Un valor de x de la recta paralela al eje Y
                double y = l1.pendiente * x + Intercepto; // Reemplazamos x en la ecuación de la otra recta para obtener y
                return new Point(x, y);
            }
            else if (double.IsInfinity(l1.pendiente))
            {
                double Intercepto = l.p1.y - l.pendiente * l.p1.x;
                double x = l1.p1.x; // Un valor de x de la recta paralela al eje Y
                double y = l.pendiente * x + Intercepto; // Reemplazamos x en la ecuación de la otra recta para obtener y
                return new Point(x, y);
            }
        }
        double A = -l.pendiente;
        double B = 1;
        double C = l.p1.y - l.pendiente * l.p1.x;

        double A1 = -l1.pendiente;
        double B1 = 1;
        double C1 = l1.p1.y - l1.pendiente * l1.p1.x;

        double X = (C1 - C) / (-A + A1);
        double Y = -A * X + C;
        Point p = new Point(X, Y);
        return p;

    }

    public static Secuencia InterseccionLineaCircle(Circle circunferencia, Linea recta)
    {
        List<IType> list = new List<IType>();
        double Intercepto = recta.p1.y - recta.pendiente * recta.p1.x;
        double a = 1 + (double)Math.Pow(recta.pendiente, 2);
        double b = 2 * (recta.pendiente * (Intercepto - circunferencia.centro.y) - circunferencia.centro.x);
        double c = (double)(Math.Pow(circunferencia.centro.x, 2) + Math.Pow(Intercepto - circunferencia.centro.y, 2) - Math.Pow(circunferencia.radio, 2));

        double discriminante = Math.Pow(b, 2) - 4 * a * c;

        if (discriminante < 0)
        {
            return new Secuencia(list); // La recta y la circunferencia no se intersectan
        }
        else if (discriminante == 0)
        {
            double x = -b / (2 * a);
            double y = recta.pendiente * x + Intercepto;
            Point p = new Point(x, y);
            list.Add(p);
            return new Secuencia(list); // La recta es tangente a la circunferencia
        }
        else
        {
            double x1 = (-b + (double)Math.Sqrt(discriminante)) / (2 * a);
            double y1 = recta.pendiente * x1 + Intercepto;

            double x2 = (-b - (double)Math.Sqrt(discriminante)) / (2 * a);
            double y2 = recta.pendiente * x2 + Intercepto;
            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y2);
            list.Add(p1);
            list.Add(p2);

            return new Secuencia(list);
        }
    }
}


