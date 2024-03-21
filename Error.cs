namespace LA_Semantic;

public class Error : Exception
{
    public Error(string message, StreamWriter log) : base(message)
    {
        log.WriteLine("[!] " + message);
    }
        
    public Error(string message) : base(message){}
        
    public Error(string message, StreamWriter log, int line) : base(message + " en la linea " + line){
        log.WriteLine("[!] " + message + " en la linea " + line);
    }
}