namespace Walle;
public class Lexer
{
    private static LexicalAnalyzer? __LexicalProcess;
    public static LexicalAnalyzer LexicalAnalyzer
    {
        get
        {
            if (__LexicalProcess == null)
            {
                __LexicalProcess = new LexicalAnalyzer();

                __LexicalProcess.Operators["<="] = TokenValues.MenIg;
                __LexicalProcess.Operators[">="] = TokenValues.MayIg;
                __LexicalProcess.Operators["=="] = TokenValues.Equal;
                __LexicalProcess.Operators["!="] = TokenValues.UnEqual;
                __LexicalProcess.Operators["+"] = TokenValues.Add;
                __LexicalProcess.Operators["*"] = TokenValues.Mul;
                __LexicalProcess.Operators["-"] = TokenValues.Sub;
                __LexicalProcess.Operators["/"] = TokenValues.Div;
                __LexicalProcess.Operators["="] = TokenValues.Assign;
                __LexicalProcess.Operators["<"] = TokenValues.Men;
                __LexicalProcess.Operators[">"] = TokenValues.May;
                

                __LexicalProcess.Operators[","] = TokenValues.ValueSeparator;
                __LexicalProcess.Operators[";"] = TokenValues.StatementSeparator;
                __LexicalProcess.Operators["("] = TokenValues.OpenBracket;
                __LexicalProcess.Operators[")"] = TokenValues.ClosedBracket;
                __LexicalProcess.Operators["{"] = TokenValues.OpenCurlyBraces;
                __LexicalProcess.Operators["}"] = TokenValues.ClosedCurlyBraces;

                __LexicalProcess.Shapes["point"] = TokenValues.point;
                __LexicalProcess.Shapes["circle"] = TokenValues.circle;
                __LexicalProcess.Shapes["line"] = TokenValues.line;
                __LexicalProcess.Shapes["segment"] = TokenValues.segment;
                __LexicalProcess.Shapes["ray"] = TokenValues.ray;
                __LexicalProcess.Shapes["arc"] = TokenValues.arc;

                __LexicalProcess.KeyWords["color"] = TokenValues.color;
                __LexicalProcess.KeyWords["restore"] = TokenValues.restore;
                __LexicalProcess.KeyWords["draw"] = TokenValues.draw;
                __LexicalProcess.KeyWords["import"] = TokenValues.import;
                __LexicalProcess.KeyWords["print"] = TokenValues.print;
                __LexicalProcess.KeyWords["let"] = TokenValues.let;
                __LexicalProcess.KeyWords["in"] = TokenValues.in_;
                __LexicalProcess.KeyWords["sequence"] = TokenValues.sequence;
                __LexicalProcess.KeyWords["if"] = TokenValues.if_;
                __LexicalProcess.KeyWords["else"] = TokenValues.else_;
                __LexicalProcess.KeyWords["then"] = TokenValues.then;

                /*  __LexicalProcess.KeyWords["measure"] = TokenValues.measure;
                __LexicalProcess.KeyWords["intersect"] = TokenValues.intersect;
                __LexicalProcess.KeyWords["count"] = TokenValues.count;
                __LexicalProcess.KeyWords["randoms"] = TokenValues.randoms;
                __LexicalProcess.KeyWords["points"] = TokenValues.points;
                __LexicalProcess.KeyWords["samples"] = TokenValues.samples;
                */

                __LexicalProcess.Texts["\""] = "\"";
            }

            return __LexicalProcess;
        }
    }

}
