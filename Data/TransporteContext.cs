using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models.ContentResponse;

namespace ProveedorApi.Data;

public class TransporteContext : DbContext
{
    public TransporteContext(DbContextOptions<TransporteContext> options) : base(options) { }

    #region Content Response (Store procedure)
    public DbSet<TransporteSegResponse> TransporteSegResponse => Set<TransporteSegResponse>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Store Procedure Query Select
        modelBuilder.Entity<TransporteSegResponse>().HasNoKey();
        #endregion
    }
}