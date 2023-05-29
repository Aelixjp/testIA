using Microsoft.AspNetCore.Http;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using TestIA_Blazor.Pages;

namespace TestIA_Blazor.Services
{
    public class AnswerGeneratorService : IAnswerGeneratorService
    {

        public IEnumerable<string> SplitOnLength(string input, int length)
        {
            {
                int index = 0;
                string operation = "Analiza el código\n";

                while (index < input.Length)
                {
                    if (index + length < input.Length)
                        yield return input.Substring(index, length) + operation;
                    else
                        yield return input.Substring(index) + operation;

                    index += length;
                }
            }
        }

        public async Task<string> GenerateAnswer(string Prompt)
        {
            /*string apiKey = "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";
            var api = new OpenAIAPI(apiKey);

            // for example
            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,
                Temperature = 0.1,
                MaxTokens = 10000,
                Messages = new ChatMessage[] {
                    new ChatMessage(ChatMessageRole.User, "Hello!")
                }
            });

            var reply = result.Choices[0].Message;
            Console.WriteLine($"{reply.Role}: {reply.Content.Trim()}");

            return reply.Content.Trim();*/

            const int segmentantionLimit = 180;

            string apiKey = "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";
            string answer = string.Empty;
            string[] array = this.SplitOnLength(Prompt, segmentantionLimit).ToArray();

            foreach (string x in array)
            {
                var openai = new OpenAIAPI(apiKey);
                CompletionRequest completion = new CompletionRequest();

                completion.Prompt = x;
                completion.MaxTokens = 4000;

                var result = await openai.Completions.CreateCompletionAsync(completion);

                if (result != null)
                {
                    foreach (var item in result.Completions)
                    {
                        answer += item.Text + "\n\n";
                    }
                }
            }

            return answer;
        }

    }
}
