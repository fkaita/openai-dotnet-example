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
                await ResponsesExamples.SingleFunctionCallingStreamingAsync();
                break;
            case "multi":
                await ResponsesExamples.MultiFunctionCallingStreamingAsync();
                break;
            case "file":
                await ResponsesExamples.FileUploadExample();
                break;
            default:
                Console.WriteLine($"Unknown example: {args[0]}");
                break;
        }
    }
}
