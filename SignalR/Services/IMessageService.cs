using SignalR.Models;

namespace SignalR.Services;

public interface IMessageService
{
    Task<Message> AddMessageAsync(string user, string text, CancellationToken ct);
    Task<IEnumerable<Message>> GetHistoryAsync(CancellationToken ct);
    Task<Message?> UpdateMessageAsync(int id, string newText, CancellationToken ct);
    Task<bool> DeleteMessageAsync(int id, CancellationToken ct);
}