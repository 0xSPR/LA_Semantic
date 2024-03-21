namespace LA_Semantic;

class Program
{
    static void Main() {
        try{
            using Language l = new Language();
            l.Program();
        } 
        catch (Exception e){
            Console.WriteLine("Error " + e.Message);
        }
    }
}