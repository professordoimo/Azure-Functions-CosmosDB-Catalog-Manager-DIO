using Newtonsoft.Json;

namespace GerenciadorCatalogoNetflix.Models
{
    public class Filme
    {
        [JsonProperty("id")] // Mapeia para o campo 'id' padrão do Cosmos DB
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("titulo")]
        public string Titulo { get; set; }

        [JsonProperty("genero")]
        public string Genero { get; set; }

        [JsonProperty("anoLancamento")]
        public int AnoLancamento { get; set; }

        [JsonProperty("sinopse")]
        public string Sinopse { get; set; }
    }
}
