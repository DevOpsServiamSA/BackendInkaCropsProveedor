namespace ProveedorApi.Models;

public class ProveedorBCTSResponse
{
    public bool Success { get; set; }
    public string ChallengeTs { get; set; }
    public string Hostname { get; set; }
    public List<string> ErrorCodes { get; set; }
}

public class TokenResponse
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string token_type { get; set; }
}

public class ValidaComprobanteBCTSResponse
{
    public string RazonSocialEmisor { get; set; }
    public string Error { get; set; }
    public string Tipo { get; set; }
    public string TipoDocumentoSUNAT { get; set; }
}

public class EnviarComprobanteBCTSResponse
{
    public bool Success { get; set; }
    public string ChallengeTs { get; set; }
    public string Hostname { get; set; }
    public List<string> ErrorCodes { get; set; }
}

public class Adjunto
{
    public string NombreArchivo { get; set; }
    public string Data { get; set; }
}