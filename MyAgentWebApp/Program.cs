using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using MyAgentWebApp.Plugins;
using MyAgentWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

//var apiKey = builder.Configuration.GetValue<string>("AppSecretsKeys:YourApiKey");
//var modelID = builder.Configuration.GetValue<string>("AppSecretsKeys:YourModelID");
var apiKeyHardCode= "chinchulin";
var modelIDHardCode = "chatgp-4o";
var qdranUrl = "http://localhost:6333";

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//qdran para vectorizar y almacenar la memoria de los agentes para implementar RAG se levanta con docker
//es solo una muestra
builder.Services.AddKernelMemory<MemoryServerless>(kernelBulder =>
{
    kernelBulder.WithOpenAIDefaults(apiKeyHardCode)
    .WithQdrantMemoryDb(qdranUrl);
}, new KernelMemoryBuilderBuildOptions()
{
    AllowMixingVolatileAndPersistentData = true
});

builder.Services.AddKernel()
    .AddOpenAIChatCompletion(modelIDHardCode, apiKeyHardCode)
    .Plugins.AddFromType<InventoryPlugin>()
            .AddFromType<ProductionPlugin>()
            .AddFromType<MemoryPlugin>();

builder.Services.AddScoped<AgentServices>();
builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if(bool.TryParse(Environment.GetEnvironmentVariable("VectorizeAtStartup"),out bool vectorize) && vectorize)
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var kernelMemory = scope.ServiceProvider.GetRequiredService<IKernelMemory>();
            //se vectoriza toda la pagina web de ejemplo y se guarda en qdran por medio de scraping para que el agente pueda responder preguntas sobre su contenido
            await kernelMemory.ImportWebPageAsync("https://www.bbc.com/news/world-us-canada-66857158");
            //agregar documentos
            //await kernelMemory.ImportDocumentAsync("filephat");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"El proceso de importacion a fallado: {ex.Message}");
        }
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
