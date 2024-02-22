using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Walle
{
    class Etiqueta
    {
        public string identificador { get; }
        public double x { get; }
        public double y { get; }

        public Etiqueta(string identificador, double x, double y)
        {
            this.identificador = identificador;
            this.x = x;
            this.y = y;
        }

        public void Dibuja(Canvas lienzo)
        {
            TextBlock textBlock = new TextBlock();
            {
                textBlock.Text = identificador;
            }

            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);

            lienzo.Children.Add(textBlock);
        }
    }
}
