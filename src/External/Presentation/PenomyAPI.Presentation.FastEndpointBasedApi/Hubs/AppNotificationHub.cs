using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Hubs;

// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AppNotificationHub : Hub
{
    public const string connectPath = "/signalr/app";

    public override async Task OnConnectedAsync()
    {
        //var identity = new ClaimsIdentity(new List<Claim>
        //{
        //    new Claim("Sub", "11805760530993152"),
        //    new Claim("Jti", "11805760530993152")
        //}, "Custom");

        //Context.User.AddIdentity(identity);
        var claims = Context.User.Claims;

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}
