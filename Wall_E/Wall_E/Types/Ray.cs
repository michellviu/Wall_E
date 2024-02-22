using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Line = System.Windows.Shapes.Line;

namespace Walle;

public class Ray : Linea
{

    //P1 es de donde sale el rayo
    public Ray(Point p1, Point p2) : base(p1, p2)
    {
    }
    public Ray(string identificador = "") : base(identificador)
    {

    }
    public static float GetAngle(Point p1, Point p2)
    {
        float m = (float)((p2.y - p1.y) / (p2.x - p1.x));
        float angle = (float)(Math.Atan(m) * 180 / Math.PI);
        if (m >= 0)
        {
            angle = p1.y > p2.y ? -180 + angle : angle;
        }
        else
        {
            angle = p1.y > p2.y ? angle : 180 + angle;
        }
        return angle;
    }
    public override void Dibuja(Canvas lienzo)
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
            Stroke = Brushes.Black
        };

        Point intersectoY = new Point(0, n);
        Point intersectoX = new Point(cero, 0);
        line.X1 = this.p1.x;
        line.Y1 = this.p1.y;

        TextBlock textBlock = new TextBlock();
        {
            textBlock.Text = etiqueta;
            textBlock.FontFamily = new FontFamily("Arial");
            textBlock.FontSize = 12;
        }

        Canvas.SetLeft(textBlock, p1.x + 2);
        Canvas.SetTop(textBlock, p1.y);

        lienzo.Children.Add(textBlock);

        if (pendiente > 0)
        {
            if (this.p1.x < this.p2.x)
            {
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
                lienzo.Children.Add(line);

            }
            else
            {
                if (n > cero)
                {
                    line.X2 = intersectoY.x;
                    line.Y2 = intersectoY.y;
                }
                else
                {
                    line.X2 = intersectoX.x;
                    line.Y2 = intersectoX.y;
                }
                //this.p1.Dibuja(lienzo);
                //this.p2.Dibuja(lienzo);
                lienzo.Children.Add(line);
            }
        }
        else
        {
            if (this.p1.x < this.p2.x)
            {
                if (intersectoX.x < points[1].x)
                {
                    line.X2 = intersectoX.x;
                    line.Y2 = intersectoX.y;
                }
                else
                {
                    line.X2 = points[1].x;
                    line.Y2 = points[1].y;
                }
                //this.p1.Dibuja(lienzo);
                //this.p2.Dibuja(lienzo);
                lienzo.Children.Add(line);

            }
            else
            {
                if (intersectoY.x > points[0].x)
                {
                    line.X2 = intersectoY.x;
                    line.Y2 = intersectoY.y;
                }
                else
                {
                    line.X2 = points[0].x;
                    line.Y2 = points[0].y;
                }
                //this.p1.Dibuja(lienzo);
                //this.p2.Dibuja(lienzo);
                lienzo.Children.Add(line);
            }


        }
    }

}


