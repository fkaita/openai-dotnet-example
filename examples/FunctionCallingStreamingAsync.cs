using System.Text.Json;
using OpenAI.Responses;

namespace Examples;

/// <summary>
/// Demonstrates how to use OpenAI Responses API with streaming + function calling.
/// </summary>
public partial class ResponsesExamples
{
    public static async Task FunctionCallingStreamingAsync()
    {
        OpenAIResponseClient client = new(
            model: "gpt-4o-mini",
            apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

        // Ask user for a prompt
        Console.Write("Enter your question: ");
        string? input = Console.ReadLine();
        string question = string.IsNullOrWhiteSpace(input) 
            ? "What is the current weather in Tokyo?" 
            : input;

        // Define a tool that can be called by the model
        ResponseTool get_weather = ResponseTool.CreateFunctionTool(
            "get_weather",
            "Get the current weather of the location provided.",
            BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "location": {
                        "type": "string",
                        "description": "Name of the location"
                    }
                },
                "required": [
                    "location"
                ]
            }
            """)
        );

        string? previousResponseId = null;
        string? functionCallId = null;
        string? functionName = null;
        JsonDocument? functionArguments = null;

        // First turn: ask the model and capture tool call
        ResponseCreationOptions options = new() { Tools = { get_weather } };

        await foreach (StreamingResponseUpdate update
            in client.CreateResponseStreamingAsync( question, options ))
        {
            if (update is StreamingResponseCreatedUpdate responseCreatedUpdate)
            {
                Console.WriteLine($"Response Created: {responseCreatedUpdate.Response.Id}");
                previousResponseId = responseCreatedUpdate.Response.Id;
            }
            else if (update is StreamingResponseOutputItemDoneUpdate itemUpdate && itemUpdate.Item is FunctionCallResponseItem functionCallUpdate)
            {
                functionCallId = functionCallUpdate.CallId;
                functionName = functionCallUpdate.FunctionName;
                functionArguments = JsonDocument.Parse(functionCallUpdate.FunctionArguments);
                Console.WriteLine("\n--- Function Call Details ---");
                Console.WriteLine($"Function ID: {functionCallId}");
                Console.WriteLine($"Function Name: {functionName}");
            }
            else if (update is StreamingResponseOutputTextDeltaUpdate outputTextUpdate)
            {
                Console.Write(outputTextUpdate.Delta ?? "");
            }
        }

        // Handle function execution if the function was called
        if (!string.IsNullOrEmpty(functionCallId))
        {
            string functionResult = "";
            if (functionName == "get_weather"){
                if (functionArguments != null && functionArguments.RootElement.TryGetProperty("location", out var location))
                {
                    functionResult = await GetCurrentWeatherTool(location.GetString() ?? "unknown location");
                }
                else
                {
                    functionResult = "No location was given.";
                }
            }
            else
            {
                functionResult = "Function not implemented.";
            }

            Console.WriteLine("\n--- Function Call Result ---");
            Console.WriteLine(functionResult);

            ResponseItem functionReply = ResponseItem.CreateFunctionCallOutputItem(functionCallId, functionResult);
            ResponseCreationOptions secondOptions = new() { PreviousResponseId = previousResponseId };

            Console.WriteLine("\n--- Streaming Response After Function Call ---");
            await foreach (StreamingResponseUpdate secondUpdate in client.CreateResponseStreamingAsync( [functionReply], secondOptions ))
            {
                if (secondUpdate is StreamingResponseOutputTextDeltaUpdate secondMessageDeltaUpdate)
                {
                    Console.Write(secondMessageDeltaUpdate.Delta ?? "");
                }
            }
        }
    }

    // Custom function used by the model
    public static Task<string> GetCurrentWeatherTool(string location)
    {
        // Implement actual API or simulated result here.
        return Task.FromResult($"The current weather in {location} is sunny!");
    }
}
