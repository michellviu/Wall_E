using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Walle
{
    public interface IFigure : IType
    {
        void Dibuja(Canvas lienzo);
        public Brush color { get; set; }
        public string etiqueta { get; set; }

    }
}