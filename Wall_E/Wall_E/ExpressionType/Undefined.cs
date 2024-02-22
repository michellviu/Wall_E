using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walle
{
    public class Undefined : IType
    {   
        public string identificador { get; set; } 
        public ExpressionType expressiontype
        {
            get { return ExpressionType.Undefined; }
        }
        public Undefined(string identificador="")
        {
            this.identificador = identificador;
        }

    }
}
