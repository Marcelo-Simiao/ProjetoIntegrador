using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ProjetoIntegrador.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoIntegrador.Services
{
    public class FactorialService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly ILogger<FactorialService> _logger;

        public FactorialService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<FactorialService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _baseUrl = configuration["Factorial:BaseUrl"] ?? throw new ArgumentNullException("BaseUrl não configurado no appsettings.json.");
            _apiKey = configuration["Factorial:ApiKey"] ?? throw new ArgumentNullException("ApiKey não configurado no appsettings.json.");
            _logger = logger;
        }

        // Método para testar a conexão com a API Factorial
        public async Task TestarConexaoAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Conexão com a API Factorial foi bem-sucedida!");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao conectar: {response.StatusCode} - {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao testar a conexão com a API Factorial.");
            }
        }

        // Método para buscar funcionários da API Factorial
        public async Task<List<Funcionario>> GetFuncionariosAsync(int page = 1, int limit = 50, string status = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/employees");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                // Adicionar parâmetros de consulta
                var uriBuilder = new UriBuilder(request.RequestUri);
                var query = new List<string>
                {
                    $"page={page}",
                    $"limit={limit}"
                };
                if (!string.IsNullOrEmpty(status))
                {
                    query.Add($"status={status}");
                }

                uriBuilder.Query = string.Join("&", query);
                request.RequestUri = uriBuilder.Uri;

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode || response.Content == null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Erro ao buscar funcionários: {response.StatusCode} - {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Funcionario>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Funcionario>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar funcionários da API Factorial.");
                throw; // Repropaga a exceção
            }
        }

        // Método para buscar candidatos da API Factorial
        public async Task<List<Candidate>> GetCandidatesAsync(int page = 1, int limit = 50, string status = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/resources/ats/candidates");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                // Adicionar parâmetros de consulta
                var uriBuilder = new UriBuilder(request.RequestUri);
                var query = new List<string>
                {
                    $"page={page}",
                    $"limit={limit}"
                };
                if (!string.IsNullOrEmpty(status))
                {
                    query.Add($"status={status}");
                }

                uriBuilder.Query = string.Join("&", query);
                request.RequestUri = uriBuilder.Uri;

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode || response.Content == null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Erro ao buscar candidatos: {response.StatusCode} - {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Candidate>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Candidate>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar candidatos da API Factorial.");
                throw; // Repropaga a exceção
            }
        }
    }
}
