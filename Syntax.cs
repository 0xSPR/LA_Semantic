namespace LA_Semantic;

public class Syntax : Lexic
{
    public Syntax()
    {
        NextToken();
    }
    
    public Syntax(string name) : base(name)
    {
        NextToken();
    }
    public void Match(string wait)
    {
        if (GetContent() == wait)
            NextToken();
        else
            throw new Error("Syntax:" + " [" + Line + "]" + " Waiting For " + wait, Log);
    }
    public void Match(Types wait)
    {
        if (GetClassification() == wait)
            NextToken();
        else
            throw new Error("Syntax:" + " [" + Line + "]" + " Waiting For " + wait + ".");
    }
}