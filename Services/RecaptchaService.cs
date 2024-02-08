using ProveedorApi.Models;
using Newtonsoft.Json;

namespace ProveedorApi.Services;

public class RecaptchaService
{
    private const string RecaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";
    private readonly string _secret;

    public RecaptchaService(string secret)
    {
        _secret = secret;
    }

    public async Task<RecaptchaResponse> ValidateRecaptcha(string token)
    {
        using var httpClient = new HttpClient();
        var postData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("secret", _secret),
            new KeyValuePair<string, string>("response", token)
        });

        var response = await httpClient.PostAsync(RecaptchaVerifyUrl, postData);
        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<RecaptchaResponse>(jsonString);
    }
}