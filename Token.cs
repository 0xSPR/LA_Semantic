namespace LA_Semantic;

public class Token
{
    public enum Types
    {
        Identifier, Number, Character, Assignment, EndSentence, LogicalOp,
        RelationalOp, TermOp, IncreaseTerm, FactorOp, FactorIn, TernaryOp,
        String, Start, End, DataTypes, Loops, Structures, Reserved
    }
    private string _content;
    private Types  _classification;
    
    // ReSharper disable once ConvertConstructorToMemberInitializers
    public Token()
    {
        _content = "";
        _classification = Types.Identifier;
    }
    public void SetContent(string content)
    {
        _content = content;
    }
    public void SetClassification(Types classification)
    {
        _classification = classification;
    }
    public string GetContent()
    {
        return _content;
    }
    public Types GetClassification()
    {
        return _classification;
    }
}