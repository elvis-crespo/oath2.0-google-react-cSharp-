using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly string clientId = "YOUR CLIENT ID";
        private readonly string clientSecret = "YOUR CLIENT SECRET";
        private readonly string redirectUri = "REDIRECT";

        // Send the URL to the front to authenticat
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var authorizationUrl = 
                $"https://accounts.google.com/o/oauth2/v2/auth" +
                $"?response_type=code&client_id={clientId}" +
                $"&redirect_uri={redirectUri}&scope=openid%20email%20profile";

            
            return Ok(new { url = authorizationUrl });
            //return Redirect(authorizationUr1l);
        }

        // Handling redirection from Google with authorization code
        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            var tokenResponse = await GetTokensAsync(code);

            if (tokenResponse == null)
            {
                return BadRequest("Error al obtener los tokens.");
            }
            var redirectUrl = $"http://localhost:5174/home?accessToken={tokenResponse.AccessToken}&idToken={tokenResponse.IdToken}";
            return Redirect(redirectUrl);
        }


        // Exchange the authorization code for an access token
        private async Task<TokenResponse> GetTokensAsync(string code)
        {
            using (var client = new HttpClient())
            {
                var tokenRequestData = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

                var response = await client.PostAsync("https://oauth2.googleapis.com/token", tokenRequestData);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TokenResponse>(responseJson);
                }

                return null;
            }
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string? Scope { get; set; }

        [JsonProperty("token_type")]
        public string? TokenType { get; set; }

        [JsonProperty("id_token")]
        public string? IdToken { get; set; }
    }
}
