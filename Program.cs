using Examples;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run -- [single|multi]");
            return;
        }

        switch (args[0].ToLower())
        {
            case "single":
                await ResponsesExamples.FunctionCallingStreamingAsync();
                break;
            // case "multi":
            //     MathExample.Run();
            //     break;
            default:
                Console.WriteLine($"Unknown example: {args[0]}");
                break;
        }
    }
}
