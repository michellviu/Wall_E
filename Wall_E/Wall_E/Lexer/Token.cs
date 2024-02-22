namespace Walle;
public class Token
{
    public string Value { get; private set; }
    public TokenType Type { get; private set; }
    public CodeLocation Location { get; set; }

    public Token(TokenType type, string value, CodeLocation location)
    {
        this.Type = type;
        this.Value = value;
        this.Location = location;
    }

    public override string ToString()
        => string.Format("{0} [{1}]", Type, Value);
}

public struct CodeLocation
{
    public string File;
    public int Line;
    public int Column;
}


public enum TokenType
{
    Unknwon,
    Number,
    Text,
    Keyword,
    Identifier,
    Symbol,
    Shape
}

public class TokenValues
{
    protected TokenValues() { }

    public const string MayIg = ">="; // <=
    public const string MenIg = "<="; // >=
    public const string Equal = "=="; // ==
    public const string UnEqual = "!="; // !=
    public const string Add = "+"; // +
    public const string Sub = "-"; // -
    public const string Mul = "*"; // *
    public const string Div = "/"; // /
    public const string May = ">"; // <
    public const string Men = "<"; // >



    public const string Assign = "="; // =
    public const string ValueSeparator = ","; // ,
    public const string StatementSeparator = ";"; // ;

    public const string OpenBracket = "("; // (
    public const string ClosedBracket = ")"; // )
    public const string OpenCurlyBraces = "{"; // {
    public const string ClosedCurlyBraces = "}"; // }

    public const string point = "point";
    public const string circle = "circle";
    public const string line = "line";
    public const string segment = "segment";
    public const string ray = "ray";
    public const string arc = "arc";
    public const string sequence = "sequence";
    public const string color = "color";
    public const string restore = "restore";
    public const string draw = "draw";
    public const string import = "import";
    public const string measure = "measure";
    public const string print = "print";
    public const string let = "let";
    public const string in_ = "in";
    public const string if_ = "if";
    public const string else_ = "else";
    public const string then = "then";
}
