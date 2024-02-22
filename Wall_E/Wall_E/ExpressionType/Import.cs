using System;
using System.IO;

namespace Walle;
public class Import: IExpressionType
{
    public ExpressionType expressiontype { get; }
    private readonly string archivo;
    public Import( string archivo)
    {
        expressiontype = ExpressionType.Import;
        this.archivo = archivo;

    }


    public object Evaluate()
    {
        if (archivo.LastIndexOf(".geo") != archivo.Length - 4 || archivo.LastIndexOf(".geo") < 0)
            throw new Exception("Se esperaba una extensión de archivo del tipo: '.geo'");

        try
        {
            string code = File.ReadAllText(@".\Projects\" + archivo + ".txt");
            return code;
        }
        catch(FileNotFoundException)
        {
            throw new Exception("No se pudo encontrar el fichero: '" + archivo + "'");
        }  
    }


}
