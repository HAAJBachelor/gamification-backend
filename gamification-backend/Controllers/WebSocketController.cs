using System.Net.WebSockets;
using gamification_backend.Game;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers;

public class WebSocketController : Controller
{
    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var id = HttpContext.Session.GetInt32(GameController.SessionId);
        while (true)
        {
            if (!id.HasValue)
            {
                id = HttpContext.Session.GetInt32(GameController.SessionId);
                Thread.Sleep(100);
                continue;
            }

            var state = GameManager.Instance().GetState(id.Value);
            buffer = BitConverter.GetBytes(state._time);

            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, sizeof(int)),
                WebSocketMessageType.Binary,
                true,
                CancellationToken.None);
            Thread.Sleep(1000);
        }
    }
}