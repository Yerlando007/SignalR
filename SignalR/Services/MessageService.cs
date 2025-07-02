using Microsoft.EntityFrameworkCore;
using SignalR.Data;
using SignalR.Models;

namespace SignalR.Services;

public class MessageService : IMessageService
{
    private readonly AppDbContext _context;

    public MessageService(AppDbContext context) => _context = context;

    public async Task<Message> AddMessageAsync(string user, string text, CancellationToken ct)
    {
        var msg = new Message { User = user, Text = text };
        _context.Messages.Add(msg);
        await _context.SaveChangesAsync(ct);
        return msg;
    }

    public async Task<IEnumerable<Message>> GetHistoryAsync(CancellationToken ct)
        => await _context.Messages.OrderBy(x => x.Timestamp).ToListAsync(ct);

    public async Task<Message?> UpdateMessageAsync(int id, string newText, CancellationToken ct)
    {
        var msg = await _context.Messages.FindAsync([id], ct);
        if (msg == null) return null;

        msg.Text = newText;
        await _context.SaveChangesAsync(ct);
        return msg;
    }

    public async Task<bool> DeleteMessageAsync(int id, CancellationToken ct)
    {
        var msg = await _context.Messages.FindAsync([id], ct);
        if (msg == null) return false;

        _context.Messages.Remove(msg);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}