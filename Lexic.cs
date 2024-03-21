namespace LA_Semantic;

public class Lexic : Token, IDisposable
{
    const int F = -1;
    const int E = -2;
    protected int Line;
        
    private readonly StreamReader _file;
    protected StreamWriter Log;

    private readonly int[,] _trand =
    {
        //WS L	D	.	E	La	=	;	&	|	!	>	<	+	-	%	*	?	"	EOF	 /	EOL	{	}
        {0, 1,  2,  27, 1,  27, 8,  10, 11, 12, 13, 16, 17, 19, 20, 22, 22, 24, 25, F,  28, 0, 32, 33}, //0
        {F, 1,  1,  F,  1,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //1
        {F, F,  2,  3,  5,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //2
        {E, E,  4,  E,  E,  E,  F,  F,  F,  F,  F,  F,  F,  E,  E,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //3
        {F, F,  4,  F,  5,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //4
        {E, E,  7,  E,  E,  E,  F,  F,  F,  F,  F,  F,  F,  6,  6,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //5
        {E, E,  7,  E,  E,  E,  F,  F,  F,  F,  F,  F,  F,  E,  E,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //6
        {F, F,  7,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //7
        {F, F,  F,  F,  F,  F,  9,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //8
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //9
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //10
        {F, F,  F,  F,  F,  F,  F,  F,  14, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //11
        {F, F,  F,  F,  F,  F,  F,  F,  F,  14, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //12
        {F, F,  F,  F,  F,  F,  15, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //13
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //14
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //15
        {F, F,  F,  F,  F,  F,  18, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //16
        {F, F,  F,  F,  F,  F,  18, F,  F,  F,  F,  18, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //17
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //18
        {F, F,  F,  F,  F,  F,  21, F,  F,  F,  F,  F,  F,  21, F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //19
        {F, F,  F,  F,  F,  F,  21, F,  F,  F,  F,  F,  F,  F,  21, F,  F,  F,  F,  F,  F,  F,  F,  F}, //20
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //21
        {F, F,  F,  F,  F,  F,  23, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //22
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //23
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //24
        {25,25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 26, E,  F,  F, 25, 25}, //25
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //26
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //27
        {F, F,  F,  F,  F,  F,  23, F,  F,  F,  F,  F,  F,  F,  F,  F,  30, F,  F,  F,  29, F,  F,  F}, //28
        {29,29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 0, 29, 29}, //29
        {30,30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 31, 30, 30, E, 30, 30, 30, 30}, //30
        {30,30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 31, 30, 30, E,  0, 30, 30, 30}, //31
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F,  32,  F}, //32
        {F, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F,  F,  33}  //33
        //WS L	D	.	E	La	=	;	&	|	!	>	<	+	-	%	*	?	"	EOF	 /	EOL	{	} 
        //0  1  2   3   4   5   6   7   8   9  10   11  12  13  14  15  16  17  18  19  20  21  22  23
    };

    public Lexic()
    {
        _file = new StreamReader("test.cpp");
        Log = new StreamWriter("test.log");
        Log.AutoFlush = true;
        Line = 1;
    }
    public Lexic(string name)
    {
        _file = new StreamReader(name);
        Log = new StreamWriter("test.log");
        Log.AutoFlush = true;
        Line = 1;
    }
    public void Dispose()
    {
        _file.Close();
        Log.Close();
    }
    private int Column(char c)
    {
        if (c == '\n')
            return 21;
        if (Eof())
            return 19;
        if (char.IsWhiteSpace(c))
            return 0;
        if (char.ToLower(c) == 'e')
            return 4;
        if (char.IsLetter(c))
            return 1;
        if (char.IsDigit(c))
            return 2;
        if (c == '.')
            return 3;
        if (c == '+')
            return 13;
        if (c == '-')
            return 14;
        if (c == '=')
            return 6;
        if (c == ';')
            return 7;
        if (c == '&')
            return 8;
        if (c == '|')
            return 9;
        if (c == '!')
            return 10;
        if (c == '>')
            return 11;
        if (c == '<')
            return 12;
        if (c == '*')
            return 16;
        if (c == '%')
            return 15;
        if (c == '/')
            return 20;
        if (c == '{')
            return 22;
        if (c == '}')
            return 23;
        if (c == '?')
            return 17;
        if (c == '"')
            return 18;
        return 5;
    }
    private void Classify(int state)
    {
        switch (state)
        {
            case  1: SetClassification(Types.Identifier); break;
            case  2: SetClassification(Types.Number); break;
            case  8: SetClassification(Types.Assignment); break;
            case  9: SetClassification(Types.RelationalOp); break;
            case 10: SetClassification(Types.EndSentence); break;
            case 11: SetClassification(Types.Character); break;
            case 12: SetClassification(Types.Character); break;
            case 13: SetClassification(Types.LogicalOp); break;
            case 14: SetClassification(Types.LogicalOp); break;
            case 15: SetClassification(Types.RelationalOp); break;
            case 16: SetClassification(Types.RelationalOp); break;
            case 17: SetClassification(Types.RelationalOp); break;
            case 19: SetClassification(Types.TermOp); break;
            case 20: SetClassification(Types.TermOp); break;
            case 21: SetClassification(Types.IncreaseTerm); break;
            case 22: SetClassification(Types.FactorOp); break;
            case 23: SetClassification(Types.FactorIn); break;
            case 24: SetClassification(Types.TernaryOp); break;
            case 25: SetClassification(Types.String); break;
            case 26: SetClassification(Types.String); break;
            case 27: SetClassification(Types.Character); break;
            case 28: SetClassification(Types.FactorOp); break;
            case 32: SetClassification(Types.Start); break;
            case 33: SetClassification(Types.End); break;
        }
    }
    public void NextToken()
    {
        string buffer = "";
        int state = 0;

        while (state >= 0)
        {
            var c = (char)_file.Peek();

            state = _trand[state, Column(c)];
            Classify(state);

            if (state >= 0)
            {
                if (c == '\n')
                    Line++;
                
                _file.Read();
                
                if (state > 0)
                    buffer += c;
                else
                    buffer = "";
            }
        }
        if (state == E)
        {
            if (GetClassification() == Types.Number)
                throw new Error("Lexico: Se espera un digito", Log, Line);
            if (GetClassification() == Types.String)
                throw new Error("Lexico: Se espera un \"", Log, Line);
        }
        else
        {
            SetContent(buffer);
            if (GetClassification() == Types.Identifier)
            {
                switch (GetContent())
                {
                    case "int":
                    case "float":
                    case "double":
                    case "long":
                    case "byte":
                    case "decimal":
                    case "short":
                    case "char":
                    case "bool":
                    case "string":
                        SetClassification(Types.DataTypes);
                        break;
                    case "while":
                    case "do":
                    case "for":
                    // case "if":
                    // case "else":
                        SetClassification(Types.Loops);
                        break;
                    case "if":
                    case "else":
                    case "switch":
                    case "break":
                        SetClassification(Types.Structures);
                        break;
                    case "main":
                        SetClassification(Types.Reserved);
                        break;
                }
            }
            Log.WriteLine(GetContent() + " = " + GetClassification());
        }
    }
    public bool Eof()
    {
        return _file.EndOfStream;
    }
}