using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walle;
public class Booleano : IType
{
    public string identificador { get; set; }
    public bool valor { get; set; }

    public Booleano(bool valor, string identificador = "")
    {
        this.valor = valor;
        this.identificador = identificador;

    }
}

