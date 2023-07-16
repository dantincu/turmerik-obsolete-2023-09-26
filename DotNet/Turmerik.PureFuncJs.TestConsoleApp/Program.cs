using Jint;

var engine = new Engine();

while (true)
{
    Console.Write("> ");
    var statement = Console.ReadLine();
    var result = engine.Execute(statement).GetCompletionValue();
    Console.WriteLine(result);
}