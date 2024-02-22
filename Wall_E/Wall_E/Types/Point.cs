using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Collections;
//using Point = Wall.Wall_EEngine.Point;


namespace Walle;

public class Point : IFigure
{
    public double x { get; private set; }
    public double y { get; private set; }
    public int grosor;
    public Brush color { get; set; }
    public string identificador { get; set; }
    public string etiqueta { get; set; }

    public Point()
    {
        etiqueta = "";
        color = Brushes.Black;
        grosor = 2;
        this.identificador = "";
        Random rnd = new Random();
        x = rnd.Next(0, 700);//(int)lienzo.ActualWidth);
        y = rnd.Next(0, 700); //(int)lienzo.ActualHeight);
    }
    public Point(string identificador)
    {
        etiqueta = "";
        color = Brushes.Black;
        grosor = 2;
        this.identificador = identificador;
        Random rnd = new Random();
        x = rnd.Next(0, 700);//(int)lienzo.ActualWidth);
        y = rnd.Next(0, 700); //(int)lienzo.ActualHeight);
    }
    public Point(double x, double y)
    {
        etiqueta = "";
        color = Brushes.Black;
        this.identificador = "";
        grosor = 2;
        //Random rnd = new Random();
        this.x = x;
        this.y = y;
    }
    //Metodo para dibujar un punto en el lienzo
    public void Dibuja(Canvas lienzo)
    {
        Ellipse punto = new Ellipse
        {
            Width = this.grosor * 2,
            Height = this.grosor * 2,
            Fill = color,
        };

        Canvas.SetLeft(punto, this.x - grosor);
        Canvas.SetTop(punto, this.y - grosor);

        TextBlock textBlock = new TextBlock();
        {
            textBlock.Text = etiqueta;
            textBlock.FontFamily = new FontFamily("Arial");
            textBlock.FontSize = 12;
        }

        Canvas.SetLeft(textBlock, x + 2);
        Canvas.SetTop(textBlock, y);

        lienzo.Children.Add(textBlock);

        lienzo.Children.Add(punto);
    }

    public System.Windows.Point GetPoint()
    {
        return new System.Windows.Point(this.x, this.y);
    }

    public static Point ObtenerPunto(Point centro, Point p2, double radio)
    {
        Linea recta = new Linea(centro, p2);
        double X2;
        double Y2;
        if (centro.x <= p2.x)
        {
            if (double.IsInfinity(recta.pendiente))
            {
                if (p2.y > centro.y)
                {
                    X2 = centro.x;
                    Y2 = centro.y + radio;
                }
                else
                {
                    X2 = centro.x;
                    Y2 = centro.y - radio;

                }
            }
            else
            {
                X2 = centro.x + radio / Math.Sqrt(1 + Math.Pow(recta.pendiente, 2));
                Y2 = centro.y + (recta.pendiente * radio) / Math.Sqrt(1 + Math.Pow(recta.pendiente, 2));

            }
        }
        else
        {
            if (double.IsInfinity(recta.pendiente))
            {
                if (p2.y > centro.y)
                {
                    X2 = centro.x;
                    Y2 = centro.y + radio;
                }
                else
                {
                    X2 = centro.x;
                    Y2 = centro.y - radio;

                }
            }
            else
            {
                X2 = centro.x - radio / Math.Sqrt(1 + Math.Pow(recta.pendiente, 2));
                Y2 = centro.y - (recta.pendiente * radio) / Math.Sqrt(1 + Math.Pow(recta.pendiente, 2));
            }
        }
        return new Point(X2, Y2);
    }

    public static Secuencia CreateSequence()
    {
        var random = new Random();
        var points = new List<IType>();
        var count = random.Next(10, 1000);
        for (int i = 0; i < count; i++)
        {
            var point = new Point();
            points.Add(point);
        }
        return new Secuencia(points);
    }

    public bool Pertenece(IFigure figura)
    {
        if (figura is Circle)
        {
            Circle circle = (Circle)figura;
            if (Math.Pow(x - circle.centro.x, 2) + Math.Pow(y - circle.centro.y, 2) == Math.Pow(circle.radio, 2))
            {
                return true;
            }
            return false;
        }
        else if (figura is Segment)
        {
            Segment s1 = (Segment)figura;
            if (y == s1.pendiente * x - s1.intercepto && ((s1.p1.x < x && x < s1.p2.x) || (s1.p2.x < x && x < s1.p1.x)))
            {
                return true;
            }
            return false;
        }
        else if (figura is Ray)
        {
            Ray s1 = (Ray)figura;
            if (y == s1.pendiente * x - s1.intercepto && ((s1.p1.x < s1.p2.x && x > s1.p1.x) || (s1.p1.x > s1.p2.x && x < s1.p1.x)))
            {
                return true;
            }
            return false;
        }
        else if (figura is Ray)
        {
            Ray s1 = (Ray)figura;
            if (y == s1.pendiente * x - s1.intercepto)
            {
                return true;
            }
            return false;
        }
        else
            return false;

    }
}

