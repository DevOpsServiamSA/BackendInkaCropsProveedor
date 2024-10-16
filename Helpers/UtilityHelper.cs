using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProveedorApi.Helpers
{
    public static class UtilityHelper
    {
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }


        public static bool DeleteFile(string fileFullName)
        {
            try
            {
                if (System.IO.File.Exists(fileFullName))
                {
                    System.IO.File.Delete(fileFullName);
                    return true;
                }

            }
            catch (System.Exception)
            {
                return false;
            }
            return false;
        }
        public static async Task<bool> UploadFileAsync(string path, string fileFullName, byte[] fileByte)
        {

            bool status = false;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (System.IO.File.Exists(fileFullName))
                {
                    System.IO.File.Delete(fileFullName);
                }
                await System.IO.File.WriteAllBytesAsync(fileFullName, fileByte);

                status = true;
            }
            catch (System.Exception)
            {
                status = false;
            }

            return status;
        }
        public static async Task<bool> UploadFormFileAsync(string path, string _filename, IFormFile _file)
        {

            bool status = false;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var stream = new FileStream(Path.Combine(path, _filename), FileMode.Create))
                {
                    await _file.CopyToAsync(stream);
                }
                status = true;
            }
            catch (System.Exception)
            {
                status = false;
            }

            return status;
        }

        public static async Task<string> GetFileUrlB64Async(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                return string.Empty;
            }

            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string? contentType;
            if (!provider.TryGetContentType(filename, out contentType))
            {
                contentType = "application/octet-stream";
            }

            byte[] _filebyte = await System.IO.File.ReadAllBytesAsync(filename);

            if (_filebyte.Length == 0)
            {
                return string.Empty;
            }

            string base64 = Convert.ToBase64String(_filebyte);

            return $"data:{contentType};base64,{base64}";
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static string CreateRandomPassword(int length = 15)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);

        }
        public static DateTime ExpireToken(string p_expire)
        {
            DateTime Now = DateTime.Now;
            DateTime expire_token = DateTime.MinValue;
            string expires_token_loc = p_expire ?? "1h";
            short expire = Int16.Parse(expires_token_loc[0..^1]);
            char type_expire = expires_token_loc[^1];

            expire_token = type_expire switch
            {
                'm' => DateTime.UtcNow.AddMinutes(expire),
                'h' => DateTime.UtcNow.AddHours(expire),
                'd' => DateTime.UtcNow.AddDays(expire),
                'y' => DateTime.UtcNow.AddYears(expire),
                _ => DateTime.UtcNow.AddHours(1),
            };
            return expire_token;
        }
    }
}