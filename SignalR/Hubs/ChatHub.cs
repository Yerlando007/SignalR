using Microsoft.AspNetCore.SignalR;
using SignalR.Models;
using SignalR.Services;

namespace SignalR.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageService _service;

    public ChatHub(IMessageService service) => _service = service;

    public async Task Ping(string user)
    {
        Console.WriteLine("Пинг получен");
    }

    public async Task SendMessage(string user, string text)
    {
        var msg = await _service.AddMessageAsync(user, text, Context.ConnectionAborted);
        await Clients.All.SendAsync("ReceiveMessage", msg);
    }

    public async Task<IEnumerable<Message>> GetHistory()
        => await _service.GetHistoryAsync(Context.ConnectionAborted);

    public async Task UpdateMessage(int id, string newText)
    {
        var msg = await _service.UpdateMessageAsync(id, newText, Context.ConnectionAborted);
        if (msg != null)
            await Clients.All.SendAsync("MessageUpdated", msg);
    }

    public async Task DeleteMessage(int id)
    {
        var deleted = await _service.DeleteMessageAsync(id, Context.ConnectionAborted);
        if (deleted)
            await Clients.All.SendAsync("MessageDeleted", id);
    }
}