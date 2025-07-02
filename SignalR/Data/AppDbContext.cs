using Microsoft.EntityFrameworkCore;
using SignalR.Models;

namespace SignalR.Data;

public class AppDbContext : DbContext
{
    public DbSet<Message> Messages => Set<Message>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}