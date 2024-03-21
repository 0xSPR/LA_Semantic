namespace LA_Semantic;

public class Variable
{
    public enum DataType
    {
        Char,Int,Float
    }
    private string _name;
    private DataType _type;
    private float _value;
    // ReSharper disable once ConvertToPrimaryConstructor
    public Variable(string name,DataType type)
    {
        this._name = name;
        this._type = type;
        this._value = 0;
    }
    public void SetValue(float value)
    {
        this._value = value;
    }
    public string GetName()
    {
        return this._name;
    }

    /*private DataType GetType()
    {
        return this._type;
    }*/
    public float GetValue()
    {
        return this._value;
    }
}