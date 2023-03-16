using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using gamification_backend.Game;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers;

public class WebSocketController : Controller
{
    [Route("/ws")]
    [HttpGet]
    public async Task InvokeAsync()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await SendNumbersAsync(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
        }
    }

    private async Task SendNumbersAsync(WebSocket webSocket)
    {
        var cancellationToken = new CancellationToken();
        var buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            var number = GameManager.Instance()
                .GetSessionTime(Guid.Parse(HttpContext.Session.GetString(GameController.SessionId)));
            Message msg;
            msg = number <= 0 ? Message.CreateStateChange("Finished") : Message.CreateUpdate(number.ToString());
            var json = JsonSerializer.Serialize(msg);
            buffer = Encoding.UTF8.GetBytes(json);
            var size = Encoding.UTF8.GetByteCount(json);

            var message = new ArraySegment<byte>(buffer, 0, size);
            await webSocket.SendAsync(message, WebSocketMessageType.Text, true, cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket closed", cancellationToken);
    }

    private class Message
    {
        private Type _type;

        private Message(Type type, string message)
        {
            _type = type;
            data = message;
        }

        public string type
        {
            get => _type.ToString();
        }

        public string data { get; set; }

        public static Message CreateUpdate(string data)
        {
            return new Message(Type.Update, data);
        }

        public static Message CreateStateChange(string data)
        {
            return new Message(Type.StateChange, data);
        }

        internal enum Type
        {
            Update,
            StateChange,
        }
    }
}