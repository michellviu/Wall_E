using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walle;

class Type : IExpressionType
{
    public ExpressionType expressiontype => ExpressionType.Type;
    public IType valor { get; set; } 


    public Type(IType objeto)
    {
        valor = objeto;
    }

    public dynamic Evaluate()
    {
        return valor;
    }
}

