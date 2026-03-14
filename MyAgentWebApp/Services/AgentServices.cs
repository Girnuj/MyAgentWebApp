using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace MyAgentWebApp.Services;

public sealed class AgentServices(Kernel kernel)
{
    private static OpenAIPromptExecutionSettings _executionSettings = new()
    {
        //FunctionChoiceBehavior = FunctionChoiceBehaviors.Auto(),
        ChatSystemPrompt = """
            Instrucciones obligatorias: responde siempre en español;
            si el usuario saluda, responde con un saludo cordial;
            busca primero en tu memoria o base de conocimiento y,
            si hay una respuesta completa y verificada,
            usala tal cual; si no existe, usa los plugins adecuados
            y si estos devuelven una respuesta completa y verificada, usala tal cual;
            si ni memoria ni plugins contienen la respuesta no generes una respuesta por tu cuenta;
            siempre siempre el orden Memoria->Plugins y nunca combines fuentes;
        """
    };

    private static KernelArguments _args = new(_executionSettings);

    public async Task<string> Chat(string message)
    {
        var result = await kernel.InvokePromptAsync(message, _args);
        return result.GetValue<string>();
    }
}
