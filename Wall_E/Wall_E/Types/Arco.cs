using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Drawing.Color;
using Size = System.Windows.Size;



namespace Walle;

internal class Arc : IFigure
{
    public Point Centro { get; set; } //Segmento de inicio del arco
    public Point P2 { get; set; } //Segmento de fin del arco
    public Point P3 { get; set; } //Segmento de fin del arco
    public double Radio { get; set; }
    public string identificador { get; set; }
    public Brush color { get; set; }
    public string etiqueta { get; set; }
    //Constructo
    public Arc(Point centro, Point p2, Point p3, Measure radio)
    {
        color = Brushes.Black;
        etiqueta = "";
        identificador = "";
        Centro = centro;
        P2 = p2;
        P3 = p3;
        Radio = radio.valor;
    }
    public Arc(string identificador = "")
    {
        etiqueta = "";
        color = Brushes.Black;
        var random = new Random();
        var count = random.Next(30, 100);
        this.identificador = identificador;
        Centro = new Point();
        P2 = new Point();
        P3 = new Point();
        Radio = count;

    }

    // Método para dibujar el arco en un Canvas
    public void Dibuja(Canvas canvas)
    {
        Point inicio = Point.ObtenerPunto(Centro, P2, Radio);
        Point fin = Point.ObtenerPunto(Centro, P3, Radio);
        PathGeometry pathGeometry = new PathGeometry();
        PathFigure pathFigure = new PathFigure();
        pathFigure.StartPoint = inicio.GetPoint(); // Punto inicial del arco

        ArcSegment arcSegment = new ArcSegment();
        arcSegment.Point = fin.GetPoint(); // Punto final del arco
        arcSegment.Size = new Size(Radio, Radio); // Tamaño del arco
        arcSegment.SweepDirection = SweepDirection.Counterclockwise; // Dirección del arco (sentido contrario a las agujas del reloj)

        float startAngle = Ray.GetAngle(Centro, fin);
        float endAngle = Ray.GetAngle(Centro, inicio);

        float possitiveStart = Math.Sign(startAngle) * startAngle;
        float possitiveEnd = Math.Sign(endAngle) * endAngle;
        float sweepAngle;

        if (Math.Sign(startAngle) == Math.Sign(endAngle))
        {
            if (startAngle < 0)
                sweepAngle = possitiveStart > possitiveEnd ? possitiveStart - possitiveEnd : 360 - possitiveEnd + possitiveStart;
            else
                sweepAngle = possitiveStart > possitiveEnd ? 360 - possitiveStart + possitiveEnd : possitiveEnd - possitiveStart;
        }
        else
        {
            sweepAngle = Math.Sign(endAngle) > 0 ? possitiveEnd + possitiveStart : 360 - possitiveEnd - possitiveStart;
        }



        // double angle = Linea.FindAngleBetweenLines(inicio, fin);
        if (sweepAngle > 180)
        {
            arcSegment.IsLargeArc = true; // Dibuja un arco mayor de 180 grados
        }
        else
        {
            arcSegment.IsLargeArc = false; // Dibuja un arco menor de 180 grados
        }


        pathFigure.Segments.Add(arcSegment);
        pathGeometry.Figures.Add(pathFigure);

        Path path = new Path();
        path.Data = pathGeometry;
        path.Stroke = color;
        path.StrokeThickness = 1;

        TextBlock textBlock = new TextBlock();
        {
            textBlock.Text = etiqueta;
            textBlock.FontFamily = new FontFamily("Arial");
            textBlock.FontSize = 12;
        }

        Canvas.SetLeft(textBlock, Centro.x + Radio + 2);
        Canvas.SetTop(textBlock, Centro.y);

        canvas.Children.Add(textBlock);
        // Agrega el arco al Canvas
        canvas.Children.Add(path);
    }
    public static Secuencia CreateSequence()
    {
        var random = new Random();
        var arcos = new List<IType>();
        var count = random.Next(10, 1000);
        for (int i = 0; i < count; i++)
        {
            var arc = new Arc();
            arcos.Add(arc);
        }
        return new Secuencia(arcos);
    }

    public Secuencia GetPoints()
    {
        List<IType> puntosAleatorios = new List<IType>();
        Random random = new Random();
        var count = random.Next(30, 100);

        Point inicio = Point.ObtenerPunto(Centro, P2, Radio);
        Point fin = Point.ObtenerPunto(Centro, P3, Radio);
        float InicioAngulo = Ray.GetAngle(Centro, fin);
        float FinAngulo = Ray.GetAngle(Centro, inicio);

        for (int i = 0; i < count; i++)
        {
            double angle = InicioAngulo + random.NextDouble() * (FinAngulo - InicioAngulo); // Generar un ángulo aleatorio dentro del arco
            double x = Centro.x + Radio * Math.Cos(angle);
            double y = Centro.y + Radio * Math.Sin(angle);
            Point punto = new Point(x, y);
            puntosAleatorios.Add(punto);
        }
        Secuencia secuencia = new Secuencia(puntosAleatorios);
        secuencia.IsFinite = false;

        return secuencia;
    }
}


