using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProveedorApi.Helpers
{
    public class MailManagerHelper
    {
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
            bool status = false;
            try
            {
                MailMessage email = new MailMessage();
                email.From = new MailAddress(AppConfig.Configuracion.UserMail);
                email.Subject = asunto;
                email.Body = mensaje;
                email.IsBodyHtml = esHtlm;

                string[] destinatarios = destinatario.Replace(";", ",").Split(',');
                foreach (string toEmail in destinatarios)
                {
                    if (!UtilityHelper.IsValidEmail(toEmail)) continue;
                    email.To.Add(new MailAddress(toEmail));
                }

                if (email.To.Count == 0) return false;

                if (attachList != null)
                {
                    if (attachList.Count > 0)
                    {
                        attachList.ForEach(att =>
                        {
                            email.Attachments.Add(att);
                        });

                    }
                }

                await cliente.SendMailAsync(email);
                cliente.Dispose();
                email.Dispose();
                status = true;
            }
            catch (System.Exception)
            {
                status = false;
            }
            return status;
        }
    }
}