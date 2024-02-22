using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Walle;

public class Circle : IFigure
{
    //Centro de la circunferencia
    public Point centro { get; private set; }
    //Radio de la circunferencia
    public double radio { get; private set; }
    public string identificador { get; set; }
    public Brush color { get; set; }
    public string etiqueta { get; set; }

    public Circle(Point c, Measure r)
    {
        etiqueta = "";
        color = Brushes.Black;
        identificador = "";
        this.centro = c;
        this.radio = (double)r.valor;
    }
    public Circle(string identificador = "")
    {
        etiqueta = "";
        color = Brushes.Black;
        var random = new Random();
        var count = random.Next(30, 100);
        this.identificador = identificador;
        centro = new Point();
        radio = count;

    }
    //Metodo que dibuja una circunferencia
    public void Dibuja(Canvas lienzo)
    {
        // if (SePuedeDibujar(this.radio, this.centro, lienzo))
        // {
        Ellipse circunferencia = new Ellipse
        {
            Width = this.radio * 2,
            Height = this.radio * 2,
            Fill = null,
            Stroke = color,
        };

        Canvas.SetLeft(circunferencia, this.centro.x - radio);
        Canvas.SetTop(circunferencia, this.centro.y - radio);


        TextBlock textBlock = new TextBlock();
        {
            textBlock.Text = etiqueta;
            textBlock.FontFamily = new FontFamily("Arial");
            textBlock.FontSize = 12;
        }

        Canvas.SetLeft(textBlock, centro.x + radio + 2);
        Canvas.SetTop(textBlock, centro.y);

        lienzo.Children.Add(textBlock);

        lienzo.Children.Add(circunferencia);
        // }
        // else
        //     throw new Exception("No se puede dibujar la circunferencia porque queda fuera del lienzo, cambia el centro o disminuye el radio");
    }

    //Metodo para saber si un circulo se puede dibujar en el lienzo
    public static bool SePuedeDibujar(double radio, Point centro, Canvas lienzo)
    {
        //double radio = c.radio;
        double x = centro.x;
        double y = centro.y;

        if (x - radio > 0 && y - radio > 0 && x + radio < lienzo.ActualWidth && y + radio < lienzo.ActualHeight)
            return true;
        return false;
    }

    public static Secuencia CreateSequence()
    {
        var random = new Random();
        var circles = new List<IType>();
        var count = random.Next(10, 1000);

        for (int i = 0; i < count; i++)
        {
            var circle = new Circle();
            circles.Add(circle);
        }
        return new Secuencia(circles);
    }

    public Secuencia GetPoints()
    {
        List<IType> puntosAleatorios = new List<IType>();
        Random random = new Random();
        var count = random.Next(30, 100);

        for (int i = 0; i < count; i++)
        {
            double angle = random.NextDouble() * 2 * Math.PI; // Generar un ángulo aleatorio
            double x = centro.x + radio * Math.Cos(angle);
            double y = centro.y + radio * Math.Sin(angle);
            Point punto = new Point(x, y);
            puntosAleatorios.Add(punto);
        }
        var secuencia = new Secuencia(puntosAleatorios);
        secuencia.IsFinite = false;
        return secuencia;
    }

    public static Secuencia InterseccionDosCircle(Circle c1, Circle c2)
    {
        List<IType> list = new List<IType>();
        double distanciaCentros = Math.Sqrt(Math.Pow(c2.centro.x - c1.centro.x, 2) + Math.Pow(c2.centro.y - c1.centro.y, 2));

        if (distanciaCentros > c1.radio + c2.radio || distanciaCentros < Math.Abs(c1.radio - c2.radio))
        {
            return new Secuencia(list); // No hay intersección
        }
        else if (distanciaCentros == 0 && c1.radio == c2.radio)
        {
            return c1.GetPoints(); // Circunferencias coincidentes
        }
        else if (distanciaCentros == c1.radio + c2.radio || distanciaCentros == Math.Abs(c1.radio - c2.radio))
        {
            // Circunferencias tangentes externamente o internamente
            // Solo hay un punto de intersección

            Point p = new Point(c1.centro.x + c1.radio * (c2.centro.x - c1.centro.x) / distanciaCentros,
                c1.centro.y + c1.radio * (c2.centro.y - c1.centro.y) / distanciaCentros);
            list.Add(p);
            return new Secuencia(list);

        }
        else
        {
            // Se intersectan en dos puntos
            // Cálculo de los puntos de intersección
            double a = (double)((Math.Pow(c1.radio, 2) - Math.Pow(c2.radio, 2) + Math.Pow(distanciaCentros, 2)) / (2 * distanciaCentros));
            double h = (double)(Math.Sqrt(Math.Pow(c1.radio, 2) - Math.Pow(a, 2)));
            double x2 = c1.centro.x + a * (c2.centro.x - c1.centro.x) / distanciaCentros;
            double y2 = c1.centro.y + a * (c2.centro.y - c1.centro.y) / distanciaCentros;
            double interseccionX1 = x2 + h * (c2.centro.y - c1.centro.y) / distanciaCentros;
            double interseccionY1 = y2 - h * (c2.centro.x - c1.centro.x) / distanciaCentros;
            double interseccionX2 = x2 - h * (c2.centro.y - c1.centro.y) / distanciaCentros;
            double interseccionY2 = y2 + h * (c2.centro.x - c1.centro.x) / distanciaCentros;

            Point p = new Point(interseccionX1, interseccionY1);
            Point p1 = new Point(interseccionX2, interseccionY2);
            list.Add(p);
            list.Add(p1);
            return new Secuencia(list);

        }
    }
}

