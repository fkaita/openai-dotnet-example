# OpenAI .NET Response API Streaming with function calling Example

This repository demonstrates how to use the [OpenAI .NET SDK](https://github.com/openai/openai-dotnet) with the new **Response API** to streaming replies and handle function calling in C#.

- [SingleFunctionCallingStreamingAsync.cs](examples/SingleFunctionCallingStreamingAsync.cs): Single Function Calling with Streaming Response

- [MultiFunctionCallingStreamingAsync.cs](examples/MultiFunctionCallingStreamingAsync.cs): Multiple Function Calling with Streaming Response

## 🛠 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- OpenAI API key with access to the `response` endpoint and `gpt-4o-mini` model

## 📦 Setup

**Set your API key as an environment variable**

macOS / Linux:

```bash
export OPENAI_API_KEY=sk-...
```

Windows (PowerShell):

```powershell
$env:OPENAI_API_KEY = "sk-..."
```

## 🚀 Run Examples

Run with one of the supported arguments:

- **`single`** – The model can make **only one function call** in response to the user’s question.

  ```bash
  dotnet run -- single
  ```

- **`multi`** – The model can make **multiple function calls** across multiple steps.

  ```bash
  dotnet run -- multi
  ```

You will be prompted to enter a question like:

```bash
Enter your question: What is the weather in Tokyo?
```
