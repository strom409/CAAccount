using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class GlobalDataNotifier
{
    private readonly IHubContext<DataHub> _hubContext;

    public GlobalDataNotifier(IHubContext<DataHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyChanged(string entityType)
    {
        await _hubContext.Clients.All.SendAsync("DataChanged", entityType);
    }
}