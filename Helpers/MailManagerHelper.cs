using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Attachment = System.Net.Mail.Attachment;

namespace ProveedorApi.Helpers
{
    public class MailManagerHelper
    {
        public static string userEmail = AppConfig.Configuracion.UserMail;
        public static string clientId = AppConfig.Configuracion.ClientId;
        public static string clientSecret = AppConfig.Configuracion.ClientSecret;
        public static string tenantId = AppConfig.Configuracion.TenantId;
        
        private SmtpClient cliente;
        public MailManagerHelper()
        {
            cliente = new SmtpClient(AppConfig.Configuracion.ServidorMail, AppConfig.Configuracion.PuertoMail)
            {
                EnableSsl = AppConfig.Configuracion.EnableSSLMail,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(AppConfig.Configuracion.UserMail, AppConfig.Configuracion.PasswordMail)
            };
        }
        public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string mensaje, List<Attachment>? attachList = null, bool esHtlm = false)
        {
            try
            {
                // Obtener token de acceso
                string accessToken = await GetTokenOAuthAsync();

                // Configurar GraphServiceClient
                var graphClient = new GraphServiceClient(
                    new DelegateAuthenticationProvider(requestMessage =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        return Task.CompletedTask;
                    })
                );

                // Crear el mensaje de correo
                var message = new Message
                {
                    Subject = asunto,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = mensaje
                    },
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = destinatario
                            }
                        }
                    }
                };

                // if (attachList != null)
                // {
                //     if (attachList.Count > 0)
                //     {
                //         attachList.ForEach(att =>
                //         {
                //             message.Attachments.Add(att);
                //         });
                //
                //     }
                // }

                // Enviar el correo utilizando Microsoft Graph API
                await graphClient.Users[userEmail]
                    .SendMail(message, null) // null indica que no se guardará una copia en la carpeta 'Sent'
                    .Request()
                    .PostAsync();

                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error enviando correo: {ex.Message}");
                return false;
            }
        }
        
        private async Task<string> GetTokenOAuthAsync()
        {
            var confidentialClient = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var authResult = await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync();
            return authResult.AccessToken;
        }
    }
    
    
}