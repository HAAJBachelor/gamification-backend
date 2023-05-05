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

    private async Task sendMessage(WebSocket webSocket, Message message, CancellationToken cancellationToken,
        byte[] buffer)
    {
        var json = JsonSerializer.Serialize(message);
        buffer = Encoding.UTF8.GetBytes(json);
        var size = Encoding.UTF8.GetByteCount(json);

        var msg = new ArraySegment<byte>(buffer, 0, size);
        await webSocket.SendAsync(msg, WebSocketMessageType.Text, true, cancellationToken);
    }

    private async Task SendNumbersAsync(WebSocket webSocket)
    {
        var cancellationToken = new CancellationToken();
        var buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            var guid = Guid.Parse(HttpContext.Session.GetString(GameController.SessionId));
            var timeLeft = GameManager.Instance().GetSessionTime(guid);
            Message msg;
            msg = timeLeft <= 0 ? Message.CreateStateChange("Finished") : Message.CreateTime(timeLeft.ToString());
            await sendMessage(webSocket, msg, cancellationToken, buffer);
            var scores = GameManager.Instance().GetScore(guid);
            msg = Message.CreateScore(scores.ToString());
            await sendMessage(webSocket, msg, cancellationToken, buffer);
            var lives = GameManager.Instance().GetState(guid)._lives;
            msg = Message.CreateSkip(lives.ToString());
            var state = GameManager.Instance().GetState(guid);
            await sendMessage(webSocket, msg, cancellationToken, buffer);
            if (timeLeft <= 0)
                break;
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

        public static Message CreateTime(string data)
        {
            return new Message(Type.Time, data);
        }

        public static Message CreateSkip(string data)
        {
            return new Message(Type.Skip, data);
        }

        public static Message CreateScore(string data)
        {
            return new Message(Type.Score, data);
        }

        public static Message CreateStateChange(string data)
        {
            return new Message(Type.StateChange, data);
        }

        internal enum Type
        {
            Time,
            StateChange,
            Skip,
            Score
        }
    }
}