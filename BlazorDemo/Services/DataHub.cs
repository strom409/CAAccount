// DataHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class DataHub : Hub
{
    // This will track all connected clients
    private static readonly HashSet<string> ConnectedClients = new();

    public override async Task OnConnectedAsync()
    {
        ConnectedClients.Add(Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        ConnectedClients.Remove(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    // Method to broadcast to all clients
    public async Task BroadcastDataUpdate(string dataType)
    {
        await Clients.All.SendAsync("ReceiveDataUpdate", dataType);
    }

    // Method to broadcast to specific group
    public async Task NotifyDataChanged(string dataType)
    {
        await Clients.Group(dataType).SendAsync("ReceiveDataUpdate", dataType);
    }
}