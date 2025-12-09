namespace ErpManagement.Services.IServices.WebSocket;

public interface IHubClient
{
    Task BroadcastMessage();
    Task SendAsync(string txt);
}
