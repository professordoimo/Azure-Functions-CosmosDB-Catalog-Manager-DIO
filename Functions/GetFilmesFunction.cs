using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using GerenciadorCatalogoNetflix.Models;
using Microsoft.Azure.Functions.Worker.Http;

namespace GerenciadorCatalogoNetflix.Functions
{
    public static class GetFilmesFunction
    {
        // Rota: GET /api/filmes/{id} (busca por ID) ou GET /api/filmes (busca geral)
        [Function("GetFilmes")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "filmes/{id?}")] HttpRequestData req,
            [CosmosDBInput("CatalogoDB", "Filmes",
                ConnectionStringSetting = "CosmosDbConnectionString",
                SqlQuery = "SELECT * FROM c WHERE c.id = {id}")] IEnumerable<Filme> filme,
            [CosmosDBInput("CatalogoDB", "Filmes",
                ConnectionStringSetting = "CosmosDbConnectionString",
                SqlQuery = "SELECT * FROM c")] IEnumerable<Filme> catalogo,
            string id,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processou um pedido de busca. ID: {id}");

            if (!string.IsNullOrEmpty(id))
            {
                // Busca por ID específica
                if (filme == null || !filme.Any())
                {
                    return new NotFoundObjectResult($"Filme com ID '{id}' não encontrado.");
                }
                return new OkObjectResult(filme.First());
            }
            else
            {
                // Lista todos
                return new OkObjectResult(catalogo);
            }
        }
    }
}
