/*Requerimento 1: Marcar errores sintacticos para variables no declaradas
Requerimiento 2: Asignacion, modificar el valor de la variable, no pasar por alto el ++ y el --
Requerimiento 3: Printf: Implementar secuencias de escape, quitar comillas
Requerimiento 4: Modificar el valor de la variable en el Scanf y levantar una excepción si lo capturado no es un numero
Requerimiento 5: Implementar el casteo
requerimiento 6: /n /t
[*]	Requerimento 1: Evalua el "else"
[*]	Requeriminto 2: Incrementar la variable del for (incremento) al final de la ejecución
[*]	Requeriminto 3: Hacer el Do
[*]	Requeriminto 4: Hacer el While 
*/

namespace LA_Semantic;

public class Language : Syntax
{
    public readonly List<Variable> Variables;
    public Language()
    {
        Variables = new List<Variable>();
    }
    public Language(string name) : base(name)
    {
        Variables = new List<Variable>();
    }
    //Programa  -> Librerias? Variables? Main
    public void Program()
    {
        Library();
        Variable();
        Main();
        //PrintVariable();
    }
    private void PrintVariable()
    {
        Log.WriteLine("Variables: ");
        foreach (Variable v in Variables)
        {
            Log.WriteLine(v.GetName() +" = "+v.GetType() +" = " + v.GetValue());
        }
    }
    /*private bool VariableExists(string nombre)
    {
        foreach (Variable v in _variables)
        {
                
            if(nombre == v.GetName())
            {
                return true;
            }
        }
        return false;
    }*/
    //Librerias -> #include<identificador(.h)?> Librerias?
    private void Library()
    {
        if (GetContent() == "#")
        {
            Match("#");
            Match("include");
            Match("<");
            Match(Types.Identifier);

            if (GetContent() == ".")
            {
                Match(".");
                Match("h");
            }

            Match(">");
            Library();
        }
    }
    //Variables -> tipoDato listaIdentificadores; Variables?
    private void Variable()
    {
        if (GetClassification() == Types.DataTypes)
        {
            string type = GetContent();
            Match(Types.DataTypes);
            Match(Types.Identifier);

            switch (GetContent())
            {
                case ",":
                    while (GetClassification() == Types.Identifier || GetContent() == ",")
                    {
                        Match(",");
                        Match(Types.Identifier);
                    }
                    break;
                case "=":
                    Match("=");

                    switch (type)
                    {
                        case "int":
                        case "float":
                        case "double":
                        case "byte":
                        case "decimal":
                        case "short":
                            Match(Types.Number);
                            break;
                        case "char":
                        case "bool":
                        case "string":
                            Match(Types.String);
                            break;
                    }
                break;
                default:
                    Match(",");
                    break;
            }

            Match(";");
            Variable();
        }
    }
    //listaIdentificadores -> Identificador (,listaIdentificadores)?
    /*private void IdentifierList(Variable.DataType tipo)
    {
        string nombre = GetContent();
        Match(Types.Identifier);
        if(!VariableExists(nombre))
        {
            _variables.Add(new Variable(nombre,tipo));

        }
        else
        {
            throw new Error("de Sintaxis : la variable " + nombre + " ya existe",Log,Line);
        }
        if (GetContent() == ",")
        {
            Match(",");
            IdentifierList(tipo);
        }
    }*/
    //bloqueInstrucciones -> { listaIntrucciones? }
    private void InstructionBlock()
    {
        Match("{");

        if (GetContent() != "}")
            InstructionList();

        Match("}");
    }
    //ListaInstrucciones -> Instruccion ListaInstrucciones?
    private void InstructionList()
    {
        Instructions();

        if (GetContent() != "}")
            InstructionList();
    }
    //Instruccion -> Printf | Scanf | If | While | do while | For | Asignacion
    private void Instructions()
    {
        if (GetContent() == "printf")
            Printf();
        else if (GetContent() == "scanf")
            Scanf();
        else if (GetContent() == "if")
            If();
        else if (GetContent() == "while")
            While();
        else if (GetContent() == "do")
            Do();
        else if (GetContent() == "for")
            For();
        else
            Assignment();
    }
    //    Requerimiento 1: Printf -> printf(cadena(, Identificador)?);
    private void Printf()
    {
        Match("printf");
        Match("(");
        Match(Types.String);
        while (GetContent() == ",")
        {
            Match(",");
            Match(Types.Identifier);
        }
        Match(")");
        Match(";");

    }
    //    Requerimiento 2: Scanf -> scanf(cadena,&Identificador);
    private void Scanf()
    {
        Match("scanf");
        Match("(");
        Match(Types.String);
        Match(",");
        Match("&");
        Match(Types.Identifier);
        Match(")");
        Match(";");
    }

    //Asignacion -> Identificador (++ | --) | (+= | -=) Expresion | (= Expresion) ;
    private void Assignment()
    {
        
        Match(Types.Identifier);
        
        if (GetClassification() == Types.IncreaseTerm)
        {
            Match(Types.IncreaseTerm);

            if(GetContent() == "(")
                Expression();
        }
        else if (GetClassification() == Types.FactorIn)
        {
            Match(Types.FactorIn);
            Expression();
        }
        else
        {
            Match("=");
            Expression();
        }

        Match(";");
    }
    //If -> if (Condicion) instruccion | bloqueInstrucciones 
    //      (else instruccion | bloqueInstrucciones)?
    private void If()
    {
        Match("if");
        Match("(");
        Condition();
        Match(")");
        if (GetContent() == "{")
            InstructionBlock();
        else
            Instructions();

        if (GetContent() == "else")
        {
            if (GetContent() == "{")
                InstructionBlock();
            else
                Instructions();
        }
    }
    //Condicion -> Expresion operadoRelacional Expresion
    private void Condition()
    {
        Expression();
        Match(Types.RelationalOp);
        Expression();
    }
    //While -> while(Condicion) bloqueInstrucciones | Instruccion
    private void While()
    {
        Match("while");
        Match("(");
        Condition();
        Match(")");

        if (GetContent() == "{")
            InstructionBlock();
        else
            Instructions();
    }
    //Do -> do bloqueInstrucciones | Intruccion while(Condicion);
    private void Do()
    {
        Match("do");

        if (GetContent() == "{")
            InstructionBlock();
        else
            Instructions();

        Match("while");
        Match("(");
        Condition();
        Match(")");
        Match(";");
            
    }
    //For* -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Instruccion 
    private void For()
    {
        Match("for");
        Match("(");

        if (GetClassification() == Types.DataTypes)
            Variable();
        else
        {
            Match(Types.Identifier);
            Match("=");
            Match(Types.Number);
        }

        Match(";");
        Condition();
        Match(";");
        Increase();
        Match(")");

        if (GetContent() == "{")
            InstructionBlock();
        else
            Instructions();
    }
    //Incremento -> Identificador ++ | --
    private void Increase()
    {
        if (GetClassification() == Types.Identifier)
        {
            Match(Types.Identifier);
            Match(Types.IncreaseTerm);
        }
        else
        {
            Match(Types.IncreaseTerm);
            Match(Types.Identifier);
        }
    }
    //Main      -> void main() bloqueInstrucciones
    private void Main()
    {
        if (GetContent() == "void")
        {
            Match("void");
            Match("main");

            //Parameters();
            Match("(");
            Match(")");
            InstructionBlock();
        }
    }
    //Expresion -> Termino MasTermino
    private void Expression()
    {   
        Term();
        PlusTerm();
    }
    //MasTermino -> (OperadorTermino Termino)?
    private void PlusTerm()
    {
        if (GetClassification() == Types.TermOp)
        {
            Match(Types.TermOp);
            Term();
        }
    }
    //Termino -> Factor PorFactor
    private void Term()
    {
        Factor();
        ByFactor();

    }
    //PorFactor -> (OperadorFactor Factor)?
    private void ByFactor()
    {
        if (GetClassification() == Types.FactorOp)
        {
            Match(Types.FactorOp);
            Factor();
        }
    }
    //Factor -> numero | identificador | (Expresion)
    private void Factor()
    {
        if (GetClassification() == Types.Number)
            Match(Types.Number);
        else if (GetClassification() == Types.Identifier)
            Match(Types.Identifier);
        else
        {
            Match("(");
            Expression();
            Match(")");
        }
    }
}