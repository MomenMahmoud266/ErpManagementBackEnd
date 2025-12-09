namespace ErpManagement.Services.Services.WebSocket;

[Authorize(RequestClaims.DomainRestricted)]
public class BroadcastHub : Hub<IHubClient>
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Come2Chat");
        await Clients.Caller.SendAsync("UserConnected");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Come2Chat");
        await base.OnDisconnectedAsync(exception);
    }
}
