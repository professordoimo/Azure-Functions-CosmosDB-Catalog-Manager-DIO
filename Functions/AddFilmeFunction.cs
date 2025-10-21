using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using GerenciadorCatalogoNetflix.Models;

namespace GerenciadorCatalogoNetflix.Functions
{
    public static class AddFilmeFunction
    {
        // O Output Binding (CosmosDBOutput) é a maneira mais simples de inserir no DB
        [Function("AddFilme")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "filmes")] HttpRequest req,
            [CosmosDBOutput("CatalogoDB", "Filmes", 
                ConnectionStringSetting = "CosmosDbConnectionString", 
                CreateLeaseContainerIfNotExists = false)] out Filme filmeOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processou um pedido para adicionar filme.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            // Tenta desserializar o corpo da requisição para o objeto Filme
            Filme novoFilme = JsonConvert.DeserializeObject<Filme>(requestBody);

            if (string.IsNullOrEmpty(novoFilme?.Titulo))
            {
                filmeOut = null; // Não insere nada
                return new BadRequestObjectResult("Por favor, passe o título do filme no corpo da requisição.");
            }

            // O objeto 'filmeOut' será inserido no Cosmos DB automaticamente
            filmeOut = novoFilme;

            return new CreatedResult("/api/filmes/" + novoFilme.Id, novoFilme);
        }
    }
}
