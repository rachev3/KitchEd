using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace KitchEd.Services
{
    public class RecaptchaService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public RecaptchaService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyRecaptchaAsync(string recaptchaResponse)
        {
            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                return false;
            }

            var secretKey = _configuration["reCAPTCHA:SecretKey"];
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", secretKey),
                new KeyValuePair<string, string>("response", recaptchaResponse)
            });

            var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var responseString = await response.Content.ReadAsStringAsync();

            // For debugging
            Console.WriteLine($"reCAPTCHA response: {responseString}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var recaptchaResult = JsonSerializer.Deserialize<RecaptchaResponse>(responseString, options);

            // For debugging
            Console.WriteLine($"Deserialized success value: {recaptchaResult?.Success}");

            return recaptchaResult?.Success ?? false;
        }

        private class RecaptchaResponse
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("error-codes")]
            public string[] ErrorCodes { get; set; }

            [JsonPropertyName("challenge_ts")]
            public string ChallengeTimestamp { get; set; }

            [JsonPropertyName("hostname")]
            public string Hostname { get; set; }
        }
    }
}