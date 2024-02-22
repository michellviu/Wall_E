using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Walle
{
    public class Secuencia : IFigure
    {
        public ExpressionType expressiontype { get; }
        public bool IsFinite { get; set; }
        public List<IType> valores { get; set; }
        public string identificador { get; set; }
        public string etiqueta { get; set; }
        public Brush color { get; set; }
        public int Count
        {
            get { return valores.Count; }
        }

        public Secuencia(List<IType> valores, string nombre = "", bool finita = true)
        {
            etiqueta = "";
            color = Brushes.Black;
            this.valores = valores;
            IsFinite = finita;
            expressiontype = ExpressionType.Sequence;
            identificador = nombre;
        }

        public IType GetElement(int index)
        {
            try
            {
                return valores[index];
            }
            catch (Exception)
            {
                return new Undefined();
            }
        }

        public IEnumerable<IType> RestofElements(int start)
        {
            int i = 0;
            foreach (var element in valores)
            {
                if (i == start)
                {
                    yield return element;
                }
                else
                    i++;
            }
        }

        public void Dibuja(Canvas lienzo)
        {
            for (int i = 0; i < valores.Count; i++)
            {

                if (!(valores[i] is IFigure))
                    throw new Exception("Los elementos de la secuencia no son dibujables");

                var figure = (IFigure)valores[i];
                figure.color = color;
                if (i == 0)
                {
                    figure.etiqueta = etiqueta;
                }
                figure.Dibuja(lienzo);
            }

        }

        public static Secuencia Interseccion(IFigure f1, IFigure f2)
        {
            List<IType> list = new List<IType>();
            if (f1 is Point && f2 is Point)
            {
                Point p1 = (Point)f1;
                Point p2 = (Point)f2;
                if (p1.x == p2.x && p1.y == p2.y)
                {
                    list.Add(p1);
                    return new Secuencia(list);
                }

            }
            else if (f1 is Segment && f2 is Segment)
            {
                Segment s1 = (Segment)f1;
                Segment s2 = (Segment)f2;
                Point p = Linea.InterseccionDosLineas(s1, s2);
                if (((s1.p1.x < p.x && p.x < s1.p2.x) || (s1.p2.x < p.x && p.x < s1.p1.x)) && ((s2.p1.x < p.x && p.x < s2.p2.x) || (s2.p2.x < p.x && p.x < s2.p1.x)))
                {
                    list.Add(p);
                    return new Secuencia(list);
                }
            }
            else if (f1 is Ray && f2 is Ray)
            {
                Ray s1 = (Ray)f1;
                Ray s2 = (Ray)f2;
                Point p = Linea.InterseccionDosLineas(s1, s2);
                if (((s1.p1.x < s1.p2.x && p.x > s1.p1.x) || (s1.p1.x > s1.p2.x && p.x < s1.p1.x)) && ((s2.p1.x < s2.p2.x && p.x > s2.p1.x) || (s2.p1.x > s2.p2.x && p.x < s2.p1.x)))
                {
                    list.Add(p);
                    return new Secuencia(list);
                }
            }
            else if ((f1 is Ray && f2 is Segment) || (f2 is Ray && f1 is Segment))
            {
                if (f1 is Ray && f2 is Segment)
                {
                    Ray s1 = (Ray)f1;
                    Segment s2 = (Segment)f2;
                    Point p = Linea.InterseccionDosLineas(s1, s2);
                    if (((s2.p1.x < p.x && p.x < s2.p2.x) || (s2.p2.x < p.x && p.x < s2.p1.x)) && ((s1.p1.x < s1.p2.x && p.x > s1.p1.x) || (s1.p1.x > s1.p2.x && p.x < s1.p1.x)))
                    {
                        list.Add(p);
                        return new Secuencia(list);
                    }
                }
                else
                {
                    Segment s2 = (Segment)f1;
                    Ray s1 = (Ray)f2;
                    Point p = Linea.InterseccionDosLineas(s1, s2);
                    if(p==null)
                    {
                        return new Secuencia(list); 
                    }
                    if (((s2.p1.x < p.x && p.x < s2.p2.x) || (s2.p2.x < p.x && p.x < s2.p1.x)) && ((s1.p1.x < s1.p2.x && p.x > s1.p1.x) || (s1.p1.x > s1.p2.x && p.x < s1.p1.x)))
                    {
                        list.Add(p);
                        return new Secuencia(list);
                    }
                }
            }
            else if ((f1 is Linea && f2 is Segment) || (f2 is Linea && f1 is Segment))
            {
                if (f1 is Linea && f2 is Segment)
                {
                    Linea s1 = (Linea)f1;
                    Segment s2 = (Segment)f2;
                    Point p1 = Linea.InterseccionDosLineas(s1, s2);
                    if ((s2.p1.x < p1.x && p1.x < s2.p2.x) || (s2.p2.x < p1.x && p1.x < s2.p1.x))
                    {
                        list.Add(p1);
                        return new Secuencia(list);
                    }
                }
                else
                {
                    Segment s1 = (Segment)f1;
                    Linea s2 = (Linea)f2;
                    Point p1 = Linea.InterseccionDosLineas(s1, s2);
                    if ((s1.p1.x < p1.x && p1.x < s1.p2.x) || (s1.p2.x < p1.x && p1.x < s1.p1.x))
                    {
                        list.Add(p1);
                        return new Secuencia(list);
                    }

                }
            }
            else if ((f1 is Linea && f2 is Ray) || (f2 is Linea && f1 is Ray))
            {
                if (f1 is Linea && f2 is Ray)
                {
                    Linea s1 = (Linea)f1;
                    Ray s2 = (Ray)f2;
                    Point point = Linea.InterseccionDosLineas(s1, s2);
                    if ((s2.p1.x < s2.p2.x && point.x > s2.p1.x) || (s2.p1.x > s2.p2.x && point.x < s2.p1.x))
                    {
                        list.Add(point);
                        return new Secuencia(list);
                    }
                }
                else
                {
                    Ray s1 = (Ray)f1;
                    Linea s2 = (Linea)f2;
                    Point point = Linea.InterseccionDosLineas(s1, s2);
                    if ((s1.p1.x < s1.p2.x && point.x > s1.p1.x) || (s1.p1.x > s1.p2.x && point.x < s1.p1.x))
                    {
                        list.Add(point);
                        return new Secuencia(list);
                    }
                }
            }
            else if (f1 is Linea && f2 is Linea)
            {

                Linea l1 = (Linea)f1;
                Linea l2 = (Linea)f2;
                Point p = Linea.InterseccionDosLineas(l1, l2);
                if (p == null)
                {
                    return new Secuencia(list);
                }
                list.Add(p);
                return new Secuencia(list);
            }
            else if (f1 is Circle && f2 is Circle)
            {
                Circle c1 = (Circle)f1;
                Circle c2 = (Circle)f2;
                return Circle.InterseccionDosCircle(c1, c2);
            }
            else if ((f1 is Circle && f2 is Segment) || (f2 is Circle && f1 is Segment))
            {
                if (f1 is Circle && f2 is Segment)
                {
                    Circle c = (Circle)f1;
                    Segment s1 = (Segment)f2;
                    foreach (var p in Linea.InterseccionLineaCircle(c, s1).valores)
                    {
                        Point p1 = (Point)p;
                        if ((s1.p1.x < p1.x && p1.x < s1.p2.x) || (s1.p2.x < p1.x && p1.x < s1.p1.x))
                        {
                            list.Add(p1);

                        }
                    }
                    return new Secuencia(list);
                }
                else
                {
                    Circle c = (Circle)f2;
                    Segment s1 = (Segment)f1;
                    foreach (var p in Linea.InterseccionLineaCircle(c, s1).valores)
                    {
                        Point p1 = (Point)p;
                        if ((s1.p1.x < p1.x && p1.x < s1.p2.x) || (s1.p2.x < p1.x && p1.x < s1.p1.x))
                        {
                            list.Add(p1);

                        }
                    }
                    return new Secuencia(list);
                }
            }
            else if ((f1 is Circle && f2 is Ray) || (f2 is Circle && f1 is Ray))
            {
                if (f1 is Circle && f2 is Ray)
                {
                    Circle c = (Circle)f1;
                    Ray s1 = (Ray)f2;
                    foreach (var p in Linea.InterseccionLineaCircle(c, s1).valores)
                    {
                        Point point = (Point)p;
                        if ((s1.p1.x < s1.p2.x && point.x > s1.p1.x) || (s1.p1.x > s1.p2.x && point.x < s1.p1.x))
                        {
                            list.Add(p);

                        }
                    }
                    return new Secuencia(list);
                }
                else
                {
                    Circle c = (Circle)f2;
                    Ray s1 = (Ray)f1;
                    foreach (var p in Linea.InterseccionLineaCircle(c, s1).valores)
                    {
                        Point point = (Point)p;
                        if ((s1.p1.x < s1.p2.x && point.x > s1.p1.x) || (s1.p1.x > s1.p2.x && point.x < s1.p1.x))
                        {
                            list.Add(p);

                        }
                    }
                    return new Secuencia(list);
                }
            }
            else if ((f1 is Circle && f2 is Linea) || (f2 is Circle && f1 is Linea))
            {
                if (f1 is Circle && f2 is Linea)
                {
                    Circle c = (Circle)f1;
                    Linea l = (Linea)f2;
                    return Linea.InterseccionLineaCircle(c, l);
                }
                else
                {
                    Circle c = (Circle)f2;
                    Linea l = (Linea)f1;
                    return Linea.InterseccionLineaCircle(c, l);

                }
            }
            else if (f1 is Point || f2 is Point)
            {
                if (f1 is Point)
                {
                    Point p = (Point)f1;
                    if (p.Pertenece(f2))
                        list.Add(p);
                    return new Secuencia(list);
                }
                else
                {
                    Point p = (Point)f2;
                    if (p.Pertenece(f1))
                        list.Add(p);
                    return new Secuencia(list);

                }
            }






            return new Secuencia(list);


        }







    }
}
