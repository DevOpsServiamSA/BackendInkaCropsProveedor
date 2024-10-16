public class TutorialVideo
{
    public int id { get; set; }
    public string nombre { get; set; } = null!;
    public string titulo { get; set; } = null!;
    public string descripcion { get; set; } = null!;
    public short orden { get; set; }
    public int modulo_id { get; set; }
    public int perfil_id { get; set; }
    public bool publico { get; set; }
    public string active { get; set; } = null!;
    public string created_by { get; set; } = null!;
    public DateTime created_at { get; set; }
    public string? updated_by { get; set; }
    public DateTime? updated_at { get; set; }
    public string? deleted_by { get; set; }
    public DateTime? deleted_at { get; set; }
}