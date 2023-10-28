using API.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;

        public PresenceHub(PresenceTracker presenceTracker)
        {
            _presenceTracker = presenceTracker;
        }
        public override async Task OnConnectedAsync()
        {
            var isOnline = await _presenceTracker.UserConnected(Context.User!.GetUsername(), Context.ConnectionId);
            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User!.GetUsername());

            var onlineUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", onlineUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var isOffline = await _presenceTracker.UserDisconnected(Context.User!.GetUsername(), Context.ConnectionId);
            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User!.GetUsername());
            await base.OnDisconnectedAsync(exception);
        }
    }
}