using Microsoft.AspNetCore.SignalR;
using SignalR.Models;
using SignalR.Services;

namespace SignalR.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageService _service;

    public ChatHub(IMessageService service)
    {
        _service = service;
    }

    public async Task Ping(string user)
    {
        Console.WriteLine("Пинг получен");
    }

    public async Task SendMessage(string user, string text)
    {
        try
        {
            var msg = await _service.AddMessageAsync(user, text, ct);
            await Clients.All.SendAsync("ReceiveMessage", msg, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Message>> GetHistory()
        => await _service.GetHistoryAsync(ct);

    public async Task UpdateMessage(int id, string newText)
    {
        var msg = await _service.UpdateMessageAsync(id, newText, ct);
        if (msg != null)
            await Clients.All.SendAsync("MessageUpdated", msg, ct);
    }

    public async Task DeleteMessage(int id)
    {
        var deleted = await _service.DeleteMessageAsync(id, ct);
        if (deleted)
            await Clients.All.SendAsync("MessageDeleted", id, ct);
    }
}