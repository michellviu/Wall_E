namespace Walle;
public class Number : IType
{
    public string identificador { get; set; }
    public double value { get; set; }

    public Number(double number, string identificador = "")
    {
        value = number;
    }


}
