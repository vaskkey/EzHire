using Microsoft.EntityFrameworkCore;

namespace ezhire_api.DAL;

public class EzHireContext : DbContext
{
    protected EzHireContext()
    {
    }
    
    public EzHireContext(DbContextOptions options) : base(options)
    {
    }
}