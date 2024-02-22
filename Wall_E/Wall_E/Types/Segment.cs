
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Walle;

public class Segment : Linea
{
    public Segment(Point p1, Point p2) : base(p1, p2)
    {
    }
    public Segment(string identificador = "") : base(identificador)
    {

    }
    public override void Dibuja(Canvas lienzo)
    {
        Line line = new Line
        {
            //Color
            Stroke = color
        };

        line.X1 = this.p1.x;
        line.Y1 = this.p1.y;
        line.X2 = this.p2.x;
        line.Y2 = this.p2.y;
        //this.p1.Dibuja(lienzo);
        //this.p2.Dibuja(lienzo);
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
}

