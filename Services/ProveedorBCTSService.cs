using System.Net;
using System.Text;
using Newtonsoft.Json;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class ProveedorBCTSService
{
    private readonly string _urlGetTokenBCTS;
    private readonly string _urlValidaComprobanteBCTS;
    private readonly string _urlEnviarComprobanteBCTS;
    private readonly string _username;
    private readonly string _password;
    private readonly string _grantType;

    public ProveedorBCTSService(WebServiceBCTSConfig webServiceBctsConfig)
    {
        _urlGetTokenBCTS = webServiceBctsConfig.URLGetTokenBCTS;
        _urlValidaComprobanteBCTS = webServiceBctsConfig.URLValidaComprobanteBCTS;
        _urlEnviarComprobanteBCTS = webServiceBctsConfig.URLEnviarComprobanteBCTS;
        _username = webServiceBctsConfig.User.UserName;
        _password = webServiceBctsConfig.User.Password;
        _grantType = webServiceBctsConfig.User.GrantType;
    }

    public async Task<string> GetAccessTokenBCTS()
    {
        var data = new Dictionary<string, string>
        {
            { "username", _username },
            { "password", _password },
            { "grant_type", _grantType }
        };

        using var httpClient = new HttpClient();

        // Realizar la solicitud POST y obtener el token
        var postData = new FormUrlEncodedContent(data);
        HttpResponseMessage response = null;

        try
        {
            response = await httpClient.PostAsync(_urlGetTokenBCTS, postData);
            // Resto del código...
        }
        catch (Exception ex)
        {
            // Manejar o imprimir la excepción
            Console.WriteLine("Excepción al realizar la solicitud: " + ex.Message);
        }


        // Verificar si la solicitud fue exitosa (código 200 OK)
        if (response.IsSuccessStatusCode)
        {
            // Leer y deserializar la respuesta JSON
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            System.Diagnostics.Debug.WriteLine("Token: " + tokenResponse?.access_token);
            // Retornar el access token
            return tokenResponse?.access_token;
        }
        else
        {
            // Manejar el error según sea necesario
            System.Diagnostics.Debug.WriteLine("Error al obtener el token. Código de estado: " + response.StatusCode);
            return null;
        }
    }

    public async Task<string> ValidaComprobanteBCTS(string token, 
                                               string ruc, 
                                               string razonSocial, 
                                               string nombreArchivo,
                                               string data,
                                               string embarque)
    {
        try
        {
            var jsonBody = $@"{{
                ""ListaEmpresasXConjunto"": {{
                    ""INKA"": [
                        {{
                            ""Proveedor"": ""{ruc}"",
                            ""NIT"": ""{ruc}"",
                            ""RazonSocial"": """"
                        }}
                    ]
                }},
                ""NombreArchivo"": ""{nombreArchivo}"",
                ""Data"": ""{data}"",
                ""DocumentoReferencia"": ""{embarque}""
            }}";

            // Crear HttpClient
            using var httpClient = new HttpClient();

            // Configurar el encabezado de autorización Bearer
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Configurar el contenido de la solicitud
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Enviar la solicitud POST
            var response = await httpClient.PostAsync(_urlValidaComprobanteBCTS, content);

            // Verificar si la solicitud fue exitosa
            response.EnsureSuccessStatusCode();

            // Leer y manejar la respuesta como cadena JSON
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJSON = JsonConvert.DeserializeObject<ValidaComprobanteBCTSResponse>(responseContent);
            
            System.Diagnostics.Debug.WriteLine("Token: " + responseJSON?.Error);
            // Retornar el access token
            return responseJSON!.Error;
        }
        catch (HttpRequestException ex)
        {
            // Manejar errores de la solicitud HTTP
            System.Diagnostics.Debug.WriteLine($"Error en la solicitud HTTP: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Manejar otros errores
            System.Diagnostics.Debug.WriteLine($"Error inesperado: {ex.Message}");
        }

        // Devolver null o un valor predeterminado en caso de error
        return null;
    }


    public async Task<string> EnviarComprobanteBCTS(string token,
                                                    string ruc,
                                                    string nombreArchivo,
                                                    string data,
                                                    string embarque,
                                                    List<Adjunto> listaAdjuntos)
    {
        try
        {
            // Crear la estructura del JSON con la lista de adjuntos dinámica
            var jsonBody = $@"{{
                ""ListaEmpresasXConjunto"": {{
                    ""INKA"": [
                        {{
                            ""Proveedor"": ""{ruc}"",
                            ""NIT"": ""{ruc}"",
                            ""RazonSocial"": """"
                        }}
                    ]
                }},
                ""NombreArchivo"": ""{nombreArchivo}"",
                ""Data"": ""{data}"",
                ""DocumentoReferencia"": ""{embarque}"",
                ""Usuario"": ""usuariobcts@bctsconsulting.com"",
                ""ListaAdjuntos"": {JsonConvert.SerializeObject(listaAdjuntos)}
            }}";
            // Crear HttpClient
            using var httpClient = new HttpClient();

            // Configurar el encabezado de autorización Bearer
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Configurar el contenido de la solicitud
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Enviar la solicitud POST
            var response = await httpClient.PostAsync(_urlEnviarComprobanteBCTS, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseJSON = JsonConvert.DeserializeObject<EnviarComprobanteBCTSResponse>(responseContent);

                System.Diagnostics.Debug.WriteLine("Token: " + responseJSON?.error);
                return responseJSON?.error;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    // Intenta deserializar la respuesta de error como un objeto ErrorResponse
                    var errorResponseJSON = JsonConvert.DeserializeObject<EnviarComprobanteBCTSResponse>(errorResponseContent);
                    return errorResponseJSON?.error;
                }
                catch (JsonException)
                {
                    // Si hay un problema al deserializar, simplemente devuelve el contenido como texto
                    return errorResponseContent;
                }
            }
            else
            {
                // Otros códigos de estado no manejados
                response.EnsureSuccessStatusCode();
                return null; // O manejar según tus necesidades
            }
        }
        catch (HttpRequestException ex)
        {
            // Manejar errores de la solicitud HTTP
            return ex.Message;
        }
        catch (Exception ex)
        {
            // Manejar otros errores
            return ex.Message;
        }
    }
}
