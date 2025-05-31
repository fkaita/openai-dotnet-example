using System;
using System.IO;
using System.Threading.Tasks;
using OpenAI.Files;
using OpenAI.Responses;

namespace Examples
{
    /// <summary>
    /// Demonstrates how to upload a file to OpenAI and use it in a chat prompt.
    /// </summary>
    public partial class ResponsesExamples
    {
        public static async Task FileUploadExample()
        {
            string filePath = "data/dummy.pdf";
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            var fileData = new BinaryData(fileBytes);

            // Upload file to OpenAI
            OpenAIFileClient fileClient = new(
                apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
            OpenAIFile uploadResponse = fileClient.UploadFile(
                fileData,
                Path.GetFileName(filePath),
                FileUploadPurpose.UserData);

            Console.WriteLine($"Uploaded File ID: {uploadResponse.Id}");

            // Create chat prompt referencing the uploaded file
            OpenAIResponseClient client = new(
                model: "gpt-4o-mini",
                apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            OpenAIResponse response = await client.CreateResponseAsync(
                [
                    ResponseItem.CreateUserMessageItem(
                        [
                            ResponseContentPart.CreateInputTextPart("What is written in the provided file?"),
                            ResponseContentPart.CreateInputFilePart(uploadResponse.Id, null, null)
                        ])
                ]);

            Console.WriteLine($"[Response]: {response.GetOutputText()}");
        }
    }
}
