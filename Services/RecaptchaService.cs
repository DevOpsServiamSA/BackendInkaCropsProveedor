using ProveedorApi.Models;
using Newtonsoft.Json;

namespace ProveedorApi.Services;

public class RecaptchaService
{
    private readonly string _recaptchaVerifyUrl;
    private readonly string _secret;

    public RecaptchaService(string secret, string url)
    {
        _secret = secret;
        _recaptchaVerifyUrl = url;
    }

    public async Task<RecaptchaResponse> ValidateRecaptcha(string token)
    {
        using var httpClient = new HttpClient();
        var postData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("secret", _secret),
            new KeyValuePair<string, string>("response", token)
        });

        var response = await httpClient.PostAsync(_recaptchaVerifyUrl, postData);
        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<RecaptchaResponse>(jsonString);
    }
}