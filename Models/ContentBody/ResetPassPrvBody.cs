namespace ProveedorApi.Models.ContentBody;
public class ResetPassPrvBody
{
    public string token { get; set; } = null!;
    public string password { get; set; } = null!;
    public string confirmPassword { get; set; } = null!;
}